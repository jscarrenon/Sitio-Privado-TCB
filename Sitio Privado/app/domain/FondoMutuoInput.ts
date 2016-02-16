module app.domain {
    export interface IFondoMutuoInput {
        rut_cli: number;
    }

    export class FondoMutuoInput extends app.domain.InputBase implements IFondoMutuoInput {
        constructor(public rut_cli: number) {

            super();
        }
    }
}