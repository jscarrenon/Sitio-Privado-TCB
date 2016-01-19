module app.domain {

    export interface IProducto {
        identificador: number;
        descriptor: string;
        categorias: Categoria[];
    }

    export class Producto extends app.domain.EntityBase implements IProducto {
        constructor(public identificador: number,
            public descriptor: string,
            public categorias: Categoria[]) {

            super();

            this.identificador = identificador;
            this.descriptor = descriptor;
            this.categorias = categorias;
        }
    }
}