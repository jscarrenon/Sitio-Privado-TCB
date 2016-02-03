module app.misInversiones {

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
        firmarDocumentos(): void;
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

        static $inject = ['constantService', 'dataService', 'authService', 'extrasService', '$filter'];
        constructor(private constantService: app.common.services.ConstantService,
            private dataService: app.common.services.DataService,
            private authService: app.common.services.AuthService,
            private extrasService: app.common.services.ExtrasService,
            private $filter: ng.IFilterService) {

            this.setTemplates();
            this.seccionId = 0;
            this.seleccionarSeccion(this.seccionId);
            this.configurarPaginacion();

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
            this.seccionURI = 'app/mis-inversiones/' + this.templates[this.seccionId];
        }

        setTemplates(): void {
            this.templates = [];
            this.templates[0] = "estado-documentos_pendientes.html";
            this.templates[1] = "estado-documentos_firmados.html";
        }

        configurarPaginacion(): void {
            this.operacionesPendientesPaginaActual = 1;
            this.operacionesPendientesPorPagina = 10;
            this.documentosPendientesPaginaActual = 1;
            this.documentosPendientesPorPagina = 10;
            this.operacionesFirmadasPaginaActual = 1;
            this.operacionesFirmadasPorPagina = 10;
            this.documentosFirmadosPaginaActual = 1;
            this.documentosFirmadosPorPagina = 10;
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
                        documento.Leido = 'S'; // valor? -KUNDER
                    }
                });
        }

        firmarDocumentos(): void {

            if (this.declaracion) {
                var operacionCodigo: string = this.$filter('filter')(this.operacionesPendientes, { Seleccionado: true }).map(function (documento) { return documento.Codigo; }).join();
                var operacionFirmarInput: app.domain.IOperacionFirmarInput = new app.domain.OperacionFirmarInput(this.authService.usuario.Rut, operacionCodigo);

                this.dataService.postWebService(this.constantService.apiDocumentoURI + 'setFirmarOperacion', operacionFirmarInput)
                    .then((result: app.domain.IDocumentoFirmarResultado) => {
                        var operacionFirmarResultado: app.domain.IDocumentoFirmarResultado = result;
                        this.actualizarDocumentosPendientes();
                        this.actualizarDocumentosFirmados();
                    });

                var documentoCodigo: string = this.$filter('filter')(this.documentosPendientes, { Seleccionado: true }).map(function (documento) { return documento.Codigo; }).join();
                var documentoFirmarInput: app.domain.IDocumentoFirmarInput = new app.domain.DocumentoFirmarInput(this.authService.usuario.Rut, documentoCodigo);

                this.dataService.postWebService(this.constantService.apiDocumentoURI + 'setFirmarDocumento', documentoFirmarInput)
                    .then((result: app.domain.IDocumentoFirmarResultado) => {
                        var documentoFirmarResultado: app.domain.IDocumentoFirmarResultado = result;
                        this.actualizarDocumentosPendientes();
                        this.actualizarDocumentosFirmados();
                    });
            }
        }

        actualizarDocumentosPendientes(): void {
            this.documentosPendientesInput = new app.domain.DocumentosPendientesInput(this.extrasService.getRutParteEntera(this.authService.usuario.Rut));
            this.getDocumentosPendientes(this.documentosPendientesInput);
        }

        actualizarDocumentosFirmados(): void {
            this.documentosFirmadosInput = new app.domain.DocumentosFirmadosInput(this.extrasService.getRutParteEntera(this.authService.usuario.Rut), this.extrasService.getFechaFormato(this.fechaFirmadosInicio), this.extrasService.getFechaFormato(this.fechaFirmadosFin));
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
    }
    angular.module('tannerPrivadoApp')
        .controller('MisInversionesDocumentosCtrl', MisInversionesDocumentosCtrl);
}