module app.domain {
    export interface ICircularizacionPendienteInput {
        fecha: string;
    }

    export class CircularizacionPendienteInput extends app.domain.InputBase implements ICircularizacionPendienteInput {
        constructor(public fecha: string) {

            super();
        }
    }

    export interface ICircularizacionArchivoInput {
        fecha: string;
    }

    export class CircularizacionArchivoInput extends app.domain.InputBase implements ICircularizacionArchivoInput {
        constructor(public fecha: string) {

            super();
        }
    }

    export interface ICircularizacionLeidaInput {
        fecha: string;
    }

    export class CircularizacionLeidaInput extends app.domain.InputBase implements ICircularizacionLeidaInput {
        constructor(public fecha: string) {

            super();
        }
    }

    export interface ICircularizacionRespondidaInput {
        fecha: string;
        respuesta: string;
        comentario: string;
    }

    export class CircularizacionRespondidaInput extends app.domain.InputBase implements ICircularizacionRespondidaInput {
        constructor(public fecha: string,
            public respuesta: string,
            public comentario: string) {
            
            super();
        }
    }
}