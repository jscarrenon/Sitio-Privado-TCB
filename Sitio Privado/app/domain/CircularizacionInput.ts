module app.domain {
    export interface ICircularizacionPendienteInput {
        rut: number;
        fecha: string;
    }

    export class CircularizacionPendienteInput extends app.domain.InputBase implements ICircularizacionPendienteInput {
        constructor(public rut: number,
            public fecha: string) {

            super();
        }
    }

    export interface ICircularizacionArchivoInput {
        rut: string;
        fecha: string;
    }

    export class CircularizacionArchivoInput extends app.domain.InputBase implements ICircularizacionArchivoInput {
        constructor(public rut: string,
            public fecha: string) {

            super();
        }
    }

    export interface ICircularizacionLeidaInput {
        rut: number;
        fecha: string;
    }

    export class CircularizacionLeidaInput extends app.domain.InputBase implements ICircularizacionLeidaInput {
        constructor(public rut: number,
            public fecha: string) {

            super();
        }
    }

    export interface ICircularizacionRespondidaInput {
        rut_cli: number;
        fecha: string;
        respuesta: string;
        comentario: string;
    }

    export class CircularizacionRespondidaInput extends app.domain.InputBase implements ICircularizacionRespondidaInput {
        constructor(public rut_cli: number,
            public fecha: string,
            public respuesta: string,
            public comentario: string) {
            
            super();
        }
    }
}