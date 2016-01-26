module App.MisInversiones {

    interface IMisInversionesNacionalesViewModel {
        balance: App.Domain.IBalance;
        getBalance(input: App.Domain.IBalanceInput): void;
        cartola: App.Domain.ICartola;
        getCartola(input: App.Domain.ICartolaInput): void;    
    }

    class MisInversionesNacionalesCtrl extends MisInversionesCtrl implements IMisInversionesNacionalesViewModel {

        balance: App.Domain.IBalance;
        balanceInput: App.Domain.IBalanceInput;
        cartola: App.Domain.ICartola;
        cartolaInput: App.Domain.ICartolaInput;

        static $inject = ['constantService', 'dataService', 'authService', 'extrasService', '$routeParams'];
        constructor(constantService: App.Common.Services.ConstantService,
            dataService: App.Common.Services.DataService,
            authService: App.Common.Services.AuthService,
            extrasService: App.Common.Services.ExtrasService,
            $routeParams: App.MisInversiones.IMisInversionesRouteParams) {

            super(constantService, dataService, authService, extrasService, $routeParams);

            this.balanceInput = new App.Domain.BalanceInput(this.authService.usuario.Rut);
            this.getBalance(this.balanceInput);

            this.cartolaInput = new App.Domain.CartolaInput(this.extrasService.getRutParteEntera(this.authService.usuario.Rut), 0);
            this.getCartola(this.cartolaInput);
        }

        getBalance(input: App.Domain.IBalanceInput): void {
            this.dataService.postWebService(this.constantService.apiBalanceURI + 'getSingle', input)
                .then((result: App.Domain.IBalance) => {
                    this.balance = result;
                });
        }

        getCartola(input: App.Domain.ICartolaInput): void {
            this.dataService.postWebService(this.constantService.apiCartolaURI + 'getSingle', input)
                .then((result: App.Domain.ICartola) => {
                    this.cartola = result;
                });
        }
    }
    angular.module('tannerPrivadoApp')
        .controller('MisInversionesNacionalesCtrl', MisInversionesNacionalesCtrl);
}