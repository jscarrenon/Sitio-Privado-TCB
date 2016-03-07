module app.domain {
    export interface ICategoriaClienteInput {
    }

    export class CategoriaClienteInput extends app.domain.InputBase implements ICategoriaClienteInput {
        constructor() {
            super();
        }
    }
}