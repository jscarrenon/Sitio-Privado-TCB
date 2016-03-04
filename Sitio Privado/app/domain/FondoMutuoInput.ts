module app.domain {
    export interface IFondoMutuoInput {
    }

    export class FondoMutuoInput extends app.domain.InputBase implements IFondoMutuoInput {
        constructor() {
            super();
        }
    }
}