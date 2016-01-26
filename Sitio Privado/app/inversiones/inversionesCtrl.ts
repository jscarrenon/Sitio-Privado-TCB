module App.inversiones {

    class InversionesCtrl {
        static $inject = ['constantService', 'dataService'];
        constructor(private constantService: App.Common.Services.ConstantService,
            private dataService: App.Common.Services.DataService) {
        }
    }
    angular.module('tannerPrivadoApp')
        .controller('InversionesCtrl', InversionesCtrl);
}