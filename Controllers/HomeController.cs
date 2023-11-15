using Microsoft.AspNetCore.Mvc;

namespace ContactManagerStarter.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        //This was to test to see ELMAH picks of the error. It does and logs it in port/elmah as well in Logs folder.
        //public IActionResult ThrowError()
       // {
           // throw new InvalidOperationException("TESTING");
        //}

    }

}
