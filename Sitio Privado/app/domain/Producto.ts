module app.domain {

    export interface IProducto {
        Identificador: number;
        Descriptor: string;
        Comentario: string;
        Riesgo: string;
        Categorias: Categoria[];
    }

    export class Producto extends app.domain.EntityBase implements IProducto {
        constructor(public Identificador: number,
            public Descriptor: string,
            public Comentario: string,
            public Riesgo: string,
            public Categorias: Categoria[]) {

            super();

            this.Identificador = Identificador;
            this.Descriptor = Descriptor;
            this.Comentario = Comentario;
            this.Riesgo = Riesgo;
            this.Categorias = Categorias;
        }
    }
}