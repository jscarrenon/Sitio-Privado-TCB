module app.common.controllers {

    interface IWebserviceViewModel {
        agente: app.domain.IAgente;
        getAgente(input: app.domain.IAgenteInput): void;
        getFondosMutuos(input: app.domain.IFondoMutuoInput): void;
        getFondosMutuosTotal(): void;
    }    

    export class WebserviceCtrl implements IWebserviceViewModel {

        agente: app.domain.IAgente;
        agenteInput: app.domain.IAgenteInput;
        fondosMutuosRF: app.domain.IFondoMutuo[];
        fondosMutuosRV: app.domain.IFondoMutuo[];
        fondosMutuosInput: app.domain.IFondoMutuoInput;
        fondosMutuosRFTotal: number;
        fondosMutuosRVTotal: number;

        static $inject = ['constantService', 'dataService'];
        constructor(private constantService: app.common.services.ConstantService,
            private dataService: app.common.services.DataService) {

            this.agenteInput = new app.domain.AgenteInput("8411855-9", 31);
            this.getAgente(this.agenteInput);
            this.fondosMutuosInput = new app.domain.FondoMutuoInput(7094569);
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
                    this.fondosMutuosRF = result["fondosMutuosRF"];
                    this.fondosMutuosRV = result["fondosMutuosRV"];
                    this.getFondosMutuosTotal();                    
                });
        }

        getFondosMutuosTotal(): void {
            this.fondosMutuosRFTotal = 0;
            this.fondosMutuosRVTotal = 0;

            for (var i = 0; i < this.fondosMutuosRF.length; i++) {
                this.fondosMutuosRFTotal += this.fondosMutuosRF[i].pesos;
            }

            for (var i = 0; i < this.fondosMutuosRV.length; i++) {
                this.fondosMutuosRFTotal += this.fondosMutuosRV[i].pesos;
            }
        }
    }

    angular.module('tannerPrivadoApp')
        .controller('WebserviceCtrl', WebserviceCtrl);
}