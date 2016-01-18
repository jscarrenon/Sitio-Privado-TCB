module app.domain {
    export interface IFondoMutuo {
        descripcion: string;
        tipo: string;
        ctaPisys: string;
        valorCuota: number;
        saldoCuota: number;
        renta: string;
        pesos: number;
    }

    export class FondoMutuo extends app.domain.EntityBase implements IFondoMutuo {
        constructor(public descripcion: string,
            public tipo: string,
            public ctaPisys: string,
            public valorCuota: number,
            public saldoCuota: number,
            public renta: string,
            public pesos: number) {

            super();

            this.descripcion = descripcion;
            this.tipo = tipo;
            this.ctaPisys = ctaPisys;
            this.valorCuota = valorCuota;
            this.saldoCuota = saldoCuota;
            this.renta = renta;
            this.pesos = pesos;
        }
    }
}