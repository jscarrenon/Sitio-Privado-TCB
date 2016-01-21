module app.domain {
    export interface IBalanceInput {
        rut: string;
    }

    export class BalanceInput extends app.domain.InputBase implements IBalanceInput {
        constructor(public rut: string) {

            super();

            this.rut = rut;
        }
    }
}