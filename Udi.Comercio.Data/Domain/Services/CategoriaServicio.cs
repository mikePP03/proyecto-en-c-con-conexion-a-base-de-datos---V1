using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Udi.Comercio.Data.Infrastructure.Data.DataModels;
using Udi.Comercio.Data.Infrastructure.Data.Repositories;

namespace Udi.Comercio.Data.Domain.Services
{
    public class CategoriaServicio
    {
        private  readonly CategoriaRepositorio categoriaRepositorio;

        public CategoriaServicio()
        {
            this.categoriaRepositorio = new CategoriaRepositorio();
        }

        public int GuardarCategoria(int id,string nombre,string descripcion)
        {
            if (id == 0)
            {
                id = categoriaRepositorio.GuardarCategoria(nombre, descripcion);
            }
            else
            {
                categoriaRepositorio.ModificarCategoria(id, nombre, descripcion);
            }
            return id;
        }
        public void EliminarCategoria(int id)
        {
            categoriaRepositorio.EliminarCategoria(id);
        }
        public Categoria ObtenerCategoria(int id)
        {
            return categoriaRepositorio.ObtenerCategoria(id);
        }
        public List<Categoria> ObtenerCategorias()
        {
            return categoriaRepositorio.ObtenerCategorias();
        }

    
    }
}
