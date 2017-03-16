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
                .when("/login/:accessToken?", {
                    templateUrl: buildFolderURI + "html/modules/login/templates/index.html",
                    controller: "AuthenticationCtrl as ctrl"
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
                .otherwise({ redirectTo: '/login:accessToken?' });

            AnalyticsProvider.setAccount('UA-73610006-2')
                .trackUrlParams(true)
                .setPageEvent('$stateChangeSuccess')
                .setDomainName('accesoclientes.tanner.cl');

            $httpProvider.defaults.headers.common['X-Requested-With'] = 'XMLHttpRequest';

            $httpProvider.interceptors.push(['$q', '$injector', '$window', '$location',
                function ($q, $injector, $window, $location) {
                    return {
                        request: function (config) {

                            return config || $q.when(config);
                        },
                        responseError: function (response) {
                            var authService: app.common.services.AuthService = $injector.get('authService');
                            console.log(response);
                            if (response.status === 401) {
                                authService.cerrarSesion();
                                $window.location.href = "https://www.tanner.cl";
                            }
                            return $q.reject(response);
                        }
                    };
                }
            ]);
        }


    }
    Config.$inject = ['$routeProvider', 'AnalyticsProvider', '$httpProvider'];

    var mainApp = angular.module('tannerPrivadoApp', ['LocalForageModule','ngRoute', 'ui.bootstrap', 'platanus.rut', 'angular-google-analytics']);
    mainApp.config(Config);
}