﻿module app.common.controllers {

    interface IWebserviceViewModel {
        agente: app.domain.IAgente;
        getAgente(input: app.domain.IAgenteInput): void;
        categoria: app.domain.ICategoria;
        getCategoria(input: app.domain.ICategoriaInput): void;
        producto: app.domain.IProducto;
        getProducto(input: app.domain.IProductoInput): void;
    }

    export class WebserviceCtrl implements IWebserviceViewModel {

        agente: app.domain.IAgente;
        agenteInput: app.domain.IAgenteInput;
        categoria: app.domain.ICategoria;
        categoriaInput: app.domain.ICategoriaInput;
        producto: app.domain.IProducto;
        productoInput: app.domain.IProductoInput;

        static $inject = ['constantService', 'dataService'];
        constructor(private constantService: app.common.services.ConstantService,
            private dataService: app.common.services.DataService) {

            //Test de Agente
            this.agenteInput = new app.domain.AgenteInput("8411855-9", 31);
            this.getAgente(this.agenteInput);

            //Test de Categoría
            this.categoriaInput = new app.domain.CategoriaInput(1);
            this.getCategoria(this.categoriaInput);

            //Test de Producto
            this.productoInput = new app.domain.ProductoInput(1);
            this.getProducto(this.productoInput);

        }

        getAgente(input: app.domain.IAgenteInput): void {
            this.dataService.postWebService(this.constantService.apiAgenteURI, input)
                .then((result: app.domain.IAgente) => {
                    this.agente = result;
                });
        }

        getCategoria(input: app.domain.ICategoriaInput): void {
            this.dataService.postWebService(this.constantService.apiCategoriaURI, input)
                .then((result: app.domain.ICategoria) => {
                    this.categoria = result;
                });
        }

        getProducto(input: app.domain.IProductoInput): void {
            this.dataService.postWebService(this.constantService.apiProductoURI, input)
                .then((result: app.domain.IProducto) => {
                    this.producto = result;
                });
        }
    }

    angular.module('tannerPrivadoApp')
        .controller('WebserviceCtrl', WebserviceCtrl);
}