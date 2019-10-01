using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Herlitz.BankID.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankIDController : ControllerBase
    {
        private readonly IBankIDService _bankIdService;
        private readonly IAuthRequest _authRequest;
        private readonly ISignRequest _signRequest;
        private readonly ICancelRequest _cancelRequest;
        private readonly ICollectRequest _collectRequest;
        private readonly IStatusHandler _statusHandler;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BankIDController(
            IBankIDService bankIdService,
            IAuthRequest authRequest,
            ISignRequest signRequest,
            ICancelRequest cancelRequest,
            ICollectRequest collectRequest,
            IStatusHandler statusHandler,
            IHttpContextAccessor httpContextAccessor)
        {
            _bankIdService = bankIdService;
            _authRequest = authRequest;
            _signRequest = signRequest;
            _cancelRequest = cancelRequest;
            _collectRequest = collectRequest;
            _statusHandler = statusHandler;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Start authentication client without personal number
        /// </summary>
        /// <param name="ip">
        /// The user IP address as seen by RP. String. IPv4 and IPv6 is allowed.
        /// Note the importance of using the correct IP address. It must be the IP address
        /// representing the user agent (the end user device) as seen by the RP.
        /// If there is a proxy for inbound traffic, special considerations may need to be
        /// taken to get the correct address. In some use cases the IP address is not available,
        /// for instance for voice based services. In this case, the internal representation
        /// of those systems IP address is ok to use. 
        /// </param>
        /// <returns></returns>
        [HttpGet("AuthAutostart")]
        public async Task<ActionResult<IAuthResponse>> GetAuthAutostart(string ip)
        {
            _authRequest.EndUserIp = ip;
            IAuthResponse response = await _bankIdService.Auth(_authRequest);

            return new ActionResult<IAuthResponse>(response);
        }

        [HttpGet("SignAutostart")]
        public async Task<ActionResult<IAuthResponse>> GetSignAutostart(string ip, string userVisibleData, string userNonVisibleData = "")
        {
            _signRequest.EndUserIp = ip;
            _signRequest.UserVisibleData = userVisibleData;

            if (!string.IsNullOrEmpty(userNonVisibleData))
            {
                _signRequest.UserNonVisibleData = userNonVisibleData;
            }

            IAuthResponse response = await _bankIdService.Sign(_signRequest);

            return new ActionResult<IAuthResponse>(response);
        }


        // Auth
        [HttpGet("Auth")]
        public async Task<ActionResult<IAuthResponse>> GetAuth(string ip, string personalNumber)
        {
            _authRequest.EndUserIp = ip;
            _authRequest.PersonalNumber = personalNumber;
            IAuthResponse response = await _bankIdService.Auth(_authRequest);

            return new ActionResult<IAuthResponse>(response);
        }

        // Sign
        [HttpGet("Sign")]
        public async Task<ActionResult<IAuthResponse>> GetSign(string ip, string personalNumber, string userVisibleData, string userNonVisibleData = "")
        {
            _signRequest.EndUserIp = ip;
            _signRequest.PersonalNumber = personalNumber;
            _signRequest.UserVisibleData = userVisibleData;

            if (!string.IsNullOrEmpty(userNonVisibleData))
            {
                _signRequest.UserNonVisibleData = userNonVisibleData;
            }

            IAuthResponse response = await _bankIdService.Sign(_signRequest);

            return new ActionResult<IAuthResponse>(response);
        }

        // Cancel
        [HttpGet("Cancel")]
        public async Task<ActionResult<bool>> GetCancel(string orderRef)
        {
            _cancelRequest.OrderRef = orderRef;
            bool response = await _bankIdService.Cancel(_cancelRequest);

            return new ActionResult<bool>(response);
        }

        // Collect
        /// <summary>
        /// Collect request from BankID
        /// </summary>
        /// <param name="orderRef">Order reference from BankID</param>
        /// <param name="requestToken">Generate tokens from Identity Server</param>
        /// <returns></returns>
        [HttpGet("Collect")]
        public async Task<ActionResult<ICollectResponse>> GetCollect(string orderRef, bool requestToken = false)
        {
            _collectRequest.OrderRef = orderRef;
            ICollectResponse response = await _bankIdService.Collect(_collectRequest);

            return new ActionResult<ICollectResponse>(response);
        }


        // Status
        [HttpGet("Status")]
        public async Task<ActionResult<string>> GetStatus(string hintCode, bool usingAutostart = false)
        {
            var statusMessage = _statusHandler.GetStatus(hintCode, usingAutostart);
            return new ActionResult<string>(statusMessage);
        }

        [HttpGet("ClientIP")]
        public async Task<ActionResult<string>> GetClientIP()
        {
            string forwardedForKey = "X-Forwarded-For";

            // If the headers contain a forwardedKey the code is probably run through a proxy
            // like azure api management or is placed behind a reverse proxy
            if (_httpContextAccessor.HttpContext.Request.Headers.ContainsKey(forwardedForKey))
            {
                // There may be multiple keys, the first should be the ip hitting the edge
                var forwardKey = _httpContextAccessor.HttpContext.Request.Headers[forwardedForKey].FirstOrDefault();
                if (forwardKey != null && forwardKey.Contains(","))
                {
                    // remove all but first address
                    forwardKey = forwardKey.Split(",")[0].Trim();
                }

                if (forwardKey != null && forwardKey.Contains(":"))
                {
                    // remove any port
                    forwardKey = forwardKey.Split(":")[0].Trim();
                }

                if (!string.IsNullOrEmpty(forwardKey))
                {
                    return new ActionResult<string>(forwardKey);
                }
            }

            // Fallback for default context
            var ip = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            return new ActionResult<string>(ip);
        }

    }
}