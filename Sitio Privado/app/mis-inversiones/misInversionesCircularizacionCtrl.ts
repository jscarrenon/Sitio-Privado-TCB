module app.misInversiones {

    class MisInversionesCircularizacionCtrl extends MisInversionesCtrl {

        setTemplates(): void {
            this.templates = [];
            this.templates[0] = "circularizacion_pendiente.html";
            this.templates[1] = "circularizacion_anterior.html";
            this.templates[2] = "circularizacion_anual-2015.html";
            this.templates[3] = "circularizacion_aprobar.html";
        }
    }
    angular.module('tannerPrivadoApp')
        .controller('MisInversionesCircularizacionCtrl', MisInversionesCircularizacionCtrl);
}