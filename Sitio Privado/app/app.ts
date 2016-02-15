module app {
    class Config {
        constructor($routeProvider: ng.route.IRouteProvider) {
            var buildFolderURI: string = ".build/";
            $routeProvider
                .when("/", {
                    templateUrl: buildFolderURI + "/html/home/index.html",
                    controller: "HomeCtrl as ctrl"
                })
                .when("/mis-inversiones/:seccion?", {
                    templateUrl: buildFolderURI + "/html/mis-inversiones/index.html",
                    controller: "MisInversionesCtrl as ctrl"
                })
                .when("/inversiones", {
                    templateUrl: buildFolderURI + "/html/inversiones/index.html",
                    controller: "InversionesCtrl as ctrl"
                })
                .when("/mis-datos", {
                    templateUrl: buildFolderURI + "/html/home/mis-datos.html",
                    controller: "HomeCtrl as ctrl"
                })
                .when("/informacion-financiera/:seccion?", {
                    templateUrl: buildFolderURI + "/html/informacion-financiera/index.html",
                    controller: "InformacionFinancieraCtrl as ctrl"
                })
                .when("/productos-servicios/:seccion?", {
                    templateUrl: buildFolderURI + "/html/productos-servicios/index.html",
                    controller: "ProductosServiciosCtrl as ctrl"
                })
                .otherwise({ redirectTo: '/' });
        }
    }
    Config.$inject = ['$routeProvider'];

    var mainApp = angular.module('tannerPrivadoApp', ['ngRoute', 'ui.bootstrap']);
    mainApp.config(Config);
}