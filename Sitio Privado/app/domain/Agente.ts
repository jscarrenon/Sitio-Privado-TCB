module App.Domain {
    export interface IAgente {
        Codigo: number;
        Nombre: string;
        Sucursal: string;
        PathImg: string;
        Email: string;
        Telefono: string;
        FechaInicioAcreditacion: string;
        FechaExpiracionAcreditacion: string;
        Descriptor: string;
    }

    export class Agente extends App.Domain.EntityBase implements IAgente {
        constructor(public Codigo: number,
            public Nombre: string,
            public Sucursal: string,
            public PathImg: string,
            public Email: string,
            public Telefono: string,
            public FechaInicioAcreditacion: string,
            public FechaExpiracionAcreditacion: string,
            public Descriptor: string) {

            super();

            this.Codigo = Codigo;
            this.Nombre = Nombre;
            this.Sucursal = Sucursal;
            this.PathImg = PathImg;
            this.Email = Email;
            this.Telefono = Telefono;
            this.FechaInicioAcreditacion = FechaInicioAcreditacion;
            this.FechaExpiracionAcreditacion = FechaExpiracionAcreditacion;
            this.Descriptor = Descriptor;
        }
    }
}