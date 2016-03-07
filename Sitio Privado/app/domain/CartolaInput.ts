module app.domain {
    export interface ICartolaInput {
    }

    export class CartolaInput extends app.domain.InputBase implements ICartolaInput {
        constructor() {
            super();
        }
    }

    export interface ICartolaTituloInput {
        _selector: number;
    }

    export class CartolaTituloInput extends app.domain.InputBase implements ICartolaTituloInput {
        constructor(public _selector: number) {
            super();
        }
    }
}