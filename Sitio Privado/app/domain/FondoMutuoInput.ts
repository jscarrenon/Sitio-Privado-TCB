module App.Domain {
    export interface IFondoMutuoInput {
        rut_cli: number;
    }

    export class FondoMutuoInput extends App.Domain.InputBase implements IFondoMutuoInput {
        constructor(public rut_cli: number) {

            super();

            this.rut_cli = rut_cli;
        }
    }
}