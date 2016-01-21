module app.misInversiones {

    interface IMisInversionesNacionalesViewModel {
        balance: app.domain.IBalance;
        getBalance(input: app.domain.IBalanceInput): void;
        cartola: app.domain.ICartola;
        getCartola(input: app.domain.ICartolaInput): void;    
    }

    class MisInversionesNacionalesCtrl extends MisInversionesCtrl implements IMisInversionesNacionalesViewModel {

        balance: app.domain.IBalance;
        balanceInput: app.domain.IBalanceInput;
        cartola: app.domain.ICartola;
        cartolaInput: app.domain.ICartolaInput;

        static $inject = ['constantService', 'dataService', 'authService', 'extrasService', '$routeParams'];
        constructor(constantService: app.common.services.ConstantService,
            dataService: app.common.services.DataService,
            authService: app.common.services.AuthService,
            extrasService: app.common.services.ExtrasService,
            $routeParams: app.misInversiones.IMisInversionesRouteParams) {

            super(constantService, dataService, authService, extrasService, $routeParams);

            this.balanceInput = new app.domain.BalanceInput(this.authService.usuario.Rut);
            this.getBalance(this.balanceInput);

            this.cartolaInput = new app.domain.CartolaInput(this.extrasService.getRutParteEntera(this.authService.usuario.Rut), 0);
            this.getCartola(this.cartolaInput);
        }

        getBalance(input: app.domain.IBalanceInput): void {
            this.dataService.postWebService(this.constantService.apiBalanceURI + 'getSingle', input)
                .then((result: app.domain.IBalance) => {
                    this.balance = result;
                });
        }

        getCartola(input: app.domain.ICartolaInput): void {
            this.dataService.postWebService(this.constantService.apiCartolaURI + 'getSingle', input)
                .then((result: app.domain.ICartola) => {
                    this.cartola = result;
                });
        }
    }
    angular.module('tannerPrivadoApp')
        .controller('MisInversionesNacionalesCtrl', MisInversionesNacionalesCtrl);
}