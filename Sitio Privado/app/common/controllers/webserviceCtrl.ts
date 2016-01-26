module app.common.controllers {

    export interface IWebserviceViewModel {
        agente: app.domain.IAgente;
        getAgente(input: app.domain.IAgenteInput): void;
        getFondosMutuos(input: app.domain.IFondoMutuoInput): void;
        getFondosMutuosTotal(): void;
        categoria: app.domain.ICategoria;
        getCategoria(input: app.domain.ICategoriaInput): void;
        producto: app.domain.IProducto;
        getProducto(input: app.domain.IProductoInput): void;
        categorias: app.domain.ICategoria[];
        getCategorias(): void;
        getProductos(): void;
        categoriaCliente: app.domain.ICategoria;
        getCategoriaCliente(input: app.domain.ICategoriaClienteInput): void;
        balance: app.domain.IBalance;
        getBalance(input: app.domain.IBalanceInput): void;
        cartola: app.domain.ICartola;
        getCartola(input: app.domain.ICartolaInput): void;
    }

    export class WebserviceCtrl implements IWebserviceViewModel {

        agente: app.domain.IAgente;
        agenteInput: app.domain.IAgenteInput;
        fondosMutuosRF: app.domain.IFondoMutuo[];
        fondosMutuosRV: app.domain.IFondoMutuo[];
        fondosMutuosInput: app.domain.IFondoMutuoInput;
        fondosMutuosRFTotal: number;
        fondosMutuosRVTotal: number;
        categoria: app.domain.ICategoria;
        categoriaInput: app.domain.ICategoriaInput;
        producto: app.domain.IProducto;
        productoInput: app.domain.IProductoInput;
        categorias: app.domain.ICategoria[];
        productos: app.domain.IProducto[];
        categoriaCliente: app.domain.ICategoria;
        categoriaClienteInput: app.domain.ICategoriaClienteInput;
        balance: app.domain.IBalance;
        balanceInput: app.domain.IBalanceInput;
        cartola: app.domain.ICartola;
        cartolaInput: app.domain.ICartolaInput;

        static $inject = ['constantService', 'dataService'];
        constructor(private constantService: app.common.services.ConstantService,
            private dataService: app.common.services.DataService) {


            /*
            //Test de Agente
            /*this.agenteInput = new app.domain.AgenteInput("8411855-9", 31);
            this.getAgente(this.agenteInput);

            //Test fondos mutuos
            this.fondosMutuosInput = new app.domain.FondoMutuoInput(7094569);
            this.getFondosMutuos(this.fondosMutuosInput);

            //Test de Categoría
            this.categoriaInput = new app.domain.CategoriaInput(1);
            this.getCategoria(this.categoriaInput);

            //Test de Producto
            this.productoInput = new app.domain.ProductoInput(1);
            this.getProducto(this.productoInput);

            //Test de Lista de Categorías
            this.getCategorias();

            //Test de Lista de Productos
            this.getProductos();

            //Test de Categoría de Cliente
            this.categoriaClienteInput = new app.domain.CategoriaClienteInput(10862228);
            this.getCategoriaCliente(this.categoriaClienteInput);*/
        }

        getAgente(input: app.domain.IAgenteInput): void {
            this.dataService.postWebService(this.constantService.apiAgenteURI + 'getSingle', input)
                .then((result: app.domain.IAgente) => {
                    this.agente = result;
                });
        }

        getFondosMutuos(input: app.domain.IFondoMutuoInput): void {
            this.dataService.postWebService(this.constantService.apiFondosMutuosURI + 'getList', input)
                .then((result: app.domain.IFondoMutuo[]) => {
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

        getCategoria(input: app.domain.ICategoriaInput): void {
            this.dataService.postWebService(this.constantService.apiCategoriaURI + 'getSingle', input)
                .then((result: app.domain.ICategoria) => {
                    this.categoria = result;
                });
        }

        getProducto(input: app.domain.IProductoInput): void {
            this.dataService.postWebService(this.constantService.apiProductoURI + 'getSingle', input)
                .then((result: app.domain.IProducto) => {
                    this.producto = result;
                });
        }

        getCategorias(): void {
            this.dataService.get(this.constantService.apiCategoriaURI + 'getList')
                .then((result: app.domain.ICategoria[]) => {
                    this.categorias = result;
            });
        }

        getProductos(): void {
            this.dataService.get(this.constantService.apiProductoURI + 'getList')
                .then((result: app.domain.IProducto[]) => {
                    this.productos = result;
                });
        }

        getCategoriaCliente(input: app.domain.ICategoriaClienteInput): void {
            this.dataService.postWebService(this.constantService.apiCategoriaURI + 'getSingleCliente', input)
                .then((result: app.domain.ICategoria) => {
                    this.categoriaCliente = result;
                });
        }

        getBalance(input: app.domain.IBalanceInput): void {
            this.dataService.postWebService(this.constantService.apiBalanceURI + 'getSingle', input)
                .then((result: app.domain.IBalance) => {
                    this.balance = result;
                });
        }

        getCartola(input: app.domain.ICartolaInput): void {
            this.dataService.postWebService(this.constantService.apiCartolaURI + 'getSingle', input)
                .then((result: app.domain.ICartola) => {
                    this.cartola = result;
                });
        }                
    }

    angular.module('tannerPrivadoApp')
        .controller('WebserviceCtrl', WebserviceCtrl);
}
