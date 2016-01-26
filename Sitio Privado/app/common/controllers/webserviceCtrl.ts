module App.Common.Controllers {

    export interface IWebserviceViewModel {
        agente: App.Domain.IAgente;
        getAgente(input: App.Domain.IAgenteInput): void;
        getFondosMutuos(input: App.Domain.IFondoMutuoInput): void;
        getFondosMutuosTotal(): void;
        categoria: App.Domain.ICategoria;
        getCategoria(input: App.Domain.ICategoriaInput): void;
        producto: App.Domain.IProducto;
        getProducto(input: App.Domain.IProductoInput): void;
        categorias: App.Domain.ICategoria[];
        getCategorias(): void;
        getProductos(): void;
        categoriaCliente: App.Domain.ICategoria;
        getCategoriaCliente(input: App.Domain.ICategoriaClienteInput): void;
        balance: App.Domain.IBalance;
        getBalance(input: App.Domain.IBalanceInput): void;
        cartola: App.Domain.ICartola;
        getCartola(input: App.Domain.ICartolaInput): void;
    }

    export class WebserviceCtrl implements IWebserviceViewModel {

        agente: App.Domain.IAgente;
        agenteInput: App.Domain.IAgenteInput;
        fondosMutuosRF: App.Domain.IFondoMutuo[];
        fondosMutuosRV: App.Domain.IFondoMutuo[];
        fondosMutuosInput: App.Domain.IFondoMutuoInput;
        fondosMutuosRFTotal: number;
        fondosMutuosRVTotal: number;
        categoria: App.Domain.ICategoria;
        categoriaInput: App.Domain.ICategoriaInput;
        producto: App.Domain.IProducto;
        productoInput: App.Domain.IProductoInput;
        categorias: App.Domain.ICategoria[];
        productos: App.Domain.IProducto[];
        categoriaCliente: App.Domain.ICategoria;
        categoriaClienteInput: App.Domain.ICategoriaClienteInput;
        balance: App.Domain.IBalance;
        balanceInput: App.Domain.IBalanceInput;
        cartola: App.Domain.ICartola;
        cartolaInput: App.Domain.ICartolaInput;

        static $inject = ['constantService', 'dataService'];
        constructor(private constantService: App.Common.Services.ConstantService,
            private dataService: App.Common.Services.DataService) {


            /*
            //Test de Agente
            /*this.agenteInput = new App.Domain.AgenteInput("8411855-9", 31);
            this.getAgente(this.agenteInput);

            //Test fondos mutuos
            this.fondosMutuosInput = new App.Domain.FondoMutuoInput(7094569);
            this.getFondosMutuos(this.fondosMutuosInput);

            //Test de Categoría
            this.categoriaInput = new App.Domain.CategoriaInput(1);
            this.getCategoria(this.categoriaInput);

            //Test de Producto
            this.productoInput = new App.Domain.ProductoInput(1);
            this.getProducto(this.productoInput);

            //Test de Lista de Categorías
            this.getCategorias();

            //Test de Lista de Productos
            this.getProductos();

            //Test de Categoría de Cliente
            this.categoriaClienteInput = new App.Domain.CategoriaClienteInput(10862228);
            this.getCategoriaCliente(this.categoriaClienteInput);*/
        }

        getAgente(input: App.Domain.IAgenteInput): void {
            this.dataService.postWebService(this.constantService.apiAgenteURI + 'getSingle', input)
                .then((result: App.Domain.IAgente) => {
                    this.agente = result;
                });
        }

        getFondosMutuos(input: App.Domain.IFondoMutuoInput): void {
            this.dataService.postWebService(this.constantService.apiFondosMutuosURI + 'getList', input)
                .then((result: App.Domain.IFondoMutuo[]) => {
                    this.fondosMutuosRF = result["fondosMutuosRF"];
                    this.fondosMutuosRV = result["fondosMutuosRV"];
                    this.getFondosMutuosTotal();
                });
        }

        getFondosMutuosTotal(): void {
            this.fondosMutuosRFTotal = 0;
            this.fondosMutuosRVTotal = 0;

            for (var i = 0; i < this.fondosMutuosRF.length; i++) {
                this.fondosMutuosRFTotal += this.fondosMutuosRF[i].Pesos;
            }

            for (var i = 0; i < this.fondosMutuosRV.length; i++) {
                this.fondosMutuosRVTotal += this.fondosMutuosRV[i].Pesos;
            }

        }

        getCategoria(input: App.Domain.ICategoriaInput): void {
            this.dataService.postWebService(this.constantService.apiCategoriaURI + 'getSingle', input)
                .then((result: App.Domain.ICategoria) => {
                    this.categoria = result;
                });
        }

        getProducto(input: App.Domain.IProductoInput): void {
            this.dataService.postWebService(this.constantService.apiProductoURI + 'getSingle', input)
                .then((result: App.Domain.IProducto) => {
                    this.producto = result;
                });
        }

        getCategorias(): void {
            this.dataService.get(this.constantService.apiCategoriaURI + 'getList')
                .then((result: App.Domain.ICategoria[]) => {
                    this.categorias = result;
            });
        }

        getProductos(): void {
            this.dataService.get(this.constantService.apiProductoURI + 'getList')
                .then((result: App.Domain.IProducto[]) => {
                    this.productos = result;
                });
        }

        getCategoriaCliente(input: App.Domain.ICategoriaClienteInput): void {
            this.dataService.postWebService(this.constantService.apiCategoriaURI + 'getSingleCliente', input)
                .then((result: App.Domain.ICategoria) => {
                    this.categoriaCliente = result;
                });
        }

        getBalance(input: App.Domain.IBalanceInput): void {
            this.dataService.postWebService(this.constantService.apiBalanceURI + 'getSingle', input)
                .then((result: App.Domain.IBalance) => {
                    this.balance = result;
                });
        }

        getCartola(input: App.Domain.ICartolaInput): void {
            this.dataService.postWebService(this.constantService.apiCartolaURI + 'getSingle', input)
                .then((result: App.Domain.ICartola) => {
                    this.cartola = result;
                });
        }

        private static _module: ng.IModule;
        public static get module(): ng.IModule {
            if (this._module) {
                return this._module;
            }
            this._module = angular.module('webserviceCtrl', []);
            this._module.controller('webserviceCtrl', WebserviceCtrl);
            return this._module;
        }
    }
}
