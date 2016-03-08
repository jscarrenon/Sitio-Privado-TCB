module app.domain {
    export interface IBalanceInput {
    }

    export class BalanceInput extends app.domain.InputBase implements IBalanceInput {
        constructor() {
            super();
        }
    }
}