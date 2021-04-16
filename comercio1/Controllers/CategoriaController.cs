using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Udi.Comercio.Data.Domain.Services;

namespace comercio1.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly CategoriaServicio categoriaServicio = new CategoriaServicio();
        // GET: Categoria
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GuardarCategoria(int id,string nombre,string descripcion)
        {
            try
            {
                id = categoriaServicio.GuardarCategoria(id, nombre, descripcion);
                return new JsonResult { Data = new{Success = true, Data = id }};
            }
            catch(Exception ex)
            {
                return new JsonResult { Data = new { Success = false, Data = "",Mensaje = ex.Message } };
            }
        }
        public JsonResult EliminarCategoria(int id)
        {
            try
            {
                categoriaServicio.EliminarCategoria(id);
                return new JsonResult { Data = new { Success = true} };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = new { Success = false, Data = "", Mensaje = ex.Message } };
            }
        }
        public JsonResult ObtenerCategoria(int id)
        {
            try
            {
                var data =categoriaServicio.ObtenerCategoria(id);
                return new JsonResult { Data = new { Success = true,Data = data } };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = new { Success = false, Data = "", Mensaje = ex.Message } };
            }
        }
        public JsonResult ObtenerCategorias()
        {
            try
            {
                var data = categoriaServicio.ObtenerCategorias();
                return new JsonResult { Data = new { Success = true, Data = data } };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = new { Success = false, Data = "", Mensaje = ex.Message } };
            }
        }

    }
}