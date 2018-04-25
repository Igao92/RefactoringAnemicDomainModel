using Logic.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Api.Utils
{
    public class BaseController : Controller
    {
        private readonly UnitOfWork _unityOfWork;

        public BaseController(UnitOfWork unitOfWork)
        {
            _unityOfWork = unitOfWork;
        }

        protected new IActionResult Ok()
        {
            _unityOfWork.Commit();
            return base.Ok(Envelope.Ok()); 
        }

        protected IActionResult Ok<T>(T result)
        {
            _unityOfWork.Commit();
            return base.Ok(Envelope.Ok(result));
        }

        protected IActionResult Error(string errorMessage)
        {
            return base.BadRequest(Envelope.Error(errorMessage));
        }
    }
}
