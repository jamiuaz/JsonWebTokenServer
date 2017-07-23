using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace JsonWebTokenServer.Controllers
{
    public class InfoController : Controller
    {
        public ActionResult Index()
        {
            return Content("Json Web Token Server <br/>  Version 1.0.0 <br/> Copyright @Nooh 2017");
        }
    }
}