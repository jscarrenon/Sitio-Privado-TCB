module app.common.controllers {

    interface IModalInstanceViewModel {
        ok(): void;
        cancelar(): void;
        fecha: Date;
    }

    export class ModalInstanceCtrl implements IModalInstanceViewModel {

        fecha: Date;

        static $inject = ['$uibModalInstance'];
        constructor(
            private $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance) {

            this.fecha = new Date();
        }

        ok(): void {
            this.$uibModalInstance.close();
        }

        cancelar(): void {
            this.$uibModalInstance.dismiss('cancelar');
        }
    }

    angular.module('tannerPrivadoApp')
        .controller('ModalInstanceCtrl', ModalInstanceCtrl);
}