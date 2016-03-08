module app {
    class Config {
        constructor($routeProvider: ng.route.IRouteProvider, AnalyticsProvider:ng.google.analytics.AnalyticsProvider) {
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
                .when("/informacion-financiera/:seccion?", {
                    templateUrl: buildFolderURI + "html/modules/informacion-financiera/templates/index.html",
                    controller: "InformacionFinancieraCtrl as ctrl"
                })
                .when("/productos-servicios/:seccion?", {
                    templateUrl: buildFolderURI + "html/modules/productos-servicios/templates/index.html",
                    controller: "ProductosServiciosCtrl as ctrl"
                })
                .otherwise({ redirectTo: '/' });

            AnalyticsProvider.setAccount('UA-73610006-1')
                .trackUrlParams(true)
                .setPageEvent('$stateChangeSuccess')
                .setDomainName('tannerclientesprod.azurewebsites.net');
        }


    }
    Config.$inject = ['$routeProvider','AnalyticsProvider'];

    var mainApp = angular.module('tannerPrivadoApp', ['ngRoute', 'ui.bootstrap', 'platanus.rut', 'angular-google-analytics']);
    mainApp.config(Config);
}