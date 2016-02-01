﻿module app.domain {
    export interface IIndices {
        RutTCB: string;
        DescriptorTCB: string;
        ActivosSieteDias: number;
        PasivosSieteDias: number;
        ActivosIntermediacion: number;
        AcreedoresIntermediacion: number;
        TotalPasivosExigibles: number;
        PatrimonioLiquido: number;
        MontoCoberturaPatrimonial: number;
        PatrimonioDepurado: number;
        FechaConsulta: string;
        LiquidezGeneral: number;
        LiquidezIntermediacion: number;
        RazonEndeudamiento: number;
        RazonCoberturaPatrimonial: number;
    }

    export class Indices extends app.domain.EntityBase implements IIndices {
        constructor(public RutTCB: string,
            public DescriptorTCB: string,
            public ActivosSieteDias: number,
            public PasivosSieteDias: number,
            public ActivosIntermediacion: number,
            public AcreedoresIntermediacion: number,
            public TotalPasivosExigibles: number,
            public PatrimonioLiquido: number,
            public MontoCoberturaPatrimonial: number,
            public PatrimonioDepurado: number,
            public FechaConsulta: string,
            public LiquidezGeneral: number,
            public LiquidezIntermediacion: number,
            public RazonEndeudamiento: number,
            public RazonCoberturaPatrimonial: number) {

            super();

            this.RutTCB = RutTCB;
            this.DescriptorTCB = DescriptorTCB;
            this.ActivosSieteDias = ActivosSieteDias;
            this.PasivosSieteDias = PasivosSieteDias;
            this.ActivosIntermediacion = ActivosIntermediacion;
            this.AcreedoresIntermediacion = AcreedoresIntermediacion;
            this.TotalPasivosExigibles = TotalPasivosExigibles;
            this.PatrimonioLiquido = PatrimonioLiquido;
            this.MontoCoberturaPatrimonial = MontoCoberturaPatrimonial;
            this.PatrimonioDepurado = PatrimonioDepurado;
            this.FechaConsulta = FechaConsulta;
            this.LiquidezGeneral = LiquidezGeneral;
            this.LiquidezIntermediacion = LiquidezIntermediacion;
            this.RazonEndeudamiento = RazonEndeudamiento;
            this.RazonCoberturaPatrimonial = RazonCoberturaPatrimonial;
        }
    }
}