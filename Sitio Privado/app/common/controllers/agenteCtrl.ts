module app.common.controllers {

    interface IAgenteViewModel {
        agente: app.domain.IAgente;
        agenteInput: app.domain.IAgenteInput;
        getAgente(input: app.domain.IAgenteInput): void;
    }

    export class AgenteCtrl implements IAgenteViewModel {

        agente: app.domain.IAgente;
        agenteInput: app.domain.IAgenteInput;

        static $inject = ['constantService', 'dataService', 'authService'];
        constructor(private constantService: app.common.services.ConstantService,
            private dataService: app.common.services.DataService,
            private authService: app.common.services.AuthService) {

            this.agenteInput = new app.domain.AgenteInput(this.authService.usuario.Rut, 0);
            this.getAgente(this.agenteInput);
        }

        getAgente(input: app.domain.IAgenteInput): void {
            this.dataService.postWebService(this.constantService.apiAgenteURI + 'getSingle', input)
                .then((result: app.domain.IAgente) => {
                    this.agente = result;
                });
        }
    }

    angular.module('tannerPrivadoApp')
        .controller('AgenteCtrl', AgenteCtrl);
}