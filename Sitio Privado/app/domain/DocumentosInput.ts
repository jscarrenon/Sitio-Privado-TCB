module app.domain {
    export interface IDocumentosPendientesInput {
    }

    export class DocumentosPendientesInput extends app.domain.InputBase implements IDocumentosPendientesInput {
        constructor() {

            super();
        }
    }

    export interface IDocumentosFirmadosInput {
        fechaIni: string;
        fechaFin: string;
    }

    export class DocumentosFirmadosInput extends app.domain.InputBase implements IDocumentosFirmadosInput {
        constructor(public fechaIni: string,
            public fechaFin: string) {

            super();
        }
    }

    export interface IDocumentoLeidoInput {
        codigo: string;
    }

    export class DocumentoLeidoInput extends app.domain.InputBase implements IDocumentoLeidoInput {
        constructor(public codigo: string) {

            super();
        }
    }

    export interface IDocumentoFirmarInput {
        codigo: string;
    }

    export class DocumentoFirmarInput extends app.domain.InputBase implements IDocumentoFirmarInput {
        constructor(public codigo: string) {

            super();
        }
    }

    export interface IOperacionFirmarInput {
        codigo: string;
    }

    export class OperacionFirmarInput extends app.domain.InputBase implements IOperacionFirmarInput {
        constructor(public codigo: string) {

            super();
        }
    }

    export interface IDocumentosPendientesCantidadInput {
    }

    export class DocumentosPendientesCantidadInput extends app.domain.InputBase implements IDocumentosPendientesCantidadInput {
        constructor() {

            super();
        }
    }
}