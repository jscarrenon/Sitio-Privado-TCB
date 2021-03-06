﻿module app.domain {
    export interface IProductoInput {
        ident_prd: number;
    }

    export class ProductoInput extends app.domain.InputBase implements IProductoInput {
        constructor(public ident_prd: number) {

            super();
        }
    }
}