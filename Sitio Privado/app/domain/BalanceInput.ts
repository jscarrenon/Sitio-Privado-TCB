module App.Domain {
    export interface IBalanceInput {
        rut: string;
    }

    export class BalanceInput extends App.Domain.InputBase implements IBalanceInput {
        constructor(public rut: string) {

            super();

            this.rut = rut;
        }
    }
}