module app.domain {
    export interface ICartola {
        Rut: string;
        Periodo: string;
        SaldoCaja: number;
        TotalRentaFija: number;
        InstrumentosRentaFija: number;
        FondosMutuosRentaFija: number;
        TotalRentaVariable: number;
        AccionesNacionales: number;
        FondosMutuosRentaVariable: number;
        ForwardCompra: number;
        ForwardVenta: number;
        TotalInversiones: number;
    }

    export class Cartola extends app.domain.EntityBase implements ICartola {
        constructor(public Rut: string,
            public Periodo: string,
            public SaldoCaja: number,
            public TotalRentaFija: number,
            public InstrumentosRentaFija: number,
            public FondosMutuosRentaFija: number,
            public TotalRentaVariable: number,
            public AccionesNacionales: number,
            public FondosMutuosRentaVariable: number,
            public ForwardCompra: number,
            public ForwardVenta: number,
            public TotalInversiones: number) {

            super();

            this.Rut = Rut;
            this.Periodo = Periodo;
            this.SaldoCaja = SaldoCaja;
            this.TotalRentaFija = TotalRentaFija;
            this.InstrumentosRentaFija = InstrumentosRentaFija;
            this.FondosMutuosRentaFija = FondosMutuosRentaFija;
            this.TotalRentaVariable = TotalRentaVariable;
            this.AccionesNacionales = AccionesNacionales;
            this.FondosMutuosRentaVariable = FondosMutuosRentaVariable;
            this.ForwardCompra = ForwardCompra;
            this.ForwardVenta = ForwardVenta;
            this.TotalInversiones = TotalInversiones;
        }
    }
}