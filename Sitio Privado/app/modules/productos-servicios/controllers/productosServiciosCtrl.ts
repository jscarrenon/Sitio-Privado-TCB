﻿module app.productosServicios {

    interface IProductosServiciosViewModel {
        categoriaCliente: app.domain.ICategoria;
        categoriaClienteInput: app.domain.ICategoriaClienteInput;
        productos: app.domain.IProducto[];
        imagenProducto: {};
        getCategoriaCliente(input: app.domain.ICategoriaClienteInput): void;
        getProductos(): void;
        scrollTo(id: string): void;
        categoriaClienteLoading: boolean;
        productosLoading: boolean;
    }

    class ProductosServiciosCtrl implements IProductosServiciosViewModel {
        
        categoriaCliente: app.domain.ICategoria;
        categoriaClienteInput: app.domain.ICategoriaClienteInput;
        productos: app.domain.IProducto[];
        imagenProducto: {};
        categoriaClienteLoading: boolean;
        productosLoading: boolean;

        static $inject = ['constantService', 'dataService', '$localForage','authService', 'extrasService', '$anchorScroll'];
        constructor(private constantService: app.common.services.ConstantService,
            private dataService: app.common.services.DataService,
            private $localForage,
            private authService: app.common.services.AuthService,
            private extrasService: app.common.services.ExtrasService,
            private $anchorScroll: ng.IAnchorScrollService) {

            this.setImagenes();
            this.categoriaClienteInput = new app.domain.CategoriaClienteInput();
            this.getCategoriaCliente(this.categoriaClienteInput);
        }

        getCategoriaCliente(input: app.domain.ICategoriaClienteInput): void {
            this.categoriaClienteLoading = true;
            this.productosLoading = true;
            this.$localForage.getItem('accessToken')
                .then((responseToken) => {
                    this.dataService.postWebService(this.constantService.apiCategoriaURI + 'getSingleCliente', input, responseToken)
                        .then((result: app.domain.ICategoria) => {
                            this.categoriaCliente = result;
                            this.productos = result.Productos;
                        })
                        .finally(() => { this.categoriaClienteLoading = false; this.productosLoading = false; });
                });
        }

        getProductos(): void {
            this.productosLoading = true;
            this.dataService.get(this.constantService.apiProductoURI + 'getList')
                .then((result: app.domain.IProducto[]) => {
                    this.productos = result.filter((producto: domain.IProducto) => {
                        return producto.Categorias.some((categoria: domain.Categoria) => {
                            return categoria.Identificador == this.categoriaCliente.Identificador;
                        });
                    });
                })
                .finally(() => this.productosLoading = false);
        }

        scrollTo(id: string): void {
            this.$anchorScroll(id);
        }

        setImagenes(): void {
            this.imagenProducto = {};
            this.imagenProducto["1"] = "moneda-ext";
            this.imagenProducto["2"] = "irf";
            this.imagenProducto["3"] = "distribucion-fm";
            this.imagenProducto["5"] = "pactos";
            this.imagenProducto["6"] = "operaciones-prestamo";
            this.imagenProducto["7"] = "operaciones-simultaneas";
            this.imagenProducto["8"] = "renta-fija-inter";
            this.imagenProducto["9"] = "operaciones-simultaneas";
            this.imagenProducto["10"] = "operaciones-prestamo";
            this.imagenProducto["11"] = "moneda-ext";
            this.imagenProducto["12"] = "renta-internacional";
            this.imagenProducto["13"] = "admin-cartera";
            this.imagenProducto["15"] = "operaciones-prestamo";
            this.imagenProducto["16"] = "moneda-ext";
            this.imagenProducto["17"] = "irf";
            this.imagenProducto["18"] = "distribucion-fm";
            this.imagenProducto["19"] = "operaciones-prestamo";
            this.imagenProducto["20"] = "renta-internacional";
            this.imagenProducto["21"] = "pactos";
            this.imagenProducto["22"] = "operaciones-prestamo";
            this.imagenProducto["23"] = "distribucion-fm";
            this.imagenProducto["24"] = "operaciones-simultaneas";
            this.imagenProducto["25"] = "operaciones-simultaneas";
            this.imagenProducto["26"] = "operaciones-prestamo";
            this.imagenProducto["27"] = "moneda-ext";
            this.imagenProducto["28"] = "admin-cartera";
        }
    }
    angular.module('tannerPrivadoApp')
        .controller('ProductosServiciosCtrl', ProductosServiciosCtrl);
}