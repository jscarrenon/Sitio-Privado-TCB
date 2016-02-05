module app.productosServicios {

    class ProductosServiciosCtrl {
        static $inject = ['constantService', 'dataService'];
        constructor(private constantService: app.common.services.ConstantService,
            private dataService: app.common.services.DataService) {
        }
    }
    angular.module('tannerPrivadoApp')
        .controller('ProductosServiciosCtrl', ProductosServiciosCtrl);
}