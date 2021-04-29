using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using Herlitz.BankID.Business.Helpers;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.DependencyInjection;

namespace Herlitz.BankID
{
    public class BankIDHttpClientHandler : HttpClientHandler
    {
        private readonly BankIDServiceSettings _bankIdServiceSettings;
        private readonly TelemetryClient _telemetryClient;

        //public BankIDHttpClientHandler()
        //{
        //    _telemetryClient = new TelemetryClient();
        //}

        public BankIDHttpClientHandler(BankIDServiceSettings bankIdServiceSettings, TelemetryClient telemetryClient)
        {
            //TelemetryConfiguration.Active
            _telemetryClient = telemetryClient; // new TelemetryClient();
            _bankIdServiceSettings = bankIdServiceSettings;
            //var bankIdServiceSettings = new BankIDServiceSettings(env);

            AllowAutoRedirect = false;
            AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            SslProtocols = SslProtocols.Tls12;

            // Hosting dependent config
            var hosting = Environment.GetEnvironmentVariable("ASPNETCORE_HOSTING");
            if (!string.IsNullOrEmpty(hosting) && hosting.Equals("Azure"))
            {
                // https://docs.microsoft.com/en-us/azure/app-service/app-service-web-ssl-cert-load


                // server certificate
                X509Store certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                certStore.Open(OpenFlags.ReadOnly);

                X509Certificate2Collection caCertCollection = certStore.Certificates.Find(
                    X509FindType.FindByThumbprint,
                    _bankIdServiceSettings.CaCertThumbprint,
                    false);

                if (caCertCollection.Count > 0)
                {
                    X509Certificate2 caCert = caCertCollection[0];
                    var validator = new RootCaValidator(caCert);
                    ServerCertificateCustomValidationCallback = validator.Validate;

                    _telemetryClient.TrackTrace($"Loaded CA Cert {caCert.SubjectName?.Name}, {caCert.Thumbprint}", SeverityLevel.Information);
                }
                else
                {
                    _telemetryClient.TrackTrace($"Could not load CA Cert", SeverityLevel.Critical);
                }

                X509Certificate2Collection rpCertCollection = certStore.Certificates.Find(
                    X509FindType.FindByThumbprint,
                    _bankIdServiceSettings.RpCertThumbprint,
                    false);

                if (rpCertCollection.Count > 0)
                {
                    X509Certificate2 rpCert = rpCertCollection[0];
                    ClientCertificates.Add(rpCert);
                    _telemetryClient.TrackTrace($"Loaded RP Cert {rpCert.SubjectName?.Name}, {rpCert.Thumbprint}", SeverityLevel.Information);
                }
                else
                {
                    _telemetryClient.TrackTrace($"Could not load RP Cert", SeverityLevel.Critical);
                }
                _telemetryClient.Flush();
                certStore.Close();
            }
            else
            {
                // Local
                SecureString secureString = new SecureString();
                "qwerty123".ToCharArray().ToList().ForEach(p => secureString.AppendChar(p));

                var rootCa = new X509Certificate2(_bankIdServiceSettings.CaCert);
                var validator = new RootCaValidator(rootCa);
                ServerCertificateCustomValidationCallback = validator.Validate;

                ClientCertificates.Add(new X509Certificate2(_bankIdServiceSettings.RpCert, secureString));
            }

        }
    }

    public static class ServiceCollectionExtensions
    {
        public static void AddBankID(this IServiceCollection services)
        {
            services.AddSingleton<BankIDServiceSettings>();
            services.AddTransient<BankIDHttpClientHandler>();
            services.AddHttpClient("BankIDClient", client =>
            {
                // required to use ReadAsAsync
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
                {
                    NoCache = true,
                    NoStore = true,
                    MaxAge = new TimeSpan(0),
                    MustRevalidate = true
                };
            }).ConfigurePrimaryHttpMessageHandler(service =>
            {
                var bankIdHttpClientHandler = service.GetService<BankIDHttpClientHandler>();

                return bankIdHttpClientHandler;
            });


            services.AddTransient<IBankIDService, BankIDService>();
            services.AddTransient<IBankIDHttpClientService, BankIDHttpClientService>();

            services.AddTransient<ISSNHelper, SSNHelper>();
            services.AddTransient<IStatusHandler, StatusHandler>();
            services.AddTransient<IAuthRequest, AuthRequest>();
            services.AddTransient<ISignRequest, SignRequest>();
            services.AddTransient<ICancelRequest, CancelRequest>();
            services.AddSingleton<ICollectRequest, CollectRequest>();
            services.AddTransient<ICompletionData, CompletionData>();
            services.AddTransient<IRequirement, Requirement>();
            services.AddTransient<IQRCodeFactory, QRCodeFactory>();
        }
    }
}
