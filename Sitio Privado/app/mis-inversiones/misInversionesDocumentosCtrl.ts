﻿module app.misInversiones {

    interface IMisInversionesDocumentosViewModel extends app.common.interfaces.ISeccion {
        operacionesPendientes: app.domain.IDocumento[];
        documentosPendientes: app.domain.IDocumento[];
        getDocumentosPendientes(input: app.domain.IDocumentosPendientesInput): void;
        operacionesFirmadas: app.domain.IDocumento[];
        documentosFirmados: app.domain.IDocumento[];
        getDocumentosFirmados(input: app.domain.IDocumentosFirmadosInput): void;
        fechaFirmadosInicio: Date;
        fechaFirmadosFin: Date;
        verDocumento(documento: app.domain.IDocumento): void;
        fechaHoy: Date;
        actualizarDocumentosFirmados(): void;
    }

    class MisInversionesDocumentosCtrl implements IMisInversionesDocumentosViewModel {

        templates: string[];
        seccionURI: string;
        seccionId: number;

        operacionesPendientes: app.domain.IDocumento[];
        documentosPendientes: app.domain.IDocumento[];
        documentosPendientesInput: app.domain.IDocumentosPendientesInput;
        operacionesFirmadas: app.domain.IDocumento[];
        documentosFirmados: app.domain.IDocumento[];
        documentosFirmadosInput: app.domain.IDocumentosFirmadosInput;
        fechaFirmadosInicio: Date;
        fechaFirmadosFin: Date;
        fechaHoy: Date;

        static $inject = ['constantService', 'dataService', 'authService', 'extrasService'];
        constructor(private constantService: app.common.services.ConstantService,
            private dataService: app.common.services.DataService,
            private authService: app.common.services.AuthService,
            private extrasService: app.common.services.ExtrasService) {

            this.setTemplates();
            this.seccionId = 0;
            this.seleccionarSeccion(this.seccionId);

            this.fechaHoy = new Date();

            this.documentosPendientesInput = new app.domain.DocumentosPendientesInput(this.extrasService.getRutParteEntera(this.authService.usuario.Rut));
            this.getDocumentosPendientes(this.documentosPendientesInput);

            this.fechaFirmadosFin = new Date();
            this.fechaFirmadosInicio = new Date();
            this.fechaFirmadosInicio.setDate(this.fechaFirmadosFin.getDate() - 1);

            this.actualizarDocumentosFirmados();
        }

        seleccionarSeccion(id: number): void {
            this.seccionId = id;
            this.seccionURI = 'app/mis-inversiones/' + this.templates[this.seccionId];
        }

        setTemplates(): void {
            this.templates = [];
            this.templates[0] = "estado-documentos_pendientes.html";
            this.templates[1] = "estado-documentos_firmados.html";
        }

        getDocumentosPendientes(input: app.domain.IDocumentosPendientesInput): void {
            this.dataService.postWebService(this.constantService.apiDocumentoURI + 'getListPendientes', input)
                .then((result: app.domain.IDocumento[]) => {
                    this.operacionesPendientes = result["operaciones"];
                    this.documentosPendientes = result["documentos"];
                });
        }

        getDocumentosFirmados(input: app.domain.IDocumentosFirmadosInput): void {
            this.dataService.postWebService(this.constantService.apiDocumentoURI + 'getListFirmados', input)
                .then((result: app.domain.IDocumento[]) => {
                    this.operacionesFirmadas = result["operaciones"];
                    this.documentosFirmados = result["documentos"];
                });
        }

        verDocumento(documento: app.domain.IDocumento): void {
            var documentoLeidoInput: app.domain.IDocumentoLeidoInput = new app.domain.DocumentoLeidoInput(this.extrasService.getRutParteEntera(this.authService.usuario.Rut), documento.Codigo);

            //Abrir documento
            this.extrasService.abrirRuta(documento.Ruta);

            this.dataService.postWebService(this.constantService.apiDocumentoURI + 'setLeido', documentoLeidoInput)
                .then((result: app.domain.IDocumentoLeidoResultado) => {
                    var documentoLeidoResultado: app.domain.IDocumentoLeidoResultado = result;
                    if (result.Resultado == true) {
                        documento.Leido = "Leido"; // valor? -KUNDER
                    }
                });
        }

        actualizarDocumentosFirmados(): void {
            this.documentosFirmadosInput = new app.domain.DocumentosFirmadosInput(this.extrasService.getRutParteEntera(this.authService.usuario.Rut), this.getFechaFormato(this.fechaFirmadosInicio), this.getFechaFormato(this.fechaFirmadosFin));
            this.getDocumentosFirmados(this.documentosFirmadosInput);
        }

        //Aquí debería usarse filtro de angular - KUNDER
        getFechaFormato(fecha: Date): string {
            var yyyy = fecha.getFullYear().toString();
            var mm = (fecha.getMonth() + 1).toString(); // getMonth() is zero-based
            var dd = fecha.getDate().toString();
            return yyyy + "-" + (mm[1] ? mm : "0" + mm[0]) + "-" + (dd[1] ? dd : "0" + dd[0]); // padding
        }
    }
    angular.module('tannerPrivadoApp')
        .controller('MisInversionesDocumentosCtrl', MisInversionesDocumentosCtrl);
}