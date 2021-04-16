using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Udi.Comercio.Data.Domain.Entities;
using Udi.Comercio.Data.Infrastructure.Data.DataModels;
using Udi.Comercio.Data.Infrastructure.Data.Repositories;

namespace Udi.Comercio.Data.Infrastructure.Data.Repositories
{
    class CategoriaRepositorio: EFRepositorio<Categoria>
    {
        public int GuardarCategoria(string nombre, string descripcion)
        {
            Categoria categoria = new Categoria()
            {
                Nombre = nombre,
                Descripcion = descripcion
            };
            Add(categoria);
            SaveChanges();
            return categoria.IdCategoria;
        }
        public void ModificarCategoria(int id,string nombre,string descripcion)
        {
            Categoria cat = Get(id);
            cat.Nombre = nombre;
            cat.Descripcion = descripcion;
            Update(cat);
            SaveChanges();
        }
        public void EliminarCategoria(int id)
        {
            Categoria cat = Get(id);
            Remove(cat);
            SaveChanges();
        }
        public Categoria ObtenerCategoria(int id)
        {
            return Get(id);
        }
        public List<Categoria> ObtenerCategorias()
        {
            return GetAll().OrderBy(x=>x.Nombre).ToList();
        }
        public List<CategoriaDTO> ObtenetCategoriaDto()
        {
            return BuildQuery().Select(cat => new CategoriaDTO()
            {
                IdCategoria = cat.IdCategoria,
                Nombre = cat.Nombre,
                Descripcion = cat.Descripcion
            }).ToList();
        }
    }
}
