module app.domain {
    export interface IFondoMutuo {
        Descripcion: string;
        Tipo: string;
        CtaPisys: string;
        ValorCuota: number;
        SaldoCuota: number;
        Csbis: string;
        Renta: string;
        Pesos: number;
    }
    
    export class FondoMutuo extends app.domain.EntityBase implements IFondoMutuo {
        constructor(public Descripcion: string,
            public Tipo: string,
            public CtaPisys: string,
            public ValorCuota: number,
            public SaldoCuota: number,
            public Csbis: string,
            public Renta: string,
            public Pesos: number) {

            super();

            this.Descripcion = Descripcion;
            this.Tipo = Tipo;
            this.CtaPisys = CtaPisys;
            this.ValorCuota = ValorCuota;
            this.SaldoCuota = SaldoCuota;
            this.Csbis = Csbis;
            this.Renta = Renta;
            this.Pesos = Pesos;
        }
    }


}