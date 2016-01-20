module app.misInversiones {

    class MisInversionesDocumentosCtrl extends MisInversionesCtrl {

        static $inject = ['constantService', 'dataService', 'authService', '$routeParams'];
        constructor(constantService: app.common.services.ConstantService,
            dataService: app.common.services.DataService,
            authService: app.common.services.AuthService,
            $routeParams: app.misInversiones.IMisInversionesRouteParams) {

            super(constantService, dataService, authService, $routeParams);

            this.setTemplates();
            this.seccionId = 0;
            this.seleccionarSeccion(this.seccionId);
        }

        setTemplates(): void {
            this.templates = [];
            this.templates[0] = "estado-documentos_pendientes.html";
            this.templates[1] = "estado-documentos_firmados.html";
        }
    }
    angular.module('tannerPrivadoApp')
        .controller('MisInversionesDocumentosCtrl', MisInversionesDocumentosCtrl);
}