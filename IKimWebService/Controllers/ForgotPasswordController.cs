using IKimWebService.Infrastructure.CustomException;
using IKimWebService.Infrastructure.IService;
using System.Net.Http;
using System.Web.Http;
using System.Net;
using System;

namespace IKimWebService.Controllers
{
    public class ForgotPasswordController : ApiController
    {

        public readonly IForgotPasswordService _service;

        public ForgotPasswordController(IForgotPasswordService service)
        {
            _service = service;
        }


        [HttpPost]
        public HttpResponseMessage Initiate(Domain.ForgotPassword mForgotPassword)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                _service.Initiate(mForgotPassword);
                response = Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (APIRequestFailedException ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            return response;
        }

    }
}
