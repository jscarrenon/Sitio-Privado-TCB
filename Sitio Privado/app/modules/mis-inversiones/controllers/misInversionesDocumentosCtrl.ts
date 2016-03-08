﻿module app.misInversiones {

    interface IMisInversionesDocumentosViewModel extends app.common.interfaces.ISeccion {
        operacionesPendientes: app.domain.IDocumento[];
        documentosPendientes: app.domain.IDocumento[];
        getDocumentosPendientes(input: app.domain.IDocumentosPendientesInput): void;
        pendientesLoading: boolean;
        operacionesFirmadas: app.domain.IDocumento[];
        documentosFirmados: app.domain.IDocumento[];
        getDocumentosFirmados(input: app.domain.IDocumentosFirmadosInput): void;
        firmadosLoading: boolean;
        fechaFirmadosInicio: Date;
        fechaFirmadosFin: Date;
        verDocumento(documento: app.domain.IDocumento): void;
        firmarDocumentos(): void;
        firmarLoading: boolean;
        fechaHoy: Date;
        actualizarDocumentosPendientes(): void;
        actualizarDocumentosFirmados(): void;
        operacionesPendientesPaginaActual: number;
        operacionesPendientesPorPagina: number;
        documentosPendientesPaginaActual: number;
        documentosPendientesPorPagina: number;
        operacionesFirmadasPaginaActual: number;
        operacionesFirmadasPorPagina: number;
        documentosFirmadosPaginaActual: number;
        documentosFirmadosPorPagina: number;
        configurarPaginacion(): void;
        declaracion: boolean;
        todasOperaciones: boolean;
        todosDocumentos: boolean;
        toggleTodasOperaciones(): void;
        toggleTodosDocumentos(): void;
        opcionOperacionToggled(): void;
        opcionDocumentoToggled(): void;
        confirmacion(): void;
        errorFechas: string;
        validarFechas(): void;
        operacionesSeleccionadas(): app.domain.IDocumento[];
        documentosSeleccionados(): app.domain.IDocumento[];
        haySeleccionados(): boolean;
        respuestaMensaje: string;
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
        operacionesPendientesPaginaActual: number;
        operacionesPendientesPorPagina: number;
        documentosPendientesPaginaActual: number;
        documentosPendientesPorPagina: number;
        operacionesFirmadasPaginaActual: number;
        operacionesFirmadasPorPagina: number;
        documentosFirmadosPaginaActual: number;
        documentosFirmadosPorPagina: number;
        declaracion: boolean;
        todasOperaciones: boolean;
        todosDocumentos: boolean;
        pendientesLoading: boolean;
        firmadosLoading: boolean;
        firmarLoading: boolean;
        errorFechas: string;
        respuestaMensaje: string;

        static $inject = ['constantService', 'dataService', 'authService', 'extrasService', '$filter', '$uibModal'];
        constructor(private constantService: app.common.services.ConstantService,
            private dataService: app.common.services.DataService,
            private authService: app.common.services.AuthService,
            private extrasService: app.common.services.ExtrasService,
            private $filter: ng.IFilterService,
            private $uibModal: ng.ui.bootstrap.IModalService) {

            this.setTemplates();
            this.seccionId = 0;
            this.seleccionarSeccion(this.seccionId);
            this.configurarPaginacion();

            this.respuestaMensaje = null;
            this.errorFechas = null;
            this.fechaHoy = new Date();
            this.declaracion = false;
            this.todasOperaciones = false;
            this.todosDocumentos = false;

            this.actualizarDocumentosPendientes();

            this.fechaFirmadosFin = new Date();
            this.fechaFirmadosInicio = new Date();
            this.fechaFirmadosInicio.setDate(this.fechaFirmadosFin.getDate() - 1);

            this.actualizarDocumentosFirmados();
        }

        seleccionarSeccion(id: number): void {
            this.seccionId = id;
            this.seccionURI = this.constantService.buildFolderURI + 'html/modules/mis-inversiones/templates/' + this.templates[this.seccionId];
        }

        setTemplates(): void {
            this.templates = [];
            this.templates[0] = "estado-documentos_pendientes.html";
            this.templates[1] = "estado-documentos_firmados.html";
        }

        configurarPaginacion(): void {
            this.operacionesPendientesPaginaActual = 1;
            this.operacionesPendientesPorPagina = 15;
            this.documentosPendientesPaginaActual = 1;
            this.documentosPendientesPorPagina = 15;
            this.operacionesFirmadasPaginaActual = 1;
            this.operacionesFirmadasPorPagina = 15;
            this.documentosFirmadosPaginaActual = 1;
            this.documentosFirmadosPorPagina = 15;
        }

        validarFechas(): void {
            if (this.fechaFirmadosInicio === undefined || this.fechaFirmadosFin === undefined) {
                if (this.fechaFirmadosInicio === undefined && this.fechaFirmadosFin === undefined)
                    this.errorFechas = 'La fecha "desde" y la fecha "hasta" son inválidas.';
                else if (this.fechaFirmadosInicio === undefined)
                    this.errorFechas = 'La fecha "desde" es inválida.';
                else
                    this.errorFechas = 'La fecha "hasta" es inválida.';
            }
            else if (this.fechaFirmadosInicio > this.fechaFirmadosFin)
                this.errorFechas = 'La fecha "desde" es mayor a la fecha "hasta".';
            else
                this.errorFechas = null;
        }

        getDocumentosPendientes(input: app.domain.IDocumentosPendientesInput): void {
            this.pendientesLoading = true;
            this.dataService.postWebService(this.constantService.apiDocumentoURI + 'getListPendientes', input)
                .then((result: app.domain.IDocumento[]) => {
                    this.operacionesPendientes = result["operaciones"];
                    this.documentosPendientes = result["documentos"];
                })
                .finally(() => this.pendientesLoading = false);
        }

        getDocumentosFirmados(input: app.domain.IDocumentosFirmadosInput): void {
            this.firmadosLoading = true;
            this.dataService.postWebService(this.constantService.apiDocumentoURI + 'getListFirmados', input)
                .then((result: app.domain.IDocumento[]) => {
                    this.operacionesFirmadas = result["operaciones"];
                    this.documentosFirmados = result["documentos"];
                })
                .finally(() => this.firmadosLoading = false);
        }

        verDocumento(documento: app.domain.IDocumento): void {
            var documentoLeidoInput: app.domain.IDocumentoLeidoInput = new app.domain.DocumentoLeidoInput(documento.Codigo);

            //Abrir documento
            this.extrasService.abrirRuta(documento.Ruta);

            this.dataService.postWebService(this.constantService.apiDocumentoURI + 'setLeido', documentoLeidoInput)
                .then((result: app.domain.IDocumentoLeidoResultado) => {
                    var documentoLeidoResultado: app.domain.IDocumentoLeidoResultado = result;
                    if (result.Resultado == true) {
                        documento.Leido = 'S';
                    }
                });
        }

        operacionesSeleccionadas(): app.domain.IDocumento[] {
            return this.$filter('filter')(this.operacionesPendientes, { Seleccionado: true });
        }

        documentosSeleccionados(): app.domain.IDocumento[] {
            return this.$filter('filter')(this.documentosPendientes, { Seleccionado: true });
        }

        haySeleccionados(): boolean {
            if (this.operacionesSeleccionadas().length > 0 || this.documentosSeleccionados().length > 0) {
                return true;
            }
            return false;
        }

        firmarDocumentos(): void {
            this.respuestaMensaje = null;
            var firmarOperacionesLoading: boolean = false;
            var firmarDocumentosLoading: boolean = false;
            this.firmarLoading = firmarOperacionesLoading || firmarDocumentosLoading;

            if (this.declaracion) {
                var operacionesSeleccionadas: app.domain.IDocumento[] = this.operacionesSeleccionadas();
                if (operacionesSeleccionadas) {
                    var operacionCodigo = operacionesSeleccionadas.map(function (documento) { return documento.Codigo; }).join();
                    if (operacionCodigo) {
                        var operacionFirmarInput: app.domain.IOperacionFirmarInput = new app.domain.OperacionFirmarInput(operacionCodigo);

                        firmarOperacionesLoading = true;
                        this.firmarLoading = firmarOperacionesLoading || firmarDocumentosLoading;
                        
                        this.dataService.postWebService(this.constantService.apiDocumentoURI + 'setFirmarOperacion', operacionFirmarInput)
                            .then((result: app.domain.IDocumentoFirmarResultado) => {
                                var operacionFirmarResultado: app.domain.IDocumentoFirmarResultado = result;
                                this.respuestaMensaje = 'ok';
                                this.actualizarDocumentosPendientes();
                                this.actualizarDocumentosFirmados();
                            })
                            .catch(() => this.respuestaMensaje = 'error')
                            .finally(() => { firmarOperacionesLoading = false; this.firmarLoading = firmarOperacionesLoading || firmarDocumentosLoading; });
                    }
                }

                var documentosSeleccionados: app.domain.IDocumento[] = this.documentosSeleccionados();
                if (documentosSeleccionados) {
                    var documentoCodigo: string = documentosSeleccionados.map(function (documento) { return documento.Codigo; }).join();
                    if (documentoCodigo) {
                        var documentoFirmarInput: app.domain.IDocumentoFirmarInput = new app.domain.DocumentoFirmarInput(documentoCodigo);

                        firmarDocumentosLoading = true;
                        this.firmarLoading = firmarOperacionesLoading || firmarDocumentosLoading;

                        this.dataService.postWebService(this.constantService.apiDocumentoURI + 'setFirmarDocumento', documentoFirmarInput)
                            .then((result: app.domain.IDocumentoFirmarResultado) => {
                                var documentoFirmarResultado: app.domain.IDocumentoFirmarResultado = result;
                                this.respuestaMensaje = 'ok';
                                this.actualizarDocumentosPendientes();
                                this.actualizarDocumentosFirmados();
                            })
                            .catch(() => this.respuestaMensaje = 'error')
                            .finally(() => { firmarDocumentosLoading = false; this.firmarLoading = firmarOperacionesLoading || firmarDocumentosLoading; });
                    }
                }
            }
        }

        actualizarDocumentosPendientes(): void {
            this.documentosPendientesInput = new app.domain.DocumentosPendientesInput();
            this.getDocumentosPendientes(this.documentosPendientesInput);
        }

        actualizarDocumentosFirmados(): void {
            this.documentosFirmadosInput = new app.domain.DocumentosFirmadosInput(this.extrasService.getFechaFormato(this.fechaFirmadosInicio), this.extrasService.getFechaFormato(this.fechaFirmadosFin));
            this.getDocumentosFirmados(this.documentosFirmadosInput);
        }

        toggleTodasOperaciones(): void {
            var toggleEstado: boolean = this.todasOperaciones;
            angular.forEach(this.operacionesPendientes, function (documento) { documento.Seleccionado = toggleEstado; });
        }

        toggleTodosDocumentos(): void {
            var toggleEstado: boolean = this.todosDocumentos;
            angular.forEach(this.documentosPendientes, function (documento) { documento.Seleccionado = toggleEstado; });
        }

        opcionOperacionToggled(): void {
            this.todasOperaciones = this.operacionesPendientes.every(function (documento) { return documento.Seleccionado; });
        }

        opcionDocumentoToggled(): void {
            this.todosDocumentos = this.documentosPendientes.every(function (documento) { return documento.Seleccionado; });
        }

        confirmacion(): void {

            var modalInstance: ng.ui.bootstrap.IModalServiceInstance = this.$uibModal.open({
                templateUrl: this.constantService.templateDocumentosConfirmacionModalURI,
                controller: 'ModalInstanceCtrl as modal'
            });

            modalInstance.result.then(
                _ => this.firmarDocumentos()
                , function () {
            });
        }
    }
    angular.module('tannerPrivadoApp')
        .controller('MisInversionesDocumentosCtrl', MisInversionesDocumentosCtrl);
}