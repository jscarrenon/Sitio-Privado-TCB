module app.common.services {

    interface IAuth {
        autenticado: boolean;
        usuario: app.domain.IUsuario;
        getUsuarioActual(): void;
        cerrarSesion(): void;
        circularizacionPendiente: boolean;
        getCircularizacionPendiente(): void;
        documentosPendientes: number;
        getDocumentosPendientes(): void;
    }

    export class AuthService implements IAuth {
        autenticado: boolean;
        usuario: app.domain.IUsuario;
        circularizacionPendiente: boolean;
        documentosPendientes: number;
                
        static $inject = ['constantService', 'dataService', 'extrasService'];
        constructor(private constantService: ConstantService,
            private dataService: DataService,
            private extrasService: ExtrasService) {
            this.circularizacionPendiente = false;
            this.documentosPendientes = 0;
            this.getUsuarioActual();
        }

        getUsuarioActual(): void {
            this.dataService.getSingle(this.constantService.mvcHomeURI + 'GetUsuarioActual').then((result: app.domain.IUsuario) => {
                this.usuario = result;
                if (this.usuario.Autenticado) {
                    this.autenticado = true;
                    this.getCircularizacionPendiente();
                    this.getDocumentosPendientes();
                }
                else {
                    this.autenticado = false;
                    this.circularizacionPendiente = false;
                    this.documentosPendientes = 0;
                }
            });
        }

        cerrarSesion(): void {
            this.autenticado = false;
            this.circularizacionPendiente = false;
            this.documentosPendientes = 0;
            this.usuario = null;
            this.extrasService.abrirRuta(this.constantService.mvcSignOutURI, "_self");
        }

        getCircularizacionPendiente(): void {
            var fecha: Date = new Date(); // Temporal --KUNDER
            var input: app.domain.ICircularizacionPendienteInput = new app.domain.CircularizacionPendienteInput(this.extrasService.getFechaFormato(fecha));
            this.dataService.postWebService(this.constantService.apiCircularizacionURI + 'getPendiente', input)
                .then((result: app.domain.ICircularizacionProcesoResultado) => {
                    this.circularizacionPendiente = result.Resultado;
                });
        }

        getDocumentosPendientes(): void {
            var input: app.domain.IDocumentosPendientesCantidadInput = new app.domain.DocumentosPendientesCantidadInput();
            this.dataService.postWebService(this.constantService.apiDocumentoURI + 'getCantidadPendientes', input)
                .then((result: app.domain.IDocumentosPendientesCantidadResultado) => {
                    this.documentosPendientes = result.Resultado;
                });
        }
    }

    angular.module('tannerPrivadoApp')
        .service('authService', AuthService);
}