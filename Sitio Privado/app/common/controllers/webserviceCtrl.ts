module app.common.controllers {

    interface IWebserviceViewModel {
        agente: app.domain.IAgente;
        getAgente(input: app.domain.IAgenteInput): void;
    }

    export class WebserviceCtrl implements IWebserviceViewModel {

        agente: app.domain.IAgente;
        agenteInput: app.domain.IAgenteInput;

        static $inject = ['constantService', 'dataService'];
        constructor(private constantService: app.common.services.ConstantService,
            private dataService: app.common.services.DataService) {

            this.agenteInput = new app.domain.AgenteInput("asdsa", 12323);
            this.getAgente(this.agenteInput);
        }

        getAgente(input: app.domain.IAgenteInput): void {
            this.dataService.postWebService(this.constantService.apiAgenteURI, input)
                .then((result: app.domain.IAgente) => {
                    this.agente = result;
                });
        }
    }

    angular.module('tannerPrivadoApp')
        .controller('WebserviceCtrl', WebserviceCtrl);
}