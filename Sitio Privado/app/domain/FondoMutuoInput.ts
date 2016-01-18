module app.domain {
    export interface IFondoMutuoInput {
        rut_cli: string;
    }

    export class FondoMutuoInput extends app.domain.InputBase implements IFondoMutuoInput {
        constructor(public rut_cli: string) {

            super();

            this.rut_cli = rut_cli;
        }
    }
}