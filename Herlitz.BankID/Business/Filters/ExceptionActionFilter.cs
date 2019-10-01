using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Herlitz.BankID
{
    public class ExceptionActionFilter : ExceptionFilterAttribute
    {
        private readonly TelemetryClient _telemetryClient;

        public ExceptionActionFilter(TelemetryClient telemetryClient)
        {
            _telemetryClient = telemetryClient;
        }


        #region Overrides of ExceptionFilterAttribute

        public override Task OnExceptionAsync(ExceptionContext context)
        {
            return base.OnExceptionAsync(context);
        }

        public override void OnException(ExceptionContext context)
        {
            // Report exception to insights
            _telemetryClient.TrackException(context.Exception);
            _telemetryClient.Flush();

            base.OnException(context);
        }

        #endregion
    }


}
