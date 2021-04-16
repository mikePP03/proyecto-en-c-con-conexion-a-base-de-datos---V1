using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Udi.Comercio.Data.Infrastructure.Data.DataModels;

namespace Udi.Comercio.Data.Domain.Entities
{
    public class ProductoABMDto
    {
        public List<ProductoDto> Productos { get; set; }
        public List<CategoriaDTO> Categorias { get; set; }

    }
}
