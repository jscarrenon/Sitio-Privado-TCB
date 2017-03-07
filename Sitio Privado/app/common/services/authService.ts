module app.common.services {
    export interface IAuthenticationRouteParams extends ng.route.IRouteParamsService {
        accessToken?: string;
    }
    interface IAuth {
        autenticado: boolean;
        usuario: app.domain.IUsuario;
        getUsuarioActual(): void;
        cerrarSesion(): void;
        circularizacionPendiente: boolean;
        getCircularizacionPendiente(): void;
        documentosPendientes: number;
        susFirmaElecDoc: number;
        getDocumentosPendientes(): void;
        setSusFirmaElecDoc(glosa: string, respuesta: string): ng.IPromise<number>;
        fechaCircularizacion: Date;
        getFechaCircularizacion(): void;
        validateToken(accessToken: string): ng.IPromise<app.domain.IUsuario>;
        setUsuario(usuario: app.domain.IUsuario): void;
    }

    export class AuthService implements IAuth {
        autenticado: boolean;
        usuario: app.domain.IUsuario;
        circularizacionPendiente: boolean;
        documentosPendientes: number;
        susFirmaElecDoc: number;
        fechaCircularizacion: Date;
        private qService: ng.IQService;
                
        static $inject = ['$window', '$location', 'constantService', 'dataService', 'extrasService', '$q', '$routeParams'];
        constructor(private $window: ng.IWindowService,
            private $location: ng.ILocationService,
            private constantService: ConstantService,
            private dataService: DataService,
            private extrasService: ExtrasService,
            $q: ng.IQService,
            private $routeParams: IAuthenticationRouteParams) { 

            this.fechaCircularizacion = null;
            this.circularizacionPendiente = false;
            this.documentosPendientes = 0;
            var token = $location.absUrl().split('=')[1];
            this.validateToken(token);
            //console.log(token);
            //this.getUsuarioActual();
            this.getSusFirmaElecDoc();
            this.qService = $q;
        }

        getUsuarioActual(): void {
            console.log('getUsuarioActual');
            this.dataService.getSingle(this.constantService.mvcHomeURI + 'GetUsuarioActual').then((result: app.domain.IUsuario) => {
                console.log(result);

                this.usuario = result;
                if (this.usuario.Autenticado) {
                    this.autenticado = true;
                    this.getFechaCircularizacion();
                    this.getCircularizacionPendiente();
                    this.getDocumentosPendientes();
                }
                else {
                    this.autenticado = true;
                    this.fechaCircularizacion = null;
                    this.circularizacionPendiente = false;
                    this.documentosPendientes = 0;
                }
            });
        }
        setUsuario(usuario: app.domain.IUsuario): void {
            if (usuario != null && usuario.Autenticado) {
                console.log('setUsuarioActual');

                this.autenticado = true;
                this.usuario = usuario;
            }

        }
        cerrarSesion(): void {
            this.autenticado = false;
            this.circularizacionPendiente = false;
            this.documentosPendientes = 0;
            this.usuario = null;
            this.extrasService.abrirRuta(this.constantService.mvcSignOutURI, "_self");
        }

        getFechaCircularizacion(): void {
            var input: app.domain.ICircularizacionFechaInput = new app.domain.CircularizacionFechaInput();
            this.dataService.postWebService(this.constantService.apiCircularizacionURI + 'getFecha', input)
                .then((result: Date) => {
                    this.fechaCircularizacion = result;
                });
        }

        getCircularizacionPendiente(): void {
            var input: app.domain.ICircularizacionPendienteInput = new app.domain.CircularizacionPendienteInput();
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

        getSusFirmaElecDoc(): void {
            this.dataService.postWebService(this.constantService.apiDocumentoURI + 'GetConsultaRespuestaSusFirmaElecDoc','')
                .then((result: number) => {
                    this.susFirmaElecDoc = result;
                });
        }

        setSusFirmaElecDoc(glosa: string, respuesta: string): ng.IPromise<number> {
            var self = this;
            var deferred = self.qService.defer();
            var input: app.domain.ISuscripcionFirmaElectronica = new app.domain.SuscripcionFirmaElectronica(respuesta,glosa);
            this.dataService.postWebService(this.constantService.apiDocumentoURI + 'SetRespuestaSusFirmaElecDoc', input )
                .then((result: number) => {
                    deferred.resolve(result);
                }).finally(() => deferred.resolve(-1));

            return deferred.promise;
        }

        validateToken(accessToken: string): ng.IPromise<app.domain.IUsuario>  {
            var self = this;
            var response = this.dataService.postVerifyLogin(this.constantService.apiAutenticacion + 'verifylogin', null, accessToken)
                .then((result: app.domain.IUsuario) => {
                    this.setUsuario(result);

                    return result;
                });

           return response;
        }
    }

    angular.module('tannerPrivadoApp')
        .service('authService', AuthService);
}