module app.home {

    interface IHomeViewModel {
    }

    class HomeCtrl implements IHomeViewModel {

        static $inject = ['constantService', 'dataService'];
        constructor(private constantService: app.common.services.ConstantService,
            private dataService: app.common.services.DataService) {
        }
    }
    angular.module('tannerPrivadoApp')
        .controller('HomeCtrl', HomeCtrl);
}