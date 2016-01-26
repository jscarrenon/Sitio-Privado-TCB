module App.Domain {
    export interface IDocumentosPendientesInput {
        rut: string;
    }

    export class DocumentosPendientesInput extends App.Domain.InputBase implements IDocumentosPendientesInput {
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

    export class DocumentosFirmadosInput extends App.Domain.InputBase implements IDocumentosFirmadosInput {
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

    export class DocumentoLeidoInput extends App.Domain.InputBase implements IDocumentoLeidoInput {
        constructor(public rut: string,
            public codigo: string) {

            super();

            this.rut = rut;
            this.codigo = codigo;
        }
    }
}