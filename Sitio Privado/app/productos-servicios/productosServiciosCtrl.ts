module app.productosServicios {

    interface IProductosServiciosViewModel {
        categoriaCliente: app.domain.ICategoria;
        categoriaClienteInput: app.domain.ICategoriaClienteInput;
        productos: app.domain.IProducto[];
        getCategoriaCliente(input: app.domain.ICategoriaClienteInput): void;
        getProductos(): void;
    }

    class ProductosServiciosCtrl implements IProductosServiciosViewModel {
        
        categoriaCliente: app.domain.ICategoria;
        categoriaClienteInput: app.domain.ICategoriaClienteInput;
        productos: app.domain.IProducto[];

        static $inject = ['constantService', 'dataService', 'authService', 'extrasService', '$anchorScroll'];
        constructor(private constantService: app.common.services.ConstantService,
            private dataService: app.common.services.DataService,
            private authService: app.common.services.AuthService,
            private extrasService: app.common.services.ExtrasService,
            private $anchorScroll: ng.IAnchorScrollService) {

            this.categoriaClienteInput = new app.domain.CategoriaClienteInput(parseInt(this.extrasService.getRutParteEntera(this.authService.usuario.Rut)));
            this.getCategoriaCliente(this.categoriaClienteInput);
        }

        getCategoriaCliente(input: app.domain.ICategoriaClienteInput): void {
            this.dataService.postWebService(this.constantService.apiCategoriaURI + 'getSingleCliente', input)
                .then((result: app.domain.ICategoria) => {
                    this.categoriaCliente = result;
                    this.getProductos();
                });
        }

        getProductos(): void {
            this.dataService.get(this.constantService.apiProductoURI + 'getList')
                .then((result: app.domain.IProducto[]) => {
                    this.productos = result.filter((producto: domain.IProducto) => {
                        return producto.Categorias.some((categoria: domain.Categoria) => {
                            return categoria.Identificador == this.categoriaCliente.Identificador;
                        });
                    });
                });
        }

        scrollTo(id: string): void {
            this.$anchorScroll(id);
        }
    }
    angular.module('tannerPrivadoApp')
        .controller('ProductosServiciosCtrl', ProductosServiciosCtrl);
}