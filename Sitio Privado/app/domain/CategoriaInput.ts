module app.domain {
    export interface ICategoriaInput {
        ident_cat: number;
    }

    export class CategoriaInput extends app.domain.InputBase implements ICategoriaInput {
        constructor(public ident_cat: number) {

            super();

            this.ident_cat = ident_cat;
        }
    }
}