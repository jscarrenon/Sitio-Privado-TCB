module app.domain {
    export interface IDocumentoFirmarResultado {
        Documentos: IDocumento[];
    }

    export class DocumentoFirmarResultado extends app.domain.EntityBase implements IDocumentoFirmarResultado {
        constructor(public Documentos: IDocumento[]) {

            super();

            this.Documentos = Documentos;
        }
    }
}