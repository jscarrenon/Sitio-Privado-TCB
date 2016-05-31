module app.domain {
    export interface ISuscripcionFirmaElectronica {
        Respuesta: string;
        Glosa: string;
    }

    export class SuscripcionFirmaElectronica extends app.domain.EntityBase implements ISuscripcionFirmaElectronica {
        constructor(public Respuesta: string,public Glosa: string) {

            super();
        }
    }
}