module app {
    class Config {
        constructor($routeProvider: ng.route.IRouteProvider) {
            $routeProvider
                .when("/", {
                    templateUrl: "/app/home/index.html",
                    controller: "HomeCtrl as ctrl"
                })
                .when("/mis-inversiones/:seccion?", {
                    templateUrl: "/app/mis-inversiones/index.html",
                    controller: "MisInversionesCtrl as ctrl"
                })
                .when("/inversiones", {
                    templateUrl: "/app/inversiones/index.html",
                    controller: "InversionesCtrl as ctrl"
                })
                .when("/mis-datos", {
                    templateUrl: "/app/home/mis-datos.html",
                    controller: "HomeCtrl as ctrl"
                })
                .when("/informacion-financiera/:seccion?", {
                    templateUrl: "/app/informacion-financiera/index.html",
                    controller: "InformacionFinancieraCtrl as ctrl"
                })
                .when("/productos-servicios/:seccion?", {
                    templateUrl: "/app/productos-servicios/index.html",
                    controller: "ProductosServiciosCtrl as ctrl"
                })
                .otherwise({ redirectTo: '/' });
        }
    }
    Config.$inject = ['$routeProvider'];

    var mainApp = angular.module('tannerPrivadoApp', ['ngRoute', 'ui.bootstrap.pagination']);
    mainApp.config(Config);
}