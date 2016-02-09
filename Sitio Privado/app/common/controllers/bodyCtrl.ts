module app.common.controllers {

    interface IBodyViewModel {
        seccionId: number;
        seleccionarSeccion(id: number): void;
        crearInstanciaModal(objeto: string): void;
    }

    export class BodyCtrl implements IBodyViewModel {

        seccionId: number;

        static $inject = ['constantService', 'dataService', 'authService', '$uibModal', '$location'];
        constructor(private constantService: app.common.services.ConstantService,
            private dataService: app.common.services.DataService,
            private authService: app.common.services.AuthService,
            private $uibModal: ng.ui.bootstrap.IModalService,
            private $location: ng.ILocationService) {
            this.seccionId = 0;
            this.seleccionarSeccion(this.seccionId);

            if (this.authService.circularizacionPendiente)
                this.crearInstanciaModal("circularizacion");

            if (this.authService.documentosPendientes > 0)
                this.crearInstanciaModal("documentos");
        }

        seleccionarSeccion(id: number): void {
            this.seccionId = id;
        }

        crearInstanciaModal(objeto: string): void {
            var urlPlantilla= null;
            var ruta = null;

            switch (objeto) {
                case "circularizacion":
                    urlPlantilla = 'app/mis-inversiones/circularizacion_pendiente_modal.html';
                    ruta = '/mis-inversiones/circularizacion';
                    break;
                case "documentos":
                    urlPlantilla = 'app/mis-inversiones/estado-documentos_pendientes_modal.html';
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