module app.domain {
    export interface IDocumentosPendientesInput {
        rut: string;
    }

    export class DocumentosPendientesInput extends app.domain.InputBase implements IDocumentosPendientesInput {
        constructor(public rut: string) {

            super();

            this.rut = rut;
        }
    }

    export interface IDocumentosFirmadosInput {
        rut: string;
        fechaIni: string;
        fechaFin: string;
    }

    export class DocumentosFirmadosInput extends app.domain.InputBase implements IDocumentosFirmadosInput {
        constructor(public rut: string,
            public fechaIni: string,
            public fechaFin: string) {

            super();

            this.rut = rut;
            this.fechaIni = fechaIni;
            this.fechaFin = fechaFin;
        }
    }

    export interface IDocumentoLeidoInput {
        rut: string;
        codigo: string;
    }

    export class DocumentoLeidoInput extends app.domain.InputBase implements IDocumentoLeidoInput {
        constructor(public rut: string,
            public codigo: string) {

            super();

            this.rut = rut;
            this.codigo = codigo;
        }
    }

    export interface IDocumentoFirmarInput {
        rut: string;
        codigo: string;
    }

    export class DocumentoFirmarInput extends app.domain.InputBase implements IDocumentoFirmarInput {
        constructor(public rut: string,
            public codigo: string) {

            super();

            this.rut = rut;
            this.codigo = codigo;
        }
    }

    export interface IOperacionFirmarInput {
        rut: string;
        codigo: string;
    }

    export class OperacionFirmarInput extends app.domain.InputBase implements IOperacionFirmarInput {
        constructor(public rut: string,
            public codigo: string) {

            super();

            this.rut = rut;
            this.codigo = codigo;
        }
    }
}