module app.misInversiones {

    interface IMisInversionesFondosMutuosViewModel {
        fondosMutuosRF: app.domain.IFondoMutuo[];
        fondosMutuosRV: app.domain.IFondoMutuo[];
        fondosMutuosRFTotal: number;
        fondosMutuosRVTotal: number;
        fondosMutuosInput: app.domain.IFondoMutuoInput;
        getFondosMutuos(input: app.domain.IFondoMutuoInput): void;
        getFondosMutuosTotal(): void;
    }

    class MisInversionesFondosMutuosCtrl implements IMisInversionesFondosMutuosViewModel {

        fondosMutuosRF: app.domain.IFondoMutuo[];
        fondosMutuosRV: app.domain.IFondoMutuo[];
        fondosMutuosInput: app.domain.IFondoMutuoInput;
        fondosMutuosRFTotal: number;
        fondosMutuosRVTotal: number;

        static $inject = ['constantService', 'dataService', 'authService', 'extrasService', '$routeParams'];
        constructor(private constantService: app.common.services.ConstantService,
            private dataService: app.common.services.DataService,
            private authService: app.common.services.AuthService,
            private extrasService: app.common.services.ExtrasService,
            private $routeParams: app.misInversiones.IMisInversionesRouteParams) {
            console.log("inicio constructor fmCtrl");
            this.fondosMutuosInput = new app.domain.FondoMutuoInput(parseInt(this.extrasService.getRutParteEntera(this.authService.usuario.Rut)));
            this.getFondosMutuos(this.fondosMutuosInput);
            
            console.log("fin constructor fmCtrl");
        }

        getFondosMutuos(input: app.domain.IFondoMutuoInput): void {
            console.log("fm1");
            console.log('dfdgdfg');
            this.dataService.postWebService(this.constantService.apiFondosMutuosURI + 'getList', input)
                .then((result: app.domain.IFondoMutuo[]) => {
                    console.log("fm2");
                    this.fondosMutuosRF = result["fondosMutuosRF"];
                    this.fondosMutuosRV = result["fondosMutuosRV"];
                    this.getFondosMutuosTotal();
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