using CityInfo.Application.Authentication;
using CityInfo.Application.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/authentication")]
    [ApiController]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class AuthenticationController : ControllerBase
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        private readonly IAuthenticationContract _authenticationContract;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public AuthenticationController(IAuthenticationContract authenticationContract)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            _authenticationContract = authenticationContract;
        }

        /// <summary>
        /// Authenticate the request
        /// </summary>
        /// <param name="authenticationRequestBody">The name that needs to be authenticated</param>
        /// <returns>An IAction Result</returns>
        /// <response code="200">Authentication Successful</response>
        /// <response code="401">Authentication Unsuccessful</response>

        [HttpPost("authenticate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<string> Authenticate(AuthenticationRequestBody authenticationRequestBody)
        {
            //Step 1: Validate the username/ password
            var user = _authenticationContract.Authenticate(authenticationRequestBody);

            if (user == null)
            {
                return Unauthorized();
            }

            return Ok(user);
        }

    }
}
