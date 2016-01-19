module app.common.controllers {

    interface IBodyViewModel {
        seccionId: number;
        seleccionarSeccion(id: number): void;
    }

    export class BodyCtrl implements IBodyViewModel {

        seccionId: number;

        static $inject = ['constantService', 'dataService'];
        constructor(private constantService: app.common.services.ConstantService,
            private dataService: app.common.services.DataService) {

            this.seccionId = 0;
            this.seleccionarSeccion(this.seccionId);
        }

        seleccionarSeccion(id: number): void {
            this.seccionId = id;
        }
    }

    angular.module('tannerPrivadoApp')
        .controller('BodyCtrl', BodyCtrl);
}