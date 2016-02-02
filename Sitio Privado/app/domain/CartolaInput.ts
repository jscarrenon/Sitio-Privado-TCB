module app.domain {
    export interface ICartolaInput {
        _rut: string;
    }

    export class CartolaInput extends app.domain.InputBase implements ICartolaInput {
        constructor(public _rut: string) {

            super();

            this._rut = _rut;
        }
    }
}