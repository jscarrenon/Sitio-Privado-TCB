module app.common.controllers {

    interface IModalInstanceViewModel {
        ok(): void;
        cancelar(): void;
    }

    export class ModalInstanceCtrl implements IModalInstanceViewModel {

        static $inject = ['$uibModalInstance'];
        constructor(
            private $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance) {
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