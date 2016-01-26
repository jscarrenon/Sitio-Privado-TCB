module App.Domain {

    export interface IProducto {
        Identificador: number;
        Descriptor: string;
        Categorias: Categoria[];
    }

    export class Producto extends App.Domain.EntityBase implements IProducto {
        constructor(public Identificador: number,
            public Descriptor: string,
            public Categorias: Categoria[]) {

            super();

            this.Identificador = Identificador;
            this.Descriptor = Descriptor;
            this.Categorias = Categorias;
        }
    }
}