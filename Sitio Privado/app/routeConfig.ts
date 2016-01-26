module App.Config {
    export class RouteConfig {
        constructor(private $routeProvider: ng.route.IRouteProvider) {
            this.Init();
        }

        private Init(): void {
            this.$routeProvider
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
                .when("/informacion-financiera", {
                    templateUrl: "/app/informacion-financiera/index.html",
                    controller: "InformacionFinancieraCtrl as ctrl"
                })
                .otherwise({ redirectTo: '/' });
        }
    }
}