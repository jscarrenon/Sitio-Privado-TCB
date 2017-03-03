module app.home {
    export interface IInformacionFinancieraRouteParams extends ng.route.IRouteParamsService {
        seccion?: string;
    }

    interface IHomeViewModel {
    }

    class HomeCtrl implements IHomeViewModel {

        static $inject = ['constantService', 'dataService'];
        constructor(private constantService: app.common.services.ConstantService,
            private dataService: app.common.services.DataService,
            private $routeParams: IInformacionFinancieraRouteParams) {
        }
    }
    angular.module('tannerPrivadoApp')
        .controller('HomeCtrl', HomeCtrl);
}