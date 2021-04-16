using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Udi.Comercio.Data.Domain.Entities;
using Udi.Comercio.Data.Infrastructure.Data.DataModels;
using Udi.Comercio.Data.Infrastructure.Data.Repositories;

namespace Udi.Comercio.Data.Domain.Services
{
    public class ProductoServicio
    {
        private readonly ProductoRepositorio productoRepositorio;
        public ProductoServicio(){
            this.productoRepositorio = new ProductoRepositorio();
        }
        public int GuardarProducto(int idProducto, string nombre, string descripcion, decimal precio,int stock, int idCategoria)
        {
            if (idProducto == 0)
            {
                idProducto = productoRepositorio.GuardarProducto(nombre, descripcion,precio,stock,idCategoria);
            }
            else
            {
                productoRepositorio.ModificarProducto(idProducto, nombre, descripcion,precio,stock,idCategoria);
            }
            return idProducto;
        }
        public void EliminarProducto(int id)
        {
            productoRepositorio.EliminarProducto(id);
        }
        public Producto ObtenerProducto(int id)
        {
            return productoRepositorio.ObtenerProducto(id);
        }
        public List<Producto> ObtenerProductos()
        {
            return productoRepositorio.ObtenerProductos();
        }
        public ProductoABMDto ObtenerProductosAbm()
        {
            CategoriaRepositorio catRep = new CategoriaRepositorio();
            return new ProductoABMDto()
            {
                Productos = productoRepositorio.ObtenerProductoDto(),
                Categorias = catRep.ObtenetCategoriaDto()
            };
        }
    }
}
