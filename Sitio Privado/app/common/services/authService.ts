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
        checkUserAuthentication(): void;
        saveToken(accessToken: string, refreshToken: string, expiresIn: number): void;
        refreshToken(): ng.IPromise<void>;
        setTimerForRefreshToken(): void;
    }

    export class AuthService implements IAuth {
        autenticado: boolean;
        usuario: app.domain.IUsuario;
        circularizacionPendiente: boolean;
        documentosPendientes: number;
        susFirmaElecDoc: number;
        fechaCircularizacion: Date;
        private qService: ng.IQService;
        private timer: ng.IPromise<any>;

        static $inject = ['$localForage', '$http', '$httpParamSerializer', '$window', '$location', 'constantService', '$timeout', 'dataService', 'extrasService', '$q', '$routeParams'];
        constructor(private $localForage,
            private $http: ng.IHttpService,
            private $httpParamSerializer,
            private $window: ng.IWindowService,
            private $location: ng.ILocationService,
            private constantService: ConstantService,
            private $timeout: angular.ITimeoutService,
            private dataService: DataService,
            private extrasService: ExtrasService,
            $q: ng.IQService,
            private $routeParams: IAuthenticationRouteParams) {

            this.fechaCircularizacion = null;
            this.circularizacionPendiente = false;
            this.documentosPendientes = 0;
            this.usuario = null;
            //this.refreshToken();
            //this.validateToken(token);
           this.checkUserAuthentication();
            //this.getUsuarioActual();
            this.getSusFirmaElecDoc();
            console.log('instacia de authService........');
            this.qService = $q;
        }

        getUsuarioActual(): void {
            this.dataService.getSingle(this.constantService.mvcHomeURI + 'GetUsuarioActual').then((result: app.domain.IUsuario) => {

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
                this.$localForage.setItem('autenticado', true);
                this.autenticado = true;
                this.usuario = usuario;
            }

        }
        cerrarSesion(): void {
            this.autenticado = false;
            this.circularizacionPendiente = false;
            this.documentosPendientes = 0;
            this.usuario = null;
            this.$localForage.removeItem(['accessToken', 'refreshToken', 'expiresIn', 'usuario', 'autenticado']);
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
            this.dataService.postWebService(this.constantService.apiDocumentoURI + 'GetConsultaRespuestaSusFirmaElecDoc', '')
                .then((result: number) => {
                    this.susFirmaElecDoc = result;
                });
        }

        setSusFirmaElecDoc(glosa: string, respuesta: string): ng.IPromise<number> {
            var self = this;
            var deferred = self.qService.defer();
            var input: app.domain.ISuscripcionFirmaElectronica = new app.domain.SuscripcionFirmaElectronica(respuesta, glosa);
            this.dataService.postWebService(this.constantService.apiDocumentoURI + 'SetRespuestaSusFirmaElecDoc', input)
                .then((result: number) => {
                    deferred.resolve(result);
                }).finally(() => deferred.resolve(-1));

            return deferred.promise;
        }

        validateToken(accessToken: string): ng.IPromise<app.domain.IUsuario> {
            console.log('..........accessT...........');
            console.log(accessToken);
            var response = this.dataService.postVerifyLogin(this.constantService.apiAutenticacion + 'verifylogin', null, accessToken)
                .then((result: app.domain.IUsuario) => {
                    this.setUsuario(result);
                    this.$localForage.setItem('usuario', JSON.stringify(result));
                    return result;
                });

            return response;
        }
        checkUserAuthentication() {
            var res = this.$localForage.getItem('autenticado')
                .then((response) => this.autenticado = response);

            this.$localForage.getItem('usuario').then((data) => this.setUsuario(JSON.parse(data)));
        }
        saveToken(accessToken: string, refreshToken: string, expiresIn: number) {
            console.log('save token');
            console.log(accessToken);

            this.$localForage.setItem('accessToken', accessToken);
            this.$localForage.setItem('refreshToken', refreshToken);
            this.$localForage.setItem('expiresIn', expiresIn);
            //this.setTimerForRefreshToken();
        }
        refreshToken(): ng.IPromise<any> {
            return this.$localForage.getItem('refreshToken')
                .then((token) => {
                    console.log('refresh token');
                    if (!token)
                        return;
                    
                    //console.log(token);
                    const refreshRequest = {
                        method: 'POST',
                        url: `${this.constantService.apiOAuthURI}auth/connect/token`,
                        data: this.$httpParamSerializer({
                            'grant_type': 'refresh_token',
                            'refresh_token': token
                        }),
                        headers: {
                            'Authorization': 'Basic ' + btoa(`${this.constantService.userClientId}:${this.constantService.userClientSecret}`),
                            'Content-Type': 'application/x-www-form-urlencoded'
                        }
                    } as ng.IRequestConfig;
                   
                    return this.$http<any>(refreshRequest);
                })
                .then((response) =>
                    this.saveToken(response.data["access_token"], response.data["refresh_token"], response.data["expires_in"]))
                .then(() => this.setTimerForRefreshToken());
        }

        setTimerForRefreshToken() {
            return this.$localForage.getItem('expiresIn')
                .then((expiresIn) => {
                    this.$timeout.cancel(this.timer);
                    this.timer = this.$timeout(() => this.refreshToken(), 1000 * expiresIn / 2);
                });
        }
    }

    angular.module('tannerPrivadoApp')
        .service('authService', AuthService);
}