module app.domain {
    export interface ICartolaInput {
    }

    export class CartolaInput extends app.domain.InputBase implements ICartolaInput {
        constructor() {
            super();
        }
    }
}