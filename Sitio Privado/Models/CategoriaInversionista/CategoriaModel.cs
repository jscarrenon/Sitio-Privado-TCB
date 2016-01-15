using System.Collections.Generic;
using Sitio_Privado.CategoriaInversionista;

namespace Sitio_Privado.Models.CategoriaInversionista
{
    public class CategoriaInput
    {
        public int ident_cat { get; set; }
    }

    public class Producto
    {
        public int identificador { get; set; }
        public string descriptor { get; set; }

        public Producto(_producto producto)
        {
            identificador = producto.identificador;
            descriptor = producto.descriptor;
        }
    }

    public class Categoria
    {
        public int identificador { get; set; }

        public string descriptor { get; set; }

        public string comentario { get; set; }

        public List<Producto> productos { get; set; }

        public Categoria() { }

        public Categoria(CategoriaInput input)
        {
            tann_catsvc webService = new tann_catsvc();
            _categoria categoria = webService.tann_cns_cat(input.ident_cat);
            identificador = categoria.identificador;
            descriptor = categoria.descriptor;
            comentario = categoria.comentario;
            
            foreach(_producto producto in categoria.Productos)
            {
                productos = new List<Producto>();
                productos.Add(new Producto(producto));
            }
            
        }
    }
}