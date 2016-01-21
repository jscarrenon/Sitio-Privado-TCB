module app.common.controllers {

    interface IBodyViewModel {
        seccionId: number;
        seleccionarSeccion(id: number): void;
        agente: app.domain.IAgente;
        agenteInput: app.domain.IAgenteInput;
        getAgente(input: app.domain.IAgenteInput): void;
    }

    export class BodyCtrl implements IBodyViewModel {

        seccionId: number;
        agente: app.domain.IAgente;
        agenteInput: app.domain.IAgenteInput;

        static $inject = ['constantService', 'dataService', 'authService'];
        constructor(private constantService: app.common.services.ConstantService,
            private dataService: app.common.services.DataService,
            private authService: app.common.services.AuthService) {

            this.seccionId = 0;
            this.seleccionarSeccion(this.seccionId);

            this.agenteInput = new app.domain.AgenteInput(this.authService.usuario.Rut, 0);
            this.getAgente(this.agenteInput);
        }

        seleccionarSeccion(id: number): void {
            this.seccionId = id;
        }

        getAgente(input: app.domain.IAgenteInput): void {
            this.dataService.postWebService(this.constantService.apiAgenteURI + 'getSingle', input)
                .then((result: app.domain.IAgente) => {
                    this.agente = result;
                });
        }
    }

    angular.module('tannerPrivadoApp')
        .controller('BodyCtrl', BodyCtrl);
}