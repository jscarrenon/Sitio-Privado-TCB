using System.Collections.Generic;
using Sitio_Privado.CategoriaInversionista;

namespace Sitio_Privado.Models.CategoriaInversionista
{
    public class CategoriaInput
    {
        public int ident_cat { get; set; }
    }

    public class CategoriaClienteInput
    {
        public int rut_cli { get; set; }
    }

    public class Categoria
    {
        public int Identificador { get; set; }

        public string Descriptor { get; set; }

        public string Comentario { get; set; }

        public List<Producto> Productos { get; set; }

        public Categoria() { }

        public Categoria(_categoria categoria)
        {
            Identificador = categoria.identificador;
            Descriptor = categoria.descriptor;
            Comentario = categoria.comentario;
        }

        public Categoria(CategoriaInput input)
        {
            tann_catsvc webService = new tann_catsvc();
            _categoria categoria = webService.tann_cns_cat(input.ident_cat);
            Identificador = categoria.identificador;
            Descriptor = categoria.descriptor;
            Comentario = categoria.comentario;
            Productos = new List<Producto>();

            foreach (_producto producto in categoria.Productos)
            {                
                Productos.Add(new Producto(producto));
            }            
        }

        public Categoria(CategoriaClienteInput input)
        {
            tann_catsvc webService = new tann_catsvc();
            _categoria categoria = webService.tann_cns_cat_cli(input.rut_cli);
            Identificador = categoria.identificador;
            Descriptor = categoria.descriptor;
            Comentario = categoria.comentario;
            Productos = new List<Producto>();

            foreach (_producto producto in categoria.Productos)
            {
                Productos.Add(new Producto(producto));
            }
        }
    }
}