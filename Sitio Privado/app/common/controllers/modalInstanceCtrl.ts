module app.common.controllers {

    interface IModalInstanceViewModel {
        ok(): void;
        cancelar(): void;
        aceptar(): void;
        rechazar(): void;
        fecha: Date;
        aceptaCondiciones: boolean;
        changeAceptaCondiciones(): void;
        btnAceptar: boolean;
        btnRechazar: boolean;
        btnCerrar: boolean;
        divInformativo: boolean;
        condiciones: boolean;
    }

    export class ModalInstanceCtrl implements IModalInstanceViewModel {

        fecha: Date;
        authService: app.common.services.AuthService;
        aceptaCondiciones: boolean;
        static $inject = ['$uibModalInstance', 'authService'];
        btnAceptar: boolean;
        btnRechazar: boolean;
        btnCerrar: boolean;
        divInformativo: boolean;
        condiciones: boolean;
        constructor(private $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance,
                    private authSer: app.common.services.AuthService) {
            
            this.fecha = new Date();
            this.authService = authSer;
            this.aceptaCondiciones = false;
            this.btnAceptar = true;
            this.btnRechazar = true;
            this.btnCerrar = false;
            this.divInformativo = false;
            this.condiciones = true;
        }

        ok(): void {
            this.$uibModalInstance.close();
        }

        cancelar(): void {
            this.$uibModalInstance.dismiss('cancelar');
        }

        aceptar(): void {
            if (this.aceptaCondiciones) {
                this.authService.setSusFirmaElecDoc('aceptado', '1')
                    .then(function (data) {
                        if (data == 1) {
                      }
                    });

                this.divInformativo = true;
                this.btnAceptar = false;
                this.btnRechazar = false;
                this.btnCerrar = true;
                this.condiciones = false;
            }    
        }
        

        rechazar(): void {
            if (this.aceptaCondiciones) {
                this.authService.setSusFirmaElecDoc('rechazado', '0');
                this.$uibModalInstance.close();
            }
        }

        changeAceptaCondiciones(): void {
            if (this.aceptaCondiciones == false) {
                this.aceptaCondiciones = true;
            } else {
                this.aceptaCondiciones = false;
            }
        }
    }

    angular.module('tannerPrivadoApp')
        .controller('ModalInstanceCtrl', ModalInstanceCtrl);
}