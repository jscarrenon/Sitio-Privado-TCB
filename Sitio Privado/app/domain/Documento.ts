﻿module app.domain {
    export interface IDocumento {
        Codigo: string;
        Producto: string;
        Tipo: string;
        Folio: string;
        FechaCreacion: string;
        Leido: string;
        Firmado: string;
        TipoFirma: string;
        FechaFirma: string;
        Ruta: string;
        NombreCliente: string;
        RutaFirmado: string;
        Seleccionado: boolean;
    }

    export class Documento extends app.domain.EntityBase implements IDocumento {
        constructor(public Codigo: string,
            public Producto: string,
            public Tipo: string,
            public Folio: string,
            public FechaCreacion: string,
            public Leido: string,
            public Firmado: string,
            public TipoFirma: string,
            public FechaFirma: string,
            public Ruta: string,
            public NombreCliente: string,
            public RutaFirmado: string,
            public Seleccionado: boolean) {

            super();

            this.Codigo = Codigo;
            this.Producto = Producto;
            this.Tipo = Tipo;
            this.Folio = Folio;
            this.FechaCreacion = FechaCreacion;
            this.Leido = Leido;
            this.Firmado = Firmado;
            this.TipoFirma = TipoFirma;
            this.FechaFirma = FechaFirma;
            this.Ruta = Ruta;
            this.NombreCliente = NombreCliente;
            this.RutaFirmado = RutaFirmado;
            this.Seleccionado = Seleccionado;
        }
    }
}