using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Udi.Comercio.Data.Domain.Entities;
using Udi.Comercio.Data.Infrastructure.Data.DataModels;

namespace Udi.Comercio.Data.Infrastructure.Data.Repositories
{
    class ProductoRepositorio:EFRepositorio<Producto>
    {
        public int GuardarProducto(string nombre, string descripcion, decimal precio, int stock, int idCategoria)
        {
            Producto producto = new Producto()
            {
                Nombre = nombre,
                Descripcion = descripcion,
                Precio = precio,
                Stock = stock,
                IdCategoria = idCategoria
            };
            Add(producto);
            SaveChanges();
            return producto.IdProducto;
        }
        public void ModificarProducto(int id, string nombre, string descripcion,decimal precio, int stock,int idCategoria)
        {
            Producto prod = Get(id);
            prod.Nombre = nombre;
            prod.Descripcion = descripcion;
            prod.Precio = precio;
            prod.Stock = stock;
            prod.IdCategoria = idCategoria;
            Update(prod);
            SaveChanges();
        }
        public void EliminarProducto(int id)
        {
            Producto prod = Get(id);
            Remove(prod);
            SaveChanges();
        }
        public Producto ObtenerProducto(int id)
        {
            return Get(id);
        }
        public List<Producto> ObtenerProductos()
        {
            return GetAll().OrderBy(x => x.Nombre).ToList();
        }

        public List<ProductoDto> ObtenerProductoDto()
        {
            return BuildQuery().Select(prod => new ProductoDto()
            {
                iDProducto =prod.IdProducto,
                Nombre = prod.Nombre,
                Descripcion = prod.Descripcion,
                Precio = prod.Precio.Value,
                Stock = prod.Stock.Value,
                IdCategoria = prod.IdCategoria,
                Categoria = prod.Categoria.Nombre


            }).ToList();
        }

    }
}
