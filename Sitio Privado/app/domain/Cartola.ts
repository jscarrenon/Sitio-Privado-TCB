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
        Conceptos: ICartolaConcepto[];
        DatosCargados?: boolean;
    }

    export class CartolaTitulo extends app.domain.EntityBase implements ICartolaTitulo {
        constructor(public Codigo: number,
            public Descriptor: string,
            public Conceptos: ICartolaConcepto[]) {

            super();
        }
    }

    export interface ICartola {
        Rut: string;
        Periodo: string;
        Titulos: ICartolaTitulo[];
    }

    export class Cartola extends app.domain.EntityBase implements ICartola {
        constructor(public Rut: string,
            public Periodo: string,
            public Titulos: ICartolaTitulo[]) {

            super();
        }
    }
}