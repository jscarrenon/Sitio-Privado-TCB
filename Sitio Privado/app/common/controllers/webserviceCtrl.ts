module app.common.controllers {

    interface IWebserviceViewModel {
        agente: app.domain.IAgente;
        getAgente(input: app.domain.IAgenteInput): void;
        categoria: app.domain.ICategoria;
        getCategoria(input: app.domain.ICategoriaInput): void;
    }

    export class WebserviceCtrl implements IWebserviceViewModel {

        agente: app.domain.IAgente;
        agenteInput: app.domain.IAgenteInput;
        categoria: app.domain.ICategoria;
        categoriaInput: app.domain.ICategoriaInput;

        static $inject = ['constantService', 'dataService'];
        constructor(private constantService: app.common.services.ConstantService,
            private dataService: app.common.services.DataService) {

            //Test de Agente
            this.agenteInput = new app.domain.AgenteInput("8411855-9", 31);
            this.getAgente(this.agenteInput);

            //Test de Categoría
            this.categoriaInput = new app.domain.CategoriaInput(1);
            this.getCategoria(this.categoriaInput);
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
    }

    angular.module('tannerPrivadoApp')
        .controller('WebserviceCtrl', WebserviceCtrl);
}