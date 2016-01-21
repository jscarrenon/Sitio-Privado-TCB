﻿module app.domain {
    export interface IDocumento {
        Codigo: string;
        Producto: string;
        Tipo: string;
        Folio: string;
        FechaCreacion: string;
        Leido: string;
        Firmada: string;
        Ruta: string;
        Resultados: string;
    }

    export class Documento extends app.domain.EntityBase implements IDocumento {
        constructor(public Codigo: string,
            public Producto: string,
            public Tipo: string,
            public Folio: string,
            public FechaCreacion: string,
            public Leido: string,
            public Firmada: string,
            public Ruta: string,
            public Resultados: string) {

            super();

            this.Codigo = Codigo;
            this.Producto = Producto;
            this.Tipo = Tipo;
            this.Folio = Folio;
            this.FechaCreacion = FechaCreacion;
            this.Leido = Leido;
            this.Firmada = Firmada;
            this.Ruta = Ruta;
            this.Resultados = Resultados;
        }
    }
}