using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace comercio1.Controllers
{
    public class AdministracionController : Controller
    {
        public ActionResult Producto()
        {
            ViewBag.Message = "Esta es mi segunda vista";

            return View();
        }
        public ActionResult Categoria()
        {
            ViewBag.Message = "Esta es mi segunda vista";

            return View();
        }
    }
}