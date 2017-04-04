module app.common.services {
    export interface IAuthenticationRouteParams extends ng.route.IRouteParamsService {
        accessToken?: string;
    }
    interface IAuth {
        autenticado: boolean;
        usuario: app.domain.IUsuario;
        getUsuarioActual(): void;
        limpiarUsuarioActual(): void;
        cerrarSesion(): void;
        circularizacionPendiente: boolean;
        getCircularizacionPendiente(): void;
        documentosPendientes: number;
        susFirmaElecDoc: number;
        getDocumentosPendientes(): void;
        setSusFirmaElecDoc(glosa: string, respuesta: string): ng.IPromise<number>;
        fechaCircularizacion: Date;
        getFechaCircularizacion(): void;
        verifyLogin(accessToken: string, refreshToken: string, expiresIn: number): ng.IPromise<app.domain.IUsuario>;
        setUsuario(usuario: app.domain.IUsuario): void;
        checkUserAuthentication(): void;
        saveToken(accessToken: string, refreshToken: string, expiresIn: number): void;
        refreshToken(): ng.IPromise<void>;
        setTimerForRefreshToken(): void;
        sites: app.domain.SiteInformation[];
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
        private httpParamSerializerProvider: any;
        sites: app.domain.SiteInformation[];

        static $inject = [
            '$http',
            '$httpParamSerializer',
            '$localForage',
            '$location',
            '$q',
            '$routeParams',
            '$timeout',
            '$window',
            'constantService',
            'dataService',
            'extrasService'
        ];
        constructor(
            private $http: ng.IHttpService,
            private $httpParamSerializer,
            private $localForage,
            private $location: ng.ILocationService,
            private $q: ng.IQService,
            private $routeParams: IAuthenticationRouteParams,
            private $timeout: angular.ITimeoutService,
            private $window: ng.IWindowService,
            private constantService: ConstantService,
            private dataService: DataService,
            private extrasService: ExtrasService
        ) {
            this.fechaCircularizacion = null;
            this.circularizacionPendiente = false;
            this.documentosPendientes = 0;
            this.sites = [];
            this.checkUserAuthentication();
            
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
            if (usuario) {
                console.log(usuario);
                this.usuario = usuario;
            }

        }

        limpiarUsuarioActual(): void {
            this.autenticado = false;
            this.circularizacionPendiente = false;
            this.documentosPendientes = 0;
            this.usuario = null;
            this.$localForage.removeItem(['accessToken', 'refreshToken', 'expiresIn', 'usuario', 'autenticado']);
        }

        cerrarSesion(): void {
            this.$localForage.getItem('accessToken')
                .then((responseToken) => {
                    if (responseToken) {
                        this.dataService.postWebService(this.constantService.apiAutenticacionURI + 'signout', null, responseToken)
                            .then(() => { })
                            .finally(() => {
                                this.limpiarUsuarioActual();
                                this.$window.location.href = this.constantService.homeTanner;
                            });
                    }
                    else {
                        this.limpiarUsuarioActual();
                        this.$window.location.href = this.constantService.homeTanner;
                    }
                });
        }

        getFechaCircularizacion(): void {
            var input: app.domain.ICircularizacionFechaInput = new app.domain.CircularizacionFechaInput();
            this.$localForage.getItem('accessToken')
                .then((responseToken) => {
                    this.dataService.postWebService(this.constantService.apiCircularizacionURI + 'getFecha', input, responseToken)
                        .then((result: Date) => {
                            this.fechaCircularizacion = result;
                        });
                });
        }

        getCircularizacionPendiente(): void {
            var input: app.domain.ICircularizacionPendienteInput = new app.domain.CircularizacionPendienteInput();
            this.$localForage.getItem('accessToken')
                .then((responseToken) => {
                    this.dataService.postWebService(this.constantService.apiCircularizacionURI + 'getPendiente', input, responseToken)
                        .then((result: app.domain.ICircularizacionProcesoResultado) => {
                            this.circularizacionPendiente = result.Resultado;
                        });
                });
        }

        getDocumentosPendientes(): void {
            var input: app.domain.IDocumentosPendientesCantidadInput = new app.domain.DocumentosPendientesCantidadInput();
            this.$localForage.getItem('accessToken')
                .then((responseToken) => {
                    this.dataService.postWebService(this.constantService.apiDocumentoURI + 'getCantidadPendientes', input, responseToken)
                        .then((result: app.domain.IDocumentosPendientesCantidadResultado) => {
                            this.documentosPendientes = result.Resultado;
                        });
                });
        }

        getSusFirmaElecDoc(): void {
            this.$localForage.getItem('accessToken')
                .then((responseToken) => {
                    if (responseToken) {
                        this.dataService.postWebService(this.constantService.apiDocumentoURI + 'GetConsultaRespuestaSusFirmaElecDoc', '', responseToken)
                            .then((result: number) => {
                                this.susFirmaElecDoc = result;
                            });
                    }
                });
        }

        setSusFirmaElecDoc(glosa: string, respuesta: string): ng.IPromise<number> {
            var self = this;
            var deferred = self.qService.defer();
            var input: app.domain.ISuscripcionFirmaElectronica = new app.domain.SuscripcionFirmaElectronica(respuesta, glosa);
            this.$localForage.getItem('accessToken')
                .then((responseToken) => {
                    this.dataService.postWebService(this.constantService.apiDocumentoURI + 'SetRespuestaSusFirmaElecDoc', input, responseToken)
                        .then((result: number) => {
                            deferred.resolve(result);
                        }).finally(() => deferred.resolve(-1));
                });
            return deferred.promise;
        }

        verifyLogin(accessToken: string, refreshToken: string, expiresIn: number): ng.IPromise<app.domain.IUsuario> {
            var response = this.dataService.postWebService(this.constantService.apiAutenticacionURI + 'verifylogin', null, accessToken)
                .then((result: app.domain.IUsuario) => {
                    this.autenticado = true;
                    this.setUsuario(result);
                    this.$localForage.setItem('usuario', JSON.stringify(result));
                    return result;
                })
                .then((response) => this.saveToken(accessToken, refreshToken, expiresIn))
                .then(() => this.setTimerForRefreshToken());
            return response;
        }

        checkUserAuthentication() {
            if (this.$location.path().indexOf('login') < 1) {
                this.verifyToken();
            } else if (!this.usuario) {
                this.$localForage.getItem('usuario')
                    .then((result) => {
                        console.log("checkUserAuthentication");
                        this.setUsuario(JSON.parse(result));
                        this.getSusFirmaElecDoc();
                        this.getUserSitesByToken();
                    });
            }
        }

        saveToken(accessToken: string, refreshToken?: string, expiresIn?: number) {

            this.$localForage.setItem('accessToken', accessToken);
            if (refreshToken)
                this.$localForage.setItem('refreshToken', refreshToken);
            if (expiresIn)
                this.$localForage.setItem('expiresIn', expiresIn);
        }
       
        refreshToken(): ng.IPromise<any> {
            return this.$localForage.getItem('refreshToken')
                .then((token) => {
                    if (!token)
                        return;

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

        verifyToken() {
            this.$localForage.getItem('accessToken')
                .then((accessTokenResult) => {
                    this.$localForage.getItem('refreshToken')
                        .then((refreshTokenResult) => {
                            this.$localForage.getItem('expiresIn')
                                .then((expiresInResult) => {
                                    this.verifyLogin(accessTokenResult, refreshTokenResult, expiresInResult)
                                        .then((response) => {
                                            console.log("verifyToken");
                                            console.log("llamando getSusFirmaElecDoc y getUserSitesByToken");
                                            this.getSusFirmaElecDoc();
                                            this.getUserSitesByToken();
                                        });
                                });
                        });
                });
        }

        checkRefreshToken() {
            this.$localForage.getItem('refreshToken')
                .then((result) => {
                    if (result)
                        this.$window.location.href = this.constantService.homeTanner;
                    else this.refreshToken();
                });
        }

        getUserSitesByToken(): void {
            this.$localForage.getItem('accessToken')
                .then((responseToken) => {
                    this.$localForage.getItem('refreshToken')
                        .then((refreshTokenResult) => {
                            this.$localForage.getItem('expiresIn')
                                .then((expiresInResult) => {
                                    if (responseToken != null) {
                                        this.dataService.get(this.constantService.tannerAuthenticationAPI + 'usersites')
                                            .then((result: app.domain.SiteInformation[]) => {
                                                result.forEach((site) => {
                                                    site.url = site.url + '?accessToken=' + responseToken + '&refreshToken=' + refreshTokenResult + '&expiresIn=' + expiresInResult;
                                                });
                                                this.sites = result;
                                            });
                                    }
                                });
                        });
                });
        }
    }

    angular.module('tannerPrivadoApp')
        .service('authService', AuthService);
}