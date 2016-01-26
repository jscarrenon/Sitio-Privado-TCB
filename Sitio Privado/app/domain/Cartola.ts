module App.Domain {
    export interface ICartola {
        Rut: string;
        Periodo: string;
        SaldoCaja: number;
        SaldoCajaPorcentaje: number;
        TotalRentaFija: number;
        TotalRentaFijaPorcentaje: number;
        InstrumentosRentaFija: number;
        InstrumentosRentaFijaPorcentaje: number;
        FondosMutuosRentaFija: number;
        FondosMutuosRentaFijaPorcentaje: number;
        TotalRentaVariable: number;
        TotalRentaVariablePorcentaje: number;
        AccionesNacionales: number;
        AccionesNacionalesPorcentaje: number;
        FondosMutuosRentaVariable: number;
        FondosMutuosRentaVariablePorcentaje: number;
        ForwardCompra: number;
        ForwardCompraPorcentaje;
        ForwardVenta: number;
        ForwardVentaPorcentaje: number;
        TotalInversiones: number;
        TotalInversionesPorcentaje: number;
    }

    export class Cartola extends App.Domain.EntityBase implements ICartola {
        constructor(public Rut: string,
            public Periodo: string,
            public SaldoCaja: number,
            public SaldoCajaPorcentaje: number,
            public TotalRentaFija: number,
            public TotalRentaFijaPorcentaje: number,
            public InstrumentosRentaFija: number,
            public InstrumentosRentaFijaPorcentaje: number,
            public FondosMutuosRentaFija: number,
            public FondosMutuosRentaFijaPorcentaje: number,
            public TotalRentaVariable: number,
            public TotalRentaVariablePorcentaje: number,
            public AccionesNacionales: number,
            public AccionesNacionalesPorcentaje: number,
            public FondosMutuosRentaVariable: number,
            public FondosMutuosRentaVariablePorcentaje: number,
            public ForwardCompra: number,
            public ForwardCompraPorcentaje: number,
            public ForwardVenta: number,
            public ForwardVentaPorcentaje: number,
            public TotalInversiones: number,
            public TotalInversionesPorcentaje: number) {

            super();

            this.Rut = Rut;
            this.Periodo = Periodo;
            this.SaldoCaja = SaldoCaja;
            this.SaldoCajaPorcentaje = SaldoCajaPorcentaje;
            this.TotalRentaFija = TotalRentaFija;
            this.TotalRentaFijaPorcentaje = TotalRentaFijaPorcentaje;
            this.InstrumentosRentaFija = InstrumentosRentaFija;
            this.InstrumentosRentaFijaPorcentaje = InstrumentosRentaFijaPorcentaje;
            this.FondosMutuosRentaFija = FondosMutuosRentaFija;
            this.FondosMutuosRentaFijaPorcentaje = FondosMutuosRentaFijaPorcentaje;
            this.TotalRentaVariable = TotalRentaVariable;
            this.TotalRentaVariablePorcentaje = TotalRentaVariablePorcentaje;
            this.AccionesNacionales = AccionesNacionales;
            this.AccionesNacionalesPorcentaje = AccionesNacionalesPorcentaje;
            this.FondosMutuosRentaVariable = FondosMutuosRentaVariable;
            this.FondosMutuosRentaVariablePorcentaje = FondosMutuosRentaVariablePorcentaje;
            this.ForwardCompra = ForwardCompra;
            this.ForwardCompraPorcentaje = ForwardCompraPorcentaje;
            this.ForwardVenta = ForwardVenta;
            this.ForwardVentaPorcentaje = ForwardVentaPorcentaje;
            this.TotalInversiones = TotalInversiones;
            this.TotalInversionesPorcentaje = TotalInversionesPorcentaje;
        }
    }
}