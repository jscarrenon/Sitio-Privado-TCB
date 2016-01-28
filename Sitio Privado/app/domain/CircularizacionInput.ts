﻿module app.domain {
    export interface ICircularizacionPendienteInput {
        rut: number;
        fecha: string;
    }

    export class CircularizacionPendienteInput extends app.domain.InputBase implements ICircularizacionPendienteInput {
        constructor(public rut: number,
            public fecha: string) {

            super();

            this.rut = rut;
            this.fecha = fecha;
        }
    }

    export interface ICircularizacionArchivoInput {
        rut: number;
        fecha: string;
    }

    export class CircularizacionArchivoInput extends app.domain.InputBase implements ICircularizacionArchivoInput {
        constructor(public rut: number,
            public fecha: string) {

            super();

            this.rut = rut;
            this.fecha = fecha;
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

            this.rut = rut;
            this.fecha = fecha;
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

            this.rut_cli = rut_cli;
            this.fecha = fecha;
            this.respuesta = respuesta;
            this.comentario = comentario;
        }
    }
}