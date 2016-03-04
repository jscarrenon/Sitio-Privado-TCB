module app.common.controllers {

    interface IBodyViewModel {
        seccionId: number;
        seleccionarSeccion(id: number): void;
        crearInstanciaModal(objeto: string): void;
    }

    export class BodyCtrl implements IBodyViewModel {

        seccionId: number;
        
        static $inject = ['constantService', 'dataService', 'authService', 'extrasService','$scope', '$uibModal', '$location','Analytics'];
        constructor(private constantService: app.common.services.ConstantService,
            private dataService: app.common.services.DataService,
            private authService: app.common.services.AuthService,
            private extrasService: app.common.services.ExtrasService,
            private $scope: ng.IScope,
            private $uibModal: ng.ui.bootstrap.IModalService,
            private $location: ng.ILocationService,
            private Analytics: ng.google.analytics.AnalyticsService) {

            this.seccionId = 0;
            this.seleccionarSeccion(this.seccionId);            
            this.$scope.$watch(() => this.authService.circularizacionPendiente,
                (newValue: boolean, oldValue: boolean) => {
                    if ((newValue != oldValue) && newValue)
                        this.crearInstanciaModal("circularizacion");
                });
            this.$scope.$watch(() => this.authService.documentosPendientes,
                (newValue: number, oldValue: number) => {
                    if ((newValue != oldValue) && newValue > 0)
                        this.crearInstanciaModal("documentos");
                });    
            this.$scope.$on('$routeChangeSuccess', (event: any) => {
                Analytics.trackPage(Analytics.getUrl());
            });
        }
        
        seleccionarSeccion(id: number): void {
            this.seccionId = id;
        }

        crearInstanciaModal(objeto: string): void {
            var urlPlantilla= null;
            var ruta = null;

            switch (objeto) {
                case "circularizacion":
                    urlPlantilla = this.constantService.templateCircularizacionModalURI;
                    ruta = '/mis-inversiones/circularizacion';
                    break;
                case "documentos":
                    urlPlantilla = this.constantService.templateDocumentosPendientesModalURI;
                    ruta = '/mis-inversiones/estado-documentos';
                    break;
            };

            if (urlPlantilla != null && ruta != null) {
                var instanciaModal: ng.ui.bootstrap.IModalServiceInstance = this.$uibModal.open({
                    templateUrl: urlPlantilla,
                    controller: 'ModalInstanceCtrl as modal'
                });

                instanciaModal.result.then(_ => this.$location.path(ruta));
            }
        }        
    }

    angular.module('tannerPrivadoApp')
        .controller('BodyCtrl', BodyCtrl);
}