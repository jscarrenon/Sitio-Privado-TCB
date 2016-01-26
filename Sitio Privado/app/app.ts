module App {
    export class TannerPrivadoMain {
        static createApp(angular: ng.IAngularStatic) {
            angular.module("app.tannerPrivado", [
                "ngRoute",
                MainController.MainAppCtrl.module.name,
                Common.Controllers.BodyCtrl.module.name,
                Common.Controllers.WebserviceCtrl.module.name,
                Common.Services.AuthService.module.name,
                Common.Services.ConstantService.module.name,
                Common.Services.DataService.module.name,
                Common.Services.ExtrasService.module.name                
            ]).config([
                "$routeProvider", ($routeProvider: ng.route.IRouteProvider) => {
                    return new Config.RouteConfig($routeProvider);
                }
            ]);
        }
    }
}

App.TannerPrivadoMain.createApp(angular);


//        constructor($routeProvider: ng.route.IRouteProvider) {
//            $routeProvider
//                .when("/", {
//                    templateUrl: "/app/home/index.html",
//                    controller: "HomeCtrl as ctrl"
//                })
//                .when("/mis-inversiones/:seccion?", {
//                    templateUrl: "/app/mis-inversiones/index.html",
//                    controller: "MisInversionesCtrl as ctrl"
//                })
//                .when("/inversiones", {
//                    templateUrl: "/app/inversiones/index.html",
//                    controller: "InversionesCtrl as ctrl"
//                })
//                .when("/mis-datos", {
//                    templateUrl: "/app/home/mis-datos.html",
//                    controller: "HomeCtrl as ctrl"
//                })
//                .when("/informacion-financiera", {
//                    templateUrl: "/app/informacion-financiera/index.html",
//                    controller: "InformacionFinancieraCtrl as ctrl"
//                })
//                .otherwise({ redirectTo: '/' });
//        }
//    }
//    Config.$inject = ['$routeProvider'];

//    var mainApp = angular.module('tannerPrivadoApp', ['ngRoute']);
//    mainApp.config(Config);
//}