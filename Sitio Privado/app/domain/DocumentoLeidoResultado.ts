module app.domain {
    export interface IDocumentoLeidoResultado {
        Resultado: boolean;
    }

    export class DocumentoLeidoResultado extends app.domain.EntityBase implements IDocumentoLeidoResultado {
        constructor(public Resultado: boolean) {

            super();
        }
    }
}