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
        btnVolver: boolean;
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
        btnConfirmacionRechazo: boolean;
        divInformativoRechazo: boolean;
        btnVolver: boolean;
        classModal: string;
        
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
            this.btnConfirmacionRechazo = false;
            this.divInformativoRechazo = false;
            this.btnVolver = false;
            this.classModal = "modal-style modal4";
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
                this.classModal = "modal-style modal2";
                this.divInformativo = true;
                this.btnAceptar = false;
                this.btnRechazar = false;
                this.btnCerrar = true;
                this.condiciones = false;
            }    
        }
        

        rechazar(): void {
            if (this.aceptaCondiciones) {
                this.classModal = "modal-style modal2";
                this.btnAceptar = false;
                this.btnRechazar = false;
                this.condiciones = false;
                this.btnConfirmacionRechazo = true;
                this.divInformativoRechazo = true;
                this.btnVolver = true;
            }

        }


        confirmacionRechazo(): void{
            if (this.aceptaCondiciones) {
                this.authService.setSusFirmaElecDoc('rechazado', '0');
                this.$uibModalInstance.dismiss('cancelar');
            }
        }

        volver(): void {

            this.btnAceptar = true;
            this.btnRechazar = true;
            this.condiciones = true;
            this.btnConfirmacionRechazo = false;
            this.divInformativoRechazo = false;
            this.btnVolver = false;
            this.classModal = "modal-style modal4";
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