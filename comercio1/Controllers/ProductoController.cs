using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Udi.Comercio.Data.Domain.Services;

namespace comercio1.Controllers
{
    public class ProductoController : Controller
    {
        private readonly ProductoServicio productoServicio = new ProductoServicio();
        // GET: Producto
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GuardarProducto(int idProducto, string nombre, string descripcion, decimal precio, int stock, int idCategoria)
        {
            try
            {
                idProducto = productoServicio.GuardarProducto(idProducto, nombre, descripcion,precio,stock,idCategoria);
                return new JsonResult { Data = new { Success = true, Data = idProducto } };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = new { Success = false, Data = "", Mensaje = ex.Message } };
            }
        }
        public JsonResult EliminarProducto(int id)
        {
            try
            {
                productoServicio.EliminarProducto(id);
                return new JsonResult { Data = new { Success = true } };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = new { Success = false, Data = "", Mensaje = ex.Message } };
            }
        }
        public JsonResult ObtenerProducto(int id)
        {
            try
            {
                var data = productoServicio.ObtenerProducto(id);
                return new JsonResult { Data = new { Success = true, Data = data } };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = new { Success = false, Data = "", Mensaje = ex.Message } };
            }
        }
        public JsonResult ObtenerProductos()
        {
            try
            {
                var data = productoServicio.ObtenerProductosAbm();
                return new JsonResult { Data = new { Success = true, Data = data } };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = new { Success = false, Data = "", Mensaje = ex.Message } };
            }
        }

    }
}