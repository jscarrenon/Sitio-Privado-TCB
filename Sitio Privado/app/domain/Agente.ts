module app.domain {
    export interface IAgente {
        codigo: number;
        nombre: string;
        sucursal: string;
        pathImg: string;
        email: string;
        telefono?: string;
        fechaAcreditacion?: string;
    }

    export class Agente extends app.domain.EntityBase implements IAgente {
        constructor(public codigo: number,
            public nombre: string,
            public sucursal: string,
            public pathImg: string,
            public email: string,
            public telefono: string,
            public fechaAcreditacion: string) {

            super();

            this.codigo = codigo;
            this.nombre = nombre;
            this.sucursal = sucursal;
            this.pathImg = pathImg;
            this.email = email;
            this.telefono = telefono;
            this.fechaAcreditacion = fechaAcreditacion;
        }
    }
}