module app.domain {
    export interface IUsuario {
        Id?: number;
        Nombres: string;
        Apellidos: string;
        NombreCompleto: string;
    }

    export class Usuario extends app.domain.EntityBase implements IUsuario {
        constructor(public Nombres: string,
            public Apellidos: string,
            public NombreCompleto: string,
            public Id?: number) {

            super();

            this.Id = Id;
            this.Nombres = Nombres;
            this.Apellidos = Apellidos;
            this.NombreCompleto = NombreCompleto;
        }
    }
}