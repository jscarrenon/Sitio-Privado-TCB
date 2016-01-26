module App.MisInversiones {

    interface IMisInversionesDocumentosViewModel {
        documentosPendientes: App.Domain.IDocumento[];
        getDocumentosPendientes(input: App.Domain.IDocumentosPendientesInput): void;
        documentosFirmados: App.Domain.IDocumento[];
        getDocumentosFirmados(input: App.Domain.IDocumentosFirmadosInput): void;
        fechaFirmadosInicio: Date;
        fechaFirmadosFin: Date;
        verDocumento(documento: App.Domain.IDocumento): void;
        fechaHoy: Date;
        actualizarDocumentosFirmados(): void;
    }

    class MisInversionesDocumentosCtrl extends MisInversionesCtrl implements IMisInversionesDocumentosViewModel {

        documentosPendientes: App.Domain.IDocumento[];
        documentosPendientesInput: App.Domain.IDocumentosPendientesInput;
        documentosFirmados: App.Domain.IDocumento[];
        documentosFirmadosInput: App.Domain.IDocumentosFirmadosInput;
        fechaFirmadosInicio: Date;
        fechaFirmadosFin: Date;
        fechaHoy: Date;

        static $inject = ['constantService', 'dataService', 'authService', 'extrasService', '$routeParams'];
        constructor(constantService: App.Common.Services.ConstantService,
            dataService: App.Common.Services.DataService,
            authService: App.Common.Services.AuthService,
            extrasService: App.Common.Services.ExtrasService,
            $routeParams: App.MisInversiones.IMisInversionesRouteParams) {

            super(constantService, dataService, authService, extrasService, $routeParams);

            this.setTemplates();
            this.seccionId = 0;
            this.seleccionarSeccion(this.seccionId);

            this.fechaHoy = new Date();

            this.documentosPendientesInput = new App.Domain.DocumentosPendientesInput(this.extrasService.getRutParteEntera(this.authService.usuario.Rut));
            this.getDocumentosPendientes(this.documentosPendientesInput);

            this.fechaFirmadosFin = new Date();
            this.fechaFirmadosInicio = new Date();
            this.fechaFirmadosInicio.setDate(this.fechaFirmadosFin.getDate() - 1);

            this.actualizarDocumentosFirmados();
        }

        setTemplates(): void {
            this.templates = [];
            this.templates[0] = "estado-documentos_pendientes.html";
            this.templates[1] = "estado-documentos_firmados.html";
        }

        getDocumentosPendientes(input: App.Domain.IDocumentosPendientesInput): void {
            this.dataService.postWebService(this.constantService.apiDocumentoURI + 'getListPendientes', input)
                .then((result: App.Domain.IDocumento[]) => {
                    this.documentosPendientes = result;
                });
        }

        getDocumentosFirmados(input: App.Domain.IDocumentosFirmadosInput): void {
            this.dataService.postWebService(this.constantService.apiDocumentoURI + 'getListFirmados', input)
                .then((result: App.Domain.IDocumento[]) => {
                    this.documentosFirmados = result;
                });
        }

        verDocumento(documento: App.Domain.IDocumento): void {
            var documentoLeidoInput: App.Domain.IDocumentoLeidoInput = new App.Domain.DocumentoLeidoInput(this.extrasService.getRutParteEntera(this.authService.usuario.Rut), "mercado", documento.Codigo, documento.Folio);

            //Abrir documento
            this.extrasService.abrirRuta(documento.Ruta);

            this.dataService.postWebService(this.constantService.apiDocumentoURI + 'setLeido', documentoLeidoInput)
                .then((result: App.Domain.IDocumentoLeidoResultado) => {
                    var documentoLeidoResultado: App.Domain.IDocumentoLeidoResultado = result;
                    if (result.Resultado == true) {
                        documento.Leido = "Leido"; // valor? -KUNDER
                    }
                });
        }

        actualizarDocumentosFirmados(): void {
            this.documentosFirmadosInput = new App.Domain.DocumentosFirmadosInput(this.extrasService.getRutParteEntera(this.authService.usuario.Rut), this.getFechaFormato(this.fechaFirmadosInicio), this.getFechaFormato(this.fechaFirmadosFin));
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