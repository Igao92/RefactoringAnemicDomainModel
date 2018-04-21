using Logic.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Controllers
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
            return base.Ok(); 
        }

        protected IActionResult Ok<T>(T result)
        {
            _unityOfWork.Commit();
            return base.Ok(result);
        }

        protected IActionResult Error(string errorMessage)
        {
            return base.BadRequest(errorMessage);
        }
    }
}
