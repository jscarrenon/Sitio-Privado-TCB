module app.inversiones {

    class InversionesCtrl {
        static $inject = ['constantService', 'dataService'];
        constructor(private constantService: app.common.services.ConstantService,
            private dataService: app.common.services.DataService) {
        }
    }
    angular.module('tannerPrivadoApp')
        .controller('InversionesCtrl', InversionesCtrl);
}