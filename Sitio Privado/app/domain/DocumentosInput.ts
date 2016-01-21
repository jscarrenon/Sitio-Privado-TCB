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
        mercado: string;
        codigo: string;
        folio: string;
    }

    export class DocumentoLeidoInput extends app.domain.InputBase implements IDocumentoLeidoInput {
        constructor(public rut: string,
            public mercado: string,
            public codigo: string,
            public folio: string) {

            super();

            this.rut = rut;
            this.mercado = mercado;
            this.codigo = codigo;
            this.folio = folio;
        }
    }
}