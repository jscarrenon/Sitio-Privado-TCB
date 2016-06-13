module app.domain {
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
        ContrasenaTemporal: boolean;
    }

    export class Usuario extends app.domain.EntityBase implements IUsuario {
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
            public CiudadPais: string,
            public ContrasenaTemporal: boolean) {

            super();
        }
    }
}