module app.domain {
    export interface IDocumentosPendientesCantidadResultado {
        Resultado: number;
    }

    export class DocumentosPendientesCantidadResultado extends app.domain.EntityBase implements IDocumentosPendientesCantidadResultado {
        constructor(public Resultado: number) {

            super();

            this.Resultado = Resultado;
        }
    }
}