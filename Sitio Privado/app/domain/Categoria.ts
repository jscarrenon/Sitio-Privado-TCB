module app.domain {

    export interface ICategoria {
        Identificador: number;
        Descriptor: string;
        Comentario: string;
        Productos: Producto[];
    }

    export class Categoria extends app.domain.EntityBase implements ICategoria {
        constructor(public Identificador: number,
            public Descriptor: string,
            public Comentario: string,
            public Productos: Producto[]) {

            super();
        }
    }
}