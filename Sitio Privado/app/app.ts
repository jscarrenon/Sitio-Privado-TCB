module app {
    class Config {
        constructor($routeProvider: ng.route.IRouteProvider) {
            $routeProvider
                .when("/", {
                    templateUrl: "/app/home/index.html",
                    controller: "HomeCtrl as ctrl"
                })
                .when("/mis-inversiones", {
                    templateUrl: "/app/mis-inversiones/index.html",
                    controller: "MisInversionesCtrl as ctrl"
                })
                .when("/inversiones", {
                    templateUrl: "/app/inversiones/index.html",
                    controller: "InversionesCtrl as ctrl"
                })
                .when("/add", {
                    templateUrl: "/app/posts/add.html",
                    controller: "PostAddCtrl as vm"
                })
                .otherwise({ redirectTo: '/' });
        }
    }
    Config.$inject = ['$routeProvider'];

    var mainApp = angular.module('tannerPrivadoApp', ['ngRoute']);
    mainApp.config(Config);
}