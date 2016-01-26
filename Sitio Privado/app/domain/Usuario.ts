module App.Domain {
    export interface IUsuario {
        Autenticado: boolean;
        Nombres: string;
        Apellidos: string;
        Rut: string;
        DireccionComercial: string;
        DireccionParticular: string;
        Ciudad: string;
        Pais: string;
        TelefonoComercial: string;
        TelefonoParticular: string;
        Email: string;
        CuentaCorriente: string;
        Banco: string;
        NombreCompleto: string;
        CiudadPais: string;
    }

    export class Usuario extends App.Domain.EntityBase implements IUsuario {
        constructor(public Autenticado: boolean,
            public Nombres: string,
            public Apellidos: string,
            public Rut: string,
            public DireccionComercial: string,
            public DireccionParticular: string,
            public Ciudad: string,
            public Pais: string,
            public TelefonoComercial: string,
            public TelefonoParticular: string,
            public Email: string,
            public CuentaCorriente: string,
            public Banco: string,
            public NombreCompleto: string,
            public CiudadPais: string) {

            super();

            this.Autenticado = Autenticado;
            this.Nombres = Nombres;
            this.Apellidos = Apellidos;
            this.Rut = Rut;
            this.DireccionComercial = DireccionComercial;
            this.DireccionParticular = DireccionParticular;
            this.Ciudad = Ciudad;
            this.Pais = Pais;
            this.TelefonoComercial = TelefonoComercial;
            this.TelefonoParticular = TelefonoParticular;
            this.Email = Email;
            this.CuentaCorriente = CuentaCorriente;
            this.Banco = Banco;
            this.NombreCompleto = NombreCompleto;
            this.CiudadPais = CiudadPais;
        }
    }
}