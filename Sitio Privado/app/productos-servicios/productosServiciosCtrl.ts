module app.productosServicios {

    interface IProductosServiciosViewModel {
        categoriaCliente: app.domain.ICategoria;
        categoriaClienteInput: app.domain.ICategoriaClienteInput;

        getCategoriaCliente(input: app.domain.ICategoriaClienteInput): void;
    }

    class ProductosServiciosCtrl implements IProductosServiciosViewModel {
        
        categoriaCliente: app.domain.ICategoria;
        categoriaClienteInput: app.domain.ICategoriaClienteInput;

        static $inject = ['constantService', 'dataService', 'authService', 'extrasService'];
        constructor(private constantService: app.common.services.ConstantService,
            private dataService: app.common.services.DataService,
            private authService: app.common.services.AuthService,
            private extrasService: app.common.services.ExtrasService) {

            this.categoriaClienteInput = new app.domain.CategoriaClienteInput(parseInt(this.extrasService.getRutParteEntera(this.authService.usuario.Rut)));
            this.getCategoriaCliente(this.categoriaClienteInput);
        }

        getCategoriaCliente(input: app.domain.ICategoriaClienteInput): void {
            this.dataService.postWebService(this.constantService.apiCategoriaURI + 'getSingleCliente', input)
                .then((result: app.domain.ICategoria) => {
                    this.categoriaCliente = result;
                });
        }
    }
    angular.module('tannerPrivadoApp')
        .controller('ProductosServiciosCtrl', ProductosServiciosCtrl);
}