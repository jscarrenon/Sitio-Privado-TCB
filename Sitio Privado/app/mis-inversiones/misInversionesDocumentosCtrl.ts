module app.misInversiones {

    interface IMisInversionesDocumentosViewModel {
        documentosPendientes: app.domain.IDocumento[];
        getDocumentosPendientes(input: app.domain.IDocumentosPendientesInput): void;
        documentosFirmados: app.domain.IDocumento[];
        getDocumentosFirmados(input: app.domain.IDocumentosFirmadosInput): void;
        documentoLeidoResultado: app.domain.IDocumentoLeidoResultado;
        setLeido(input: app.domain.IDocumentoLeidoInput): void;
    }

    class MisInversionesDocumentosCtrl extends MisInversionesCtrl implements IMisInversionesDocumentosViewModel {

        documentosPendientes: app.domain.IDocumento[];
        documentosPendientesInput: app.domain.IDocumentosPendientesInput;
        documentosFirmados: app.domain.IDocumento[];
        documentosFirmadosInput: app.domain.IDocumentosFirmadosInput;
        documentoLeidoResultado: app.domain.IDocumentoLeidoResultado;

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

        setLeido(input: app.domain.IDocumentoLeidoInput): void {
            this.dataService.postWebService(this.constantService.apiDocumentoURI + 'setLeido', input)
                .then((result: app.domain.IDocumentoLeidoResultado) => {
                    this.documentoLeidoResultado = result;
                });
        }
    }
    angular.module('tannerPrivadoApp')
        .controller('MisInversionesDocumentosCtrl', MisInversionesDocumentosCtrl);
}