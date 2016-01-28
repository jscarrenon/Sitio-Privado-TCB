module app.misInversiones {

    interface IMisInversionesCircularizacionViewModel extends app.common.interfaces.ISeccion {
        pendienteResultado: app.domain.ICircularizacionProcesoResultado;
        getPendiente(input: app.domain.ICircularizacionPendienteInput): void;
        archivo: app.domain.ICircularizacionArchivo;
        getArchivo(input: app.domain.ICircularizacionArchivoInput): void;
        leidaResultado: app.domain.ICircularizacionProcesoResultado;
        setLeida(input: app.domain.ICircularizacionLeidaInput): void;
        respondidaResultado: app.domain.ICircularizacionProcesoResultado;
        setRespondida(input: app.domain.ICircularizacionRespondidaInput): void;             
    }

    class MisInversionesCircularizacionCtrl implements IMisInversionesCircularizacionViewModel {

        templates: string[];
        seccionURI: string;
        seccionId: number;

        pendienteResultado: app.domain.ICircularizacionProcesoResultado;
        archivo: app.domain.ICircularizacionArchivo;
        leidaResultado: app.domain.ICircularizacionProcesoResultado;
        respondidaResultado: app.domain.ICircularizacionProcesoResultado;

        static $inject = ['constantService', 'dataService', 'authService', 'extrasService'];
        constructor(private constantService: app.common.services.ConstantService,
            private dataService: app.common.services.DataService,
            private authService: app.common.services.AuthService,
            private extrasService: app.common.services.ExtrasService) {

            this.pendienteResultado.Resultado = false;

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
            this.templates[1] = "circularizacion_anual.html";
            this.templates[2] = "circularizacion_aprobar.html";
        }

        getPendiente(input: app.domain.ICircularizacionPendienteInput): void {
            this.dataService.postWebService(this.constantService.apiCircularizacionURI + 'getPendiente', input)
                .then((result: app.domain.ICircularizacionProcesoResultado) => {
                    this.pendienteResultado = result;
                });
        }

        getArchivo(input: app.domain.ICircularizacionArchivoInput): void {
            this.dataService.postWebService(this.constantService.apiCircularizacionURI + 'getArchivo', input)
                .then((result: app.domain.ICircularizacionArchivo) => {
                    this.archivo = result;
                });
        }

        setLeida(input: app.domain.ICircularizacionLeidaInput): void {
            this.dataService.postWebService(this.constantService.apiCircularizacionURI + 'getPendiente', input)
                .then((result: app.domain.ICircularizacionProcesoResultado) => {
                    this.leidaResultado = result;
                });
        }

        setRespondida(input: app.domain.ICircularizacionRespondidaInput): void {
            this.dataService.postWebService(this.constantService.apiCircularizacionURI + 'getPendiente', input)
                .then((result: app.domain.ICircularizacionProcesoResultado) => {
                    this.respondidaResultado = result;
                });
        }
    }
    angular.module('tannerPrivadoApp')
        .controller('MisInversionesCircularizacionCtrl', MisInversionesCircularizacionCtrl);
}