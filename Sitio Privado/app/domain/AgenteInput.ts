module app.domain {
    export interface IAgenteInput {
        _rut: string;
        _sec: number;
    }

    export class AgenteInput extends app.domain.InputBase implements IAgenteInput {
        constructor(public _rut: string,
            public _sec: number) {

            super();

            this._rut = _rut;
            this._sec = _sec;
        }
    }
}