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
        fondosMutuos: app.domain.IDiccionarioFondo[];
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
                .then((result: app.domain.IDiccionarioFondo[]) => {
                    console.log(result);
                    this.fondosMutuos = result;
                    if (this.fondosMutuos != null) {
                        this.getFondosMutuosTotal();
                    }
                });
        }

        getFondosMutuosTotal(): void {
            this.fondosMutuosRFTotal = 0;
            this.fondosMutuosRVTotal = 0;

            for (var fondoMutuoRF in this.fondosMutuos[0].saldos) {
                this.fondosMutuosRFTotal += fondoMutuoRF.pesos;
            }

            for (var fondoMutuoRV in this.fondosMutuos[1].saldos) {
                this.fondosMutuosRVTotal += fondoMutuoRV.pesos;
            }

        }
    }

    angular.module('tannerPrivadoApp')
        .controller('WebserviceCtrl', WebserviceCtrl);
}