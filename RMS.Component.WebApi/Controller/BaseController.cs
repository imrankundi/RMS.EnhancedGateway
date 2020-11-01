
using RMS.Component.WebApi.Responses;
using System.Web.Http;

namespace RMS.Component.WebApi.Controller
{
    public abstract class BaseController : ApiController
    {
        public abstract IResponse Post(object request);
    }
}
