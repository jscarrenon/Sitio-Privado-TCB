﻿using System.Collections.Generic;
using Sitio_Privado.CategoriaInversionista;

namespace Sitio_Privado.Models
{
    public class ProductoInput
    {
        public int ident_prd { get; set; }
    }

    public class Producto
    {
        public int Identificador { get; set; }
        public string Descriptor { get; set; }
        public string Comentario { get; set; }
        public string Riesgo { get; set; }
        public List<Categoria> Categorias { get; set; }

        public Producto() { }

        public Producto(_producto producto)
        {
            Identificador = producto.identificador;
            Descriptor = producto.descriptor;
            Categorias = new List<Categoria>();
            Comentario = producto.comentario;
            Riesgo = producto.riesgo;
            if(producto.Categorias != null)
            {
                foreach (_categoria categoria in producto.Categorias)
                {
                    Categorias.Add(new Categoria(categoria));
                }
            }
        }

        public Producto(ProductoInput input)
        {
            tann_catsvc webService = new tann_catsvc();
            _producto producto = webService.tann_cns_prod(input.ident_prd);
            Identificador = producto.identificador;
            Descriptor = producto.descriptor;
            Comentario = producto.comentario;
            Riesgo = producto.riesgo;
            Categorias = new List<Categoria>();

            foreach (_categoria categoria in producto.Categorias)
            {
                Categorias.Add(new Categoria(categoria));
            }
        }
    }
}