module app.informacionFinanciera {

    interface IInformacionFinancieraViewModel {
        fecha: Date;
        indices: app.domain.IIndices;
        getIndices(input: app.domain.IIndicesInput): void;
        loading: boolean;
        errorFecha: string;
        validarFecha(): void;
    }

    class InformacionFinancieraIndicesCtrl implements IInformacionFinancieraViewModel {

        fecha: Date;
        indices: app.domain.IIndices;
        indicesInput: app.domain.IIndicesInput;
        loading: boolean;
        errorFecha: string;

        static $inject = ['$localForage', 'constantService', 'dataService', 'extrasService'];
        constructor(
            private $localForage,
            private constantService: app.common.services.ConstantService,
            private dataService: app.common.services.DataService,
            private extrasService: app.common.services.ExtrasService) {

            this.fecha = new Date();
            this.fecha.setDate(this.fecha.getDate() - 1); //día anterior

            this.actualizarIndices();
        }

        validarFecha(): void {
            if (this.fecha === undefined) {
                this.errorFecha = 'La fecha es inválida.';
            }
            else {
                this.errorFecha = null;
            }
        }

        getIndices(input: app.domain.IIndicesInput): void {
            this.loading = true;

            this.$localForage.getItem('accessToken')
                .then(responseToken => {
                    this.dataService.postWebService(this.constantService.apiIndicesURI + 'getSingle', input, responseToken)
                        .then((result: app.domain.IIndices) => { this.indices = result; })
                        .finally(() => this.loading = false);
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