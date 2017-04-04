module app.common.controllers {

    interface IAgenteViewModel {
        agente: app.domain.IAgente;
        agenteInput: app.domain.IAgenteInput;
        getAgente(input: app.domain.IAgenteInput): void;
        loading: boolean;
    }

    export class AgenteCtrl implements IAgenteViewModel {

        agente: app.domain.IAgente;
        agenteInput: app.domain.IAgenteInput;
        loading: boolean;

        static $inject = ['constantService', 'dataService','$localForage', 'authService'];
        constructor(private constantService: app.common.services.ConstantService,
            private dataService: app.common.services.DataService,
            private $localForage,
            private authService: app.common.services.AuthService) {

            this.agenteInput = new app.domain.AgenteInput();
            this.getAgente(this.agenteInput);
        }

        getAgente(input: app.domain.IAgenteInput): void {
            this.loading = true;
            this.$localForage.getItem('accessToken')
                .then((responseToken) => {
                    this.dataService.postWebService(this.constantService.apiAgenteURI + 'getSingle', input, responseToken)
                        .then((result: app.domain.IAgente) => {
                            this.agente = result;
                        })
                        .catch((reason) => { console.log(reason);})
                        .finally(() => this.loading = false);
                })
                .catch((reason) => { console.log(reason); });
        }
    }

    angular.module('tannerPrivadoApp')
        .controller('AgenteCtrl', AgenteCtrl);
}