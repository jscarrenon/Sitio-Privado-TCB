module app.misInversiones {

    interface IMisInversionesDocumentosViewModel {
        documentosPendientes: app.domain.IDocumento[];
        getDocumentosPendientes(input: app.domain.IDocumentosPendientesInput): void;
        documentosFirmados: app.domain.IDocumento[];
        getDocumentosFirmados(input: app.domain.IDocumentosFirmadosInput): void;
        fechaFirmadosInicio: Date;
        fechaFirmadosFin: Date;
        verDocumento(documento: app.domain.IDocumento): void;
        fechaHoy: Date;
        actualizarDocumentosFirmados(): void;
    }

    class MisInversionesDocumentosCtrl extends MisInversionesCtrl implements IMisInversionesDocumentosViewModel {

        documentosPendientes: app.domain.IDocumento[];
        documentosPendientesInput: app.domain.IDocumentosPendientesInput;
        documentosFirmados: app.domain.IDocumento[];
        documentosFirmadosInput: app.domain.IDocumentosFirmadosInput;
        fechaFirmadosInicio: Date;
        fechaFirmadosFin: Date;
        fechaHoy: Date;

        static $inject = ['constantService', 'dataService', 'authService', 'extrasService', '$routeParams'];
        constructor(constantService: app.common.services.ConstantService,
            dataService: app.common.services.DataService,
            authService: app.common.services.AuthService,
            extrasService: app.common.services.ExtrasService,
            $routeParams: app.misInversiones.IMisInversionesRouteParams) {

            super(constantService, dataService, authService, extrasService, $routeParams);

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

        setTemplates(): void {
            this.templates = [];
            this.templates[0] = "estado-documentos_pendientes.html";
            this.templates[1] = "estado-documentos_firmados.html";
        }

        getDocumentosPendientes(input: app.domain.IDocumentosPendientesInput): void {
            this.dataService.postWebService(this.constantService.apiDocumentoURI + 'getListPendientes', input)
                .then((result: app.domain.IDocumento[]) => {
                    this.documentosPendientes = result;
                });
        }

        getDocumentosFirmados(input: app.domain.IDocumentosFirmadosInput): void {
            this.dataService.postWebService(this.constantService.apiDocumentoURI + 'getListFirmados', input)
                .then((result: app.domain.IDocumento[]) => {
                    this.documentosFirmados = result;
                });
        }

        verDocumento(documento: app.domain.IDocumento): void {
            var documentoLeidoInput: app.domain.IDocumentoLeidoInput = new app.domain.DocumentoLeidoInput(this.extrasService.getRutParteEntera(this.authService.usuario.Rut), "mercado", documento.Codigo, documento.Folio);

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