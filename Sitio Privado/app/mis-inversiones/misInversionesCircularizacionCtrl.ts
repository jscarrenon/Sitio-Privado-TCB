module app.misInversiones {

    class MisInversionesCircularizacionCtrl extends MisInversionesCtrl {

        setTemplates(): void {
            this.templates = [];
            this.templates[0] = "circularizacion_pendiente.html";
            this.templates[1] = "circularizacion_anterior.html";
        }
    }
    angular.module('tannerPrivadoApp')
        .controller('MisInversionesCircularizacionCtrl', MisInversionesCircularizacionCtrl);
}