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
        public int Identificador { get; set; }
        public string Descriptor { get; set; }

        public Producto(_producto producto)
        {
            Identificador = producto.identificador;
            Descriptor = producto.descriptor;
        }
    }

    public class Categoria
    {
        public int Identificador { get; set; }

        public string Descriptor { get; set; }

        public string Comentario { get; set; }

        public List<Producto> Productos { get; set; }

        public Categoria() { }

        public Categoria(CategoriaInput input)
        {
            tann_catsvc webService = new tann_catsvc();
            _categoria categoria = webService.tann_cns_cat(input.ident_cat);
            Identificador = categoria.identificador;
            Descriptor = categoria.descriptor;
            Comentario = categoria.comentario;
            
            foreach(_producto producto in categoria.Productos)
            {
                Productos = new List<Producto>();
                Productos.Add(new Producto(producto));
            }
            
        }
    }
}