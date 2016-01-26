module App.Domain {
    export interface IProductoInput {
        ident_prd: number;
    }

    export class ProductoInput extends App.Domain.InputBase implements IProductoInput {
        constructor(public ident_prd: number) {

            super();

            this.ident_prd = ident_prd;
        }
    }
}