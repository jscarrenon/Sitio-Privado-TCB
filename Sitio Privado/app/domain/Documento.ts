module App.Domain {
    export interface IDocumento {
        Codigo: string;
        Producto: string;
        Tipo: string;
        Folio: string;
        FechaCreacion: string;
        Leido: string;
        Firmado: string;
        Ruta: string;
        Resultados: string;
    }

    export class Documento extends App.Domain.EntityBase implements IDocumento {
        constructor(public Codigo: string,
            public Producto: string,
            public Tipo: string,
            public Folio: string,
            public FechaCreacion: string,
            public Leido: string,
            public Firmado: string,
            public Ruta: string,
            public Resultados: string) {

            super();

            this.Codigo = Codigo;
            this.Producto = Producto;
            this.Tipo = Tipo;
            this.Folio = Folio;
            this.FechaCreacion = FechaCreacion;
            this.Leido = Leido;
            this.Firmado = Firmado;
            this.Ruta = Ruta;
            this.Resultados = Resultados;
        }
    }
}