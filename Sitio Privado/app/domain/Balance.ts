module App.Domain {
    export interface IBalance {
        Enlace: string;
    }

    export class Balance extends App.Domain.EntityBase implements IBalance {
        constructor(public Enlace: string) {

            super();

            this.Enlace = Enlace;
        }
    }
}