module App.MisInversiones {

    interface IMisInversionesCircularizacionViewModel extends app.common.interfaces.ISeccion {
    }

    class MisInversionesCircularizacionCtrl implements IMisInversionesCircularizacionViewModel {

        templates: string[];
        seccionURI: string;
        seccionId: number;

        constructor() {

            this.setTemplates();
            this.seccionId = 0;
            this.seleccionarSeccion(this.seccionId);
        }

        seleccionarSeccion(id: number): void {
            this.seccionId = id;
            this.seccionURI = 'app/mis-inversiones/' + this.templates[this.seccionId];
        }  

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