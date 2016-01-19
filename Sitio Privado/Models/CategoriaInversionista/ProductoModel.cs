using System.Collections.Generic;
using Sitio_Privado.CategoriaInversionista;

namespace Sitio_Privado.Models.CategoriaInversionista
{
    public class ProductoInput
    {
        public int ident_prd { get; set; }
    }

    public class Producto
    {
        public int identificador { get; set; }
        public string descriptor { get; set; }
        public List<Categoria> categorias { get; set; }

        public Producto() { }

        public Producto(_producto producto)
        {
            identificador = producto.identificador;
            descriptor = producto.descriptor;
        }

        public Producto(ProductoInput input)
        {
            tann_catsvc webService = new tann_catsvc();
            _producto producto = webService.tann_cns_prod(input.ident_prd);
            identificador = producto.identificador;
            descriptor = producto.descriptor;
            categorias = new List<Categoria>();

            foreach (_categoria categoria in producto.Categorias)
            {
                categorias.Add(new Categoria(categoria));
            }
        }
    }
}