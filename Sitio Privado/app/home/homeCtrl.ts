module App.Home {

    interface IHomeViewModel {
    }

    class HomeCtrl implements IHomeViewModel {

        static $inject = ['constantService', 'dataService'];
        constructor(private constantService: App.Common.Services.ConstantService,
            private dataService: App.Common.Services.DataService) {
        }
    }
    angular.module('tannerPrivadoApp')
        .controller('HomeCtrl', HomeCtrl);
}