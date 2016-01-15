module app.domain {
    export interface ICategoriaInput {
        ident_cat: string;
    }

    export class CategoriaInput extends app.domain.InputBase implements ICategoriaInput {
        constructor(public ident_cat: string) {

            super();

            this.ident_cat = ident_cat;
        }
    }
}