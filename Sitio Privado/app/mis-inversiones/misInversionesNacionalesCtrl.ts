module app.misInversiones {

    interface IMisInversionesNacionalesViewModel {
        balance: app.domain.IBalance;
        getBalance(input: app.domain.IBalanceInput): void;
        cartola: app.domain.ICartola;
        getCartola(input: app.domain.ICartolaInput): void;    
    }

    class MisInversionesNacionalesCtrl implements IMisInversionesNacionalesViewModel {

        balance: app.domain.IBalance;
        balanceInput: app.domain.IBalanceInput;
        cartola: app.domain.ICartola;
        cartolaInput: app.domain.ICartolaInput;

        static $inject = ['constantService', 'dataService', 'authService', 'extrasService'];
        constructor(private constantService: app.common.services.ConstantService,
            private dataService: app.common.services.DataService,
            private authService: app.common.services.AuthService,
            private extrasService: app.common.services.ExtrasService) {
            
            this.balanceInput = new app.domain.BalanceInput(this.authService.usuario.Rut);
            this.getBalance(this.balanceInput);

            this.cartolaInput = new app.domain.CartolaInput(this.extrasService.getRutParteEntera(this.authService.usuario.Rut));
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