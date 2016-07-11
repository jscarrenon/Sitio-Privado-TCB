module app.misInversiones {

    enum TipoDocumento { Cartola, Circularizacion }

    interface IMisInversionesCircularizacionViewModel extends app.common.interfaces.ISeccion {
        fecha: Date;
        getFecha(input: app.domain.ICircularizacionFechaInput): void;
        pendienteResultado: app.domain.ICircularizacionProcesoResultado;
        getPendiente(input: app.domain.ICircularizacionPendienteInput): void;
        pendienteLoading: boolean;
        archivo: app.domain.ICircularizacionArchivo;
        getArchivo(input: app.domain.ICircularizacionArchivoInput): void;
        archivoLoading: boolean;
        leidaResultado: app.domain.ICircularizacionProcesoResultado;
        setLeida(input: app.domain.ICircularizacionLeidaInput): void;
        respondidaResultado: app.domain.ICircularizacionProcesoResultado;
        setRespondida(input: app.domain.ICircularizacionRespondidaInput): void;
        respondidaLoading: boolean;
        verDocumento(tipoDocumento: TipoDocumento): void;
        leida: boolean;
        respuestaInput: app.domain.ICircularizacionRespondidaInput;
        responder(): void;
    }

    class MisInversionesCircularizacionCtrl implements IMisInversionesCircularizacionViewModel {

        templates: string[];
        seccionURI: string;
        seccionId: number;

        fecha: Date;
        fechaInput: app.domain.ICircularizacionFechaInput;
        pendienteResultado: app.domain.ICircularizacionProcesoResultado;
        pendienteInput: app.domain.ICircularizacionPendienteInput;
        archivo: app.domain.ICircularizacionArchivo;
        leidaResultado: app.domain.ICircularizacionProcesoResultado;
        respondidaResultado: app.domain.ICircularizacionProcesoResultado;
        leida: boolean;
        respuestaInput: app.domain.ICircularizacionRespondidaInput;
        pendienteLoading: boolean;
        archivoLoading: boolean;
        respondidaLoading: boolean;

        static $inject = ['constantService', 'dataService', 'authService', 'extrasService'];
        constructor(private constantService: app.common.services.ConstantService,
            private dataService: app.common.services.DataService,
            private authService: app.common.services.AuthService,
            private extrasService: app.common.services.ExtrasService) {
            this.setTemplates();
            this.seccionId = 0;
            this.seleccionarSeccion(this.seccionId);
            this.fechaInput = new app.domain.CircularizacionFechaInput();
            this.getFecha(this.fechaInput);
            this.leida = false;
            this.respuestaInput = new app.domain.CircularizacionRespondidaInput("S", null);
            this.pendienteInput = new app.domain.CircularizacionPendienteInput();
            this.getPendiente(this.pendienteInput);                       
        }

        seleccionarSeccion(id: number): void {
            this.seccionId = id;

            if (this.seccionId == 1) {
                var archivoInput: app.domain.ICircularizacionArchivoInput = new app.domain.CircularizacionArchivoInput();
                this.getArchivo(archivoInput);
            }

            this.seccionURI = this.constantService.buildFolderURI + 'html/modules/mis-inversiones/templates/' + this.templates[this.seccionId];
        }  

        setTemplates(): void {
            this.templates = [];
            this.templates[0] = "circularizacion_pendiente.html";
            this.templates[1] = "circularizacion_anual.html";
            this.templates[2] = "circularizacion_aprobar.html";
        }

        getPendiente(input: app.domain.ICircularizacionPendienteInput): void {
            this.pendienteLoading = true;
            this.dataService.postWebService(this.constantService.apiCircularizacionURI + 'getPendiente', input)
                .then((result: app.domain.ICircularizacionProcesoResultado) => {
                    this.pendienteResultado = result;
                    this.authService.circularizacionPendiente = result.Resultado;
                })
                .finally(() => this.pendienteLoading = false);
        }

        getArchivo(input: app.domain.ICircularizacionArchivoInput): void {
            this.archivoLoading = true;
            this.dataService.postWebService(this.constantService.apiCircularizacionURI + 'getArchivo', input)
                .then((result: app.domain.ICircularizacionArchivo) => {
                    this.archivo = result;
                })
                .finally(() => this.archivoLoading = false);
        }

        setLeida(input: app.domain.ICircularizacionLeidaInput): void {
            this.dataService.postWebService(this.constantService.apiCircularizacionURI + 'setLeida', input)
                .then((result: app.domain.ICircularizacionProcesoResultado) => {
                    this.leidaResultado = result;
                    if (this.leidaResultado.Resultado == true) {
                        this.leida = true;
                    }
                });
        }

        setRespondida(input: app.domain.ICircularizacionRespondidaInput): void {
            this.respondidaLoading = true;
            this.dataService.postWebService(this.constantService.apiCircularizacionURI + 'setRespondida', input)
                .then((result: app.domain.ICircularizacionProcesoResultado) => {
                    this.respondidaResultado = result;
                    if (this.respondidaResultado.Resultado == true) {
                        this.seleccionarSeccion(0);
                        this.getPendiente(this.pendienteInput);
                    }
                })
                .finally(() => this.respondidaLoading = false);
        }

        verDocumento(tipoDocumento: TipoDocumento): void {
            var documentoAbierto: boolean = false;

            //Abrir documento
            if (tipoDocumento == TipoDocumento.Cartola && this.archivo.UrlCartola) {
                this.extrasService.abrirRuta(this.archivo.UrlCartola, '_blank');
                documentoAbierto = true;
            }
            else if (tipoDocumento == TipoDocumento.Circularizacion && this.archivo.UrlCircularizacion) {
                this.extrasService.abrirRuta(this.archivo.UrlCircularizacion);
                documentoAbierto = true;
            }

            if (documentoAbierto) {
                var leidaInput: app.domain.ICircularizacionLeidaInput = new app.domain.CircularizacionLeidaInput();
                this.setLeida(leidaInput);
            }
        }

        responder(): void {
            this.setRespondida(this.respuestaInput);
        }

        getFecha(input: app.domain.ICircularizacionFechaInput): void {
            this.dataService.postWebService(this.constantService.apiCircularizacionURI + 'getFecha', input)
                .then((result: Date) => {
                    this.fecha = result;
                    this.authService.fechaCircularizacion = result;
                });
        }
    }
    angular.module('tannerPrivadoApp')
        .controller('MisInversionesCircularizacionCtrl', MisInversionesCircularizacionCtrl);
}