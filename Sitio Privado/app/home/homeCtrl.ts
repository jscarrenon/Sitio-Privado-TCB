module app.home {

    class HomeCtrl {
        static $inject = ['constantService', 'dataService'];
        constructor(private constantService: app.common.services.ConstantService,
            private dataService: app.common.services.DataService) {
        }
    }
    angular.module('tannerPrivadoApp')
        .controller('HomeCtrl', HomeCtrl);
}