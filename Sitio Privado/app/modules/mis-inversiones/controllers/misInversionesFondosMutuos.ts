module app.misInversiones {

    interface IMisInversionesFondosMutuosViewModel {
        fondosMutuosRF: app.domain.IFondoMutuo[];
        fondosMutuosRV: app.domain.IFondoMutuo[];
        fondosMutuosRFTotal: number;
        fondosMutuosRVTotal: number;
        fondosMutuosInput: app.domain.IFondoMutuoInput;
        getFondosMutuos(input: app.domain.IFondoMutuoInput): void;
        getFondosMutuosTotal(): void;
        loading: boolean;
    }

    class MisInversionesFondosMutuosCtrl implements IMisInversionesFondosMutuosViewModel {

        fondosMutuosRF: app.domain.IFondoMutuo[];
        fondosMutuosRV: app.domain.IFondoMutuo[];
        fondosMutuosInput: app.domain.IFondoMutuoInput;
        fondosMutuosRFTotal: number;
        fondosMutuosRVTotal: number;
        loading: boolean;

        static $inject = ['constantService', 'dataService', '$localForage','authService', 'extrasService', '$routeParams'];
        constructor(private constantService: app.common.services.ConstantService,
            private dataService: app.common.services.DataService,
            private $localForage,
            private authService: app.common.services.AuthService,
            private extrasService: app.common.services.ExtrasService,
            private $routeParams: app.misInversiones.IMisInversionesRouteParams) {

            this.fondosMutuosInput = new app.domain.FondoMutuoInput();
            this.getFondosMutuos(this.fondosMutuosInput);
        }

        getFondosMutuos(input: app.domain.IFondoMutuoInput): void {
            this.loading = true;
            this.$localForage.getItem('accessToken')
                .then((responseToken) => {
                    this.dataService.postWebService(this.constantService.apiFondosMutuosURI + 'getList', input, responseToken)
                        .then((result: app.domain.IFondoMutuo[]) => {
                            this.fondosMutuosRF = result["fondosMutuosRF"];
                            this.fondosMutuosRV = result["fondosMutuosRV"];
                            this.getFondosMutuosTotal();
                        })
                        .finally(() => this.loading = false);
                });
        }

        getFondosMutuosTotal(): void {
            this.fondosMutuosRFTotal = 0;
            this.fondosMutuosRVTotal = 0;

            for (var i = 0; i < this.fondosMutuosRF.length; i++) {
                this.fondosMutuosRFTotal += this.fondosMutuosRF[i].Pesos;
            }

            for (var i = 0; i < this.fondosMutuosRV.length; i++) {
                this.fondosMutuosRVTotal += this.fondosMutuosRV[i].Pesos;
            }

        }
    }
    angular.module('tannerPrivadoApp')
        .controller('MisInversionesFondosMutuosCtrl', MisInversionesFondosMutuosCtrl);
}