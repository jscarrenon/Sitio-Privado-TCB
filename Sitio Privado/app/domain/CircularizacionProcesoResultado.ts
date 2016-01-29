module app.domain {
    export interface ICircularizacionProcesoResultado {
        Resultado: boolean;
    }

    export class CircularizacionProcesoResultado extends app.domain.EntityBase implements ICircularizacionProcesoResultado {
        constructor(public Resultado: boolean) {

            super();

            this.Resultado = Resultado;
        }
    }
}