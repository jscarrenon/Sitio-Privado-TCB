module App.Domain {
    export interface ICategoriaClienteInput {
        rut_cli: number;
    }

    export class CategoriaClienteInput extends App.Domain.InputBase implements ICategoriaClienteInput {
        constructor(public rut_cli: number) {

            super();

            this.rut_cli = rut_cli;
        }
    }
}