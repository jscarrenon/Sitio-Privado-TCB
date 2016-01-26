module App.Domain {
    export interface IDocumentoLeidoResultado {
        Resultado: boolean;
    }

    export class DocumentoLeidoResultado extends App.Domain.EntityBase implements IDocumentoLeidoResultado {
        constructor(public Resultado: boolean) {

            super();

            this.Resultado = Resultado;
        }
    }
}