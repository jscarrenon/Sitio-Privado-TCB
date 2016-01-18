module app.domain {
    export interface IAgente {
        codigo: number;
        nombre: string;
        sucursal: string;
        pathImg: string;
        email: string;
        telefono: string;
        fechaInicioAcreditacion: string;
        fechaExpiracionAcreditacion: string;
        descriptor: string;
    }

    export class Agente extends app.domain.EntityBase implements IAgente {
        constructor(public codigo: number,
            public nombre: string,
            public sucursal: string,
            public pathImg: string,
            public email: string,
            public telefono: string,
            public fechaInicioAcreditacion: string,
            public fechaExpiracionAcreditacion: string,
            public descriptor: string) {

            super();

            this.codigo = codigo;
            this.nombre = nombre;
            this.sucursal = sucursal;
            this.pathImg = pathImg;
            this.email = email;
            this.telefono = telefono;
            this.fechaInicioAcreditacion = fechaInicioAcreditacion;
            this.fechaExpiracionAcreditacion = fechaExpiracionAcreditacion;
            this.descriptor = descriptor;
        }
    }
}