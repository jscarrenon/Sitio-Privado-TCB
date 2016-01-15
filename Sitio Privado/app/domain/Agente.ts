module app.domain {
    export interface IAgente {
        Codigo: number;
        Nombre: string;
        Sucursal: string;
        PathImg: string;
        Email: string;
        Telefono: string;
        FechaAcreditacion: string;
    }

    export class Agente extends app.domain.EntityBase implements IAgente {
        constructor(public Codigo: number,
            public Nombre: string,
            public Sucursal: string,
            public PathImg: string,
            public Email: string,
            public Telefono: string,
            public FechaAcreditacion: string) {

            super();

            this.Codigo = Codigo;
            this.Nombre = Nombre;
            this.Sucursal = Sucursal;
            this.PathImg = PathImg;
            this.Email = Email;
            this.Telefono = Telefono;
            this.FechaAcreditacion = FechaAcreditacion;
        }
    }
}