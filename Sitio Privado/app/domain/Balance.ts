module app.domain {
    export interface IBalance {
        Enlace: string;
    }

    export class Balance extends app.domain.EntityBase implements IBalance {
        constructor(public Enlace: string) {

            super();
        }
    }
}