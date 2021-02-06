using RMS.Component.WebApi.Responses;
using RMS.Core.Security;
using RMS.Parser;
using RMS.Server.DataTypes.Requests;
using RMS.Server.DataTypes.Responses;
using RMS.Server.WebApi.Configuration;
using System;
using System.Web.Http;

namespace RMS.Server.WebApi.Controllers
{
    //For Super Admin, the userName is "sa", and the password is "$pkH1&*^"
    [Route("api/token")]
    public class TokenController : ApiController
    {
        [HttpPost]
        [AllowAnonymous]
        public TokenResponse Generate(TokenRequest request)
        {
            TokenResponse response = new TokenResponse();
            response.RequestId = request.RequestId;
            try
            {
                var user = UserManager.Instance.FindUserByUserName(request.UserName);

                if (user != null)
                {
                    bool passOk = HashGenerator.VerifyHash(request.Password, user.PasswordHash);

                    if (passOk)
                    {
                        string jwt = Jwt.JwtManager.GenerateToken(user.UserName);

                        if (!string.IsNullOrWhiteSpace(jwt))
                        {
                            response.Token = jwt;
                            response.ResponseStatus = ResponseStatus.Success;
                            response.Message = "Token Successfully Generated";
                        }
                        else
                        {

                            response.ResponseStatus = ResponseStatus.Failed;
                            response.Message = "JWT Generator returned empty string";
                        }
                    }
                    else
                    {
                        response.ResponseStatus = ResponseStatus.Failed;
                        response.Message = "Invalid Credentials";
                    }
                }
                else
                {
                    response.ResponseStatus = ResponseStatus.Failed;
                    response.Message = "Invalid Credentials";
                }
            }
            catch (Exception ex)
            {
                response.ResponseStatus = ResponseStatus.Failed;
                response.Message = ex.Message;
            }


            return response;
        }
    }
}