module app.common.controllers {

    interface IWebserviceViewModel {
        agente: app.domain.IAgente;
        getAgente(input: app.domain.IAgenteInput): void;
    }

    export class WebserviceCtrl implements IWebserviceViewModel {

        agente: app.domain.IAgente;
        agenteInput: app.domain.IAgenteInput;
        fondosMutuos: app.domain.IFondoMutuo[];
        fondosMutuosInput: app.domain.IFondoMutuoInput;

        static $inject = ['constantService', 'dataService'];
        constructor(private constantService: app.common.services.ConstantService,
            private dataService: app.common.services.DataService) {

            this.agenteInput = new app.domain.AgenteInput("8411855-9", 31);
            this.getAgente(this.agenteInput);
            this.fondosMutuosInput = new app.domain.FondoMutuoInput(84118559);
            this.getFondosMutuos(this.fondosMutuosInput);
        }

        getAgente(input: app.domain.IAgenteInput): void {
            this.dataService.postWebService(this.constantService.apiAgenteURI, input)
                .then((result: app.domain.IAgente) => {
                    console.log(result);
                    this.agente = result;
                });
        }

        getFondosMutuos(input: app.domain.IFondoMutuoInput): void {
            this.dataService.postWebService(this.constantService.apiFondosMutuosURI, input)
                .then((result: app.domain.IFondoMutuo[]) => {
                    console.log(result);
                    this.fondosMutuos = result;
                });
        }
    }

    angular.module('tannerPrivadoApp')
        .controller('WebserviceCtrl', WebserviceCtrl);
}