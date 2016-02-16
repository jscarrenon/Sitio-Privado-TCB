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

    export interface ICartola {
        Rut: string;
        Periodo: string;
        Conceptos: ICartolaConcepto[];

    }

    export class Cartola extends app.domain.EntityBase implements ICartola {
        constructor(public Rut: string,
            public Periodo: string,
            public Conceptos: ICartolaConcepto[]) {

            super();
        }
    }
}