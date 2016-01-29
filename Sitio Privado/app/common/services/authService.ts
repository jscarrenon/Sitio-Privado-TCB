module app.common.services {

    interface IAuth {
        autenticado: boolean;
        usuario: app.domain.IUsuario;
        getUsuarioActual(): void;
        cerrarSesion(): void;
        circularizacionPendiente: boolean;
        getCircularizacionPendiente(input: app.domain.ICircularizacionPendienteInput): void;
    }

    export class AuthService implements IAuth {
        autenticado: boolean;
        usuario: app.domain.IUsuario;
        circularizacionPendiente: boolean;
                
        static $inject = ['constantService', 'dataService', 'extrasService'];
        constructor(private constantService: ConstantService,
            private dataService: DataService,
            private extrasService: ExtrasService) {
            this.circularizacionPendiente = false;
            this.getUsuarioActual();
        }

        getUsuarioActual(): void {
            this.dataService.getSingle(this.constantService.mvcHomeURI + 'GetUsuarioActual').then((result: app.domain.IUsuario) => {
                this.usuario = result;
                if (this.usuario.Autenticado) {
                    this.autenticado = true;
                    var fecha: Date = new Date(); //Temporal --KUNDER
                    var circularizacionPendienteInput: app.domain.ICircularizacionPendienteInput = new app.domain.CircularizacionPendienteInput(parseInt(this.extrasService.getRutParteEntera(this.usuario.Rut)), this.extrasService.getFechaFormato(fecha));
                    this.getCircularizacionPendiente(circularizacionPendienteInput);
                }
                else {
                    this.autenticado = false;
                    this.circularizacionPendiente = false;
                }
            });
        }

        cerrarSesion(): void {
            this.autenticado = false;
            this.circularizacionPendiente = false;
            this.usuario = null;
            this.extrasService.abrirRuta(this.constantService.mvcSignOutURI, "_self");
        }

        getCircularizacionPendiente(input: app.domain.ICircularizacionPendienteInput): void {
            this.dataService.postWebService(this.constantService.apiCircularizacionURI + 'getPendiente', input)
                .then((result: app.domain.ICircularizacionProcesoResultado) => {
                    this.circularizacionPendiente = result.Resultado;
                    if (this.circularizacionPendiente) {
                        setTimeout(function () {
                            uglipop({
                                class: 'modal-style modal1',
                                source: 'html',
                                content: '<div class="pretitle">Pendiente</div><div class="title">Circularización Anual de Custodia 2015</div><div class="text">Estimado Cliente,<br/> En conformidad a lo dispuesto en la Circular 1962 de la Superintendencia de Valores y Seguros (SVS), solicitamos a usted revisar los informes de saldos, que de acuerdo a nuestros registros se encuentran depositados a su nombre en custodia y/o garantía de Tanner Corredores de Bolsa S.A. al día 30 de Junio de 2015.</div><div class="button green"><a href="#/mis-inversiones/circularizacion" class="clink" onclick="rem();">Continuar</a></div><button class="modal-close" onclick="rem();"></button>'
                            });
                        }, 100);
                    }
                });
        }
    }

    angular.module('tannerPrivadoApp')
        .service('authService', AuthService);
}