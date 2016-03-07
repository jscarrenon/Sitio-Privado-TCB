module app.domain {
    export interface ICartolaConcepto {
        Concepto: string;
        Valor: number;
        Porcentaje: number;
    }

    export class CartolaConcepto extends app.domain.EntityBase implements ICartolaConcepto {
        constructor(public Concepto: string,
            public Valor: number,
            public Porcentaje: number) {

            super();
        }
    }

    export interface ICartolaTitulo {
        Codigo: number;
        Descriptor: string;
        Rut: string;
        Periodo: string;
        Conceptos: ICartolaConcepto[];
        DatosCargados?: boolean;
    }

    export class CartolaTitulo extends app.domain.EntityBase implements ICartolaTitulo {
        constructor(public Codigo: number,
            public Descriptor: string,
            public Rut: string,
            public Periodo: string,
            public Conceptos: ICartolaConcepto[]) {

            super();
        }
    }

    export interface ICartolaConceptosTituloResultado {
        Rut: string;
        Periodo: string;
        Conceptos: ICartolaConcepto[];
    }

    export class CartolaConceptosTituloResultado extends app.domain.EntityBase implements ICartolaConceptosTituloResultado {
        constructor(public Rut: string,
            public Periodo: string,
            public Conceptos: ICartolaConcepto[]) {

            super();
        }
    }

    export interface ICartola {
        Titulos: ICartolaTitulo[];
    }

    export class Cartola extends app.domain.EntityBase implements ICartola {
        constructor(public Titulos: ICartolaTitulo[]) {

            super();
        }
    }
}