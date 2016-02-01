module app.informacionFinanciera {

    interface IInformacionFinancieraViewModel {
        fecha: Date;
        indices: app.domain.IIndices;
        getIndices(input: app.domain.IIndicesInput): void;
    }

    class InformacionFinancieraIndicesCtrl implements IInformacionFinancieraViewModel {

        fecha: Date;
        indices: app.domain.IIndices;
        indicesInput: app.domain.IIndicesInput;

        static $inject = ['constantService', 'dataService', 'extrasService'];
        constructor(private constantService: app.common.services.ConstantService,
            private dataService: app.common.services.DataService,
            private extrasService: app.common.services.ExtrasService) {

            this.fecha = new Date();
            this.fecha.setDate(this.fecha.getDate() - 1); //día anterior

            this.actualizarIndices();
        }

        getIndices(input: app.domain.IIndicesInput): void {
            this.dataService.postWebService(this.constantService.apiIndicesURI + 'getSingle', input)
                .then((result: app.domain.IIndices) => {
                    this.indices = result;
                });
        }

        actualizarIndices(): void {
            this.indicesInput = new app.domain.IndicesInput(this.extrasService.getFechaFormato(this.fecha, "dd/mm/aaaa"));
            this.getIndices(this.indicesInput);
        }
    }
    angular.module('tannerPrivadoApp')
        .controller('InformacionFinancieraIndicesCtrl', InformacionFinancieraIndicesCtrl);
}