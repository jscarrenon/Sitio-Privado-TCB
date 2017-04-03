module app {

    class Config {
        constructor($routeProvider: ng.route.IRouteProvider,
            AnalyticsProvider: ng.google.analytics.AnalyticsProvider,
            $httpProvider: ng.IHttpProvider) {

            var buildFolderURI: string = ".build/";
            $routeProvider
                .when("/", {
                    templateUrl: buildFolderURI + "html/modules/home/templates/index.html",
                    controller: "HomeCtrl as ctrl"
                })
                .when("/mis-inversiones/:seccion?", {
                    templateUrl: buildFolderURI + "html/modules/mis-inversiones/templates/index.html",
                    controller: "MisInversionesCtrl as ctrl"
                })
                .when("/inversiones", {
                    templateUrl: buildFolderURI + "html/modules/inversiones/templates/index.html",
                    controller: "InversionesCtrl as ctrl"
                })
                .when("/mis-datos", {
                    templateUrl: buildFolderURI + "html/modules/home/templates/mis-datos.html",
                    controller: "HomeCtrl as ctrl"
                })
                .when("/cambiar-contrasena", {
                    templateUrl: buildFolderURI + "html/modules/home/templates/cambiar-contrasena.html",
                    controller: "ChangePasswordCtrl as ctrl"
                })
                .when("/informacion-financiera/:seccion?", {
                    templateUrl: buildFolderURI + "html/modules/informacion-financiera/templates/index.html",
                    controller: "InformacionFinancieraCtrl as ctrl"
                })
                .when("/productos-servicios/:seccion?", {
                    templateUrl: buildFolderURI + "html/modules/productos-servicios/templates/index.html",
                    controller: "ProductosServiciosCtrl as ctrl"
                })
                .otherwise({ redirectTo: '/' });

            AnalyticsProvider.setAccount('UA-73610006-2')
                .trackUrlParams(true)
                .setPageEvent('$stateChangeSuccess')
                .setDomainName('accesoclientes.tanner.cl');

            $httpProvider.defaults.headers.common['X-Requested-With'] = 'XMLHttpRequest';

            $httpProvider.interceptors.push(['$q', '$injector', '$window', '$location', 'constantService',
                function ($q, $injector, $window, $location, constantService: app.common.services.ConstantService) {
                    return {
                        request: function (config) {

                            return config || $q.when(config);
                        },
                        responseError: function (response) {
                            var authService: app.common.services.AuthService = $injector.get('authService');
                            if (response.status === 401) {
                                authService.limpiarUsuarioActual();
                                $window.location.href = constantService.homeTanner;
                            }
                            return $q.reject(response);
                        }
                    };
                }
            ]);
        }


    }
    Config.$inject = ['$routeProvider', 'AnalyticsProvider', '$httpProvider'];

    var mainApp = angular.module('tannerPrivadoApp', ['LocalForageModule', 'ngRoute', 'ui.bootstrap', 'platanus.rut', 'angular-google-analytics']);
    mainApp.config(Config);

    class Run {
        constructor($rootScope: ng.IRootScopeService,
            $location: ng.ILocationService,
            authenticationService: app.common.services.AuthService,
            constantService: app.common.services.ConstantService,
            $window: ng.IWindowService,
            $localForage) {
            $rootScope.$on("$routeChangeStart", function (event, next, current) {
                if (!current && !authenticationService.autenticado) {
                    // two options
                    event.preventDefault();

                    if ($location.path() === '/login') {
                        // we need to validate token
                        var token = $location.search().accessToken;
                        var refreshToken = $location.search().refreshToken;
                        var expiresIn = $location.search().expiresIn;
                        authenticationService.validateToken(token, refreshToken, expiresIn)
                            .then(() => (authenticationService.autenticado = true, $location.path("/").search({})));
                    } else {
                        // we need to make sure out authorization header is valid
                        $localForage.getItem('accessToken')
                            .then((responseToken) => {
                                if (responseToken == null) {
                                    $window.location.href = constantService.homeTanner;
                                } else {
                                    authenticationService.autenticado = true;
                                    $rootScope.$evalAsync(() => $location.path($location.path() + '?'));
                                }
                            });
                    }
                }
            });
        }
    }
    Run.$inject = ['$rootScope', '$location', 'authService', 'constantService', '$window', '$localForage'];
    mainApp.run(Run);
}