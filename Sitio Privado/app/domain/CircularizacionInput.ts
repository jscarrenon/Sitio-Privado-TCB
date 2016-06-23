module app.domain {
    export interface ICircularizacionPendienteInput {
    }

    export class CircularizacionPendienteInput extends app.domain.InputBase implements ICircularizacionPendienteInput {
        constructor() {

            super();
        }
    }

    export interface ICircularizacionArchivoInput {
    }

    export class CircularizacionArchivoInput extends app.domain.InputBase implements ICircularizacionArchivoInput {
        constructor() {

            super();
        }
    }

    export interface ICircularizacionLeidaInput {
    }

    export class CircularizacionLeidaInput extends app.domain.InputBase implements ICircularizacionLeidaInput {
        constructor() {

            super();
        }
    }

    export interface ICircularizacionRespondidaInput {
        respuesta: string;
        comentario: string;
    }

    export class CircularizacionRespondidaInput extends app.domain.InputBase implements ICircularizacionRespondidaInput {
        constructor(public respuesta: string,
            public comentario: string) {
            
            super();
        }
    }

    export interface ICircularizacionFechaInput {
    }

    export class CircularizacionFechaInput extends app.domain.InputBase implements ICircularizacionFechaInput {
        constructor() {

            super();
        }
    }
}