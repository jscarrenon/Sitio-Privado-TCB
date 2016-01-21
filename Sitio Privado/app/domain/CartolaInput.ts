module app.domain {
    export interface ICartolaInput {
        _rut: string;
        _secuencia: number;
    }

    export class CartolaInput extends app.domain.InputBase implements ICartolaInput {
        constructor(public _rut: string,
            public _secuencia: number) {

            super();

            this._rut = _rut;
            this._secuencia = _secuencia;
        }
    }
}