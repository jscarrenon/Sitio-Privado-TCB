module app.domain {

    export interface ICategoria {
        identificador: number;
        descriptor: string;
        comentario: string;
        productos: Producto[];
    }

    export class Categoria extends app.domain.EntityBase implements ICategoria {
        constructor(public identificador: number,
            public descriptor: string,
            public comentario: string,
            public productos: Producto[]) {

            super();

            this.identificador = identificador;
            this.descriptor = descriptor;
            this.comentario = comentario;
            this.productos = productos;
        }
    }
}