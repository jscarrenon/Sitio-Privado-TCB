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
        public int identificador { get; set; }

        public string descriptor { get; set; }

        public string comentario { get; set; }

        public List<Producto> productos { get; set; }

        public Categoria() { }

        public Categoria(_categoria categoria)
        {
            identificador = categoria.identificador;
            descriptor = categoria.descriptor;
            comentario = categoria.comentario;
        }

        public Categoria(CategoriaInput input)
        {
            tann_catsvc webService = new tann_catsvc();
            _categoria categoria = webService.tann_cns_cat(input.ident_cat);
            identificador = categoria.identificador;
            descriptor = categoria.descriptor;
            comentario = categoria.comentario;
            productos = new List<Producto>();

            foreach (_producto producto in categoria.Productos)
            {                
                productos.Add(new Producto(producto));
            }            
        }

        public Categoria(CategoriaClienteInput input)
        {
            tann_catsvc webService = new tann_catsvc();
            _categoria categoria = webService.tann_cns_cat_cli(input.rut_cli);
            identificador = categoria.identificador;
            descriptor = categoria.descriptor;
            comentario = categoria.comentario;
            productos = new List<Producto>();

            foreach (_producto producto in categoria.Productos)
            {
                productos.Add(new Producto(producto));
            }
        }
    }
}