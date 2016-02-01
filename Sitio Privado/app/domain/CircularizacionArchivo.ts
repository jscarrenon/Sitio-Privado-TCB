module app.domain {
    export interface ICircularizacionArchivo {
        UrlCartola: string;
        UrlCircularizacion: string;
    }

    export class CircularizacionArchivo extends app.domain.EntityBase implements ICircularizacionArchivo {
        constructor(public UrlCartola: string,
            public UrlCircularizacion: string) {
            
            super();

            this.UrlCartola = UrlCartola;
            this.UrlCircularizacion = UrlCircularizacion;
        }
    }
}