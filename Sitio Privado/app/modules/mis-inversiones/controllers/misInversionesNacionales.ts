module app.misInversiones {

    interface IMisInversionesNacionalesViewModel {
        balance: app.domain.IBalance;
        getBalance(input: app.domain.IBalanceInput): void;
        balanceLoading: boolean;
        cartola: app.domain.ICartola;
        getCartola(input: app.domain.ICartolaInput): void;
        cartolaLoading: boolean;
        getConceptosTitulo(titulo: app.domain.ICartolaTitulo, loadingIndex: number): void;
        cartolaTitulosLoadings: boolean[];
        tieneSimultaneas: boolean;
        tienePactos: boolean;
    }

    class MisInversionesNacionalesCtrl implements IMisInversionesNacionalesViewModel {

        balance: app.domain.IBalance;
        balanceInput: app.domain.IBalanceInput;
        balanceLoading: boolean;
        cartola: app.domain.ICartola;
        cartolaInput: app.domain.ICartolaInput;
        cartolaLoading: boolean;
        cartolaTitulosLoadings: boolean[];
        tieneSimultaneas: boolean;
        tienePactos: boolean;

        static $inject = ['constantService', 'dataService', 'authService', 'extrasService', '$scope'];
        constructor(private constantService: app.common.services.ConstantService,
            private dataService: app.common.services.DataService,
            private authService: app.common.services.AuthService,
            private extrasService: app.common.services.ExtrasService,
            private $scope: ng.IScope) {

            this.cartolaTitulosLoadings = [];

            this.balanceInput = new app.domain.BalanceInput();
            this.getBalance(this.balanceInput);

            this.cartolaInput = new app.domain.CartolaInput();
            this.getCartola(this.cartolaInput);
        }

        getBalance(input: app.domain.IBalanceInput): void {
            this.balanceLoading = true;
            this.dataService.postWebService(this.constantService.apiBalanceURI + 'getSingle', input)
                .then((result: app.domain.IBalance) => {
                    this.balance = result;
                })
                .finally(() => this.balanceLoading = false);
        }

        getCartola(input: app.domain.ICartolaInput): void {
            this.cartolaLoading = true;
            this.dataService.postWebService(this.constantService.apiCartolaURI + 'getSingle', input)
                .then((result: app.domain.ICartola) => {
                    this.cartola = result;
                    this.cartola.Titulos.forEach((value: app.domain.ICartolaTitulo) => { this.cartolaTitulosLoadings.push(false); value.DatosCargados = false; });
                })
                .finally(() => this.cartolaLoading = false);
        }

        getConceptosTitulo(titulo: app.domain.ICartolaTitulo, loadingIndex: number): void {
            if (!titulo.DatosCargados) {
                this.cartolaTitulosLoadings[loadingIndex] = true;
                var input: app.domain.ICartolaTituloInput = new app.domain.CartolaTituloInput(titulo.Codigo);
                this.dataService.postWebService(this.constantService.apiCartolaURI + 'getConceptosTitulo', input)
                    .then((result: app.domain.ICartolaConceptosTituloResultado) => {
                        titulo.Conceptos = result.Conceptos;
                        titulo.Rut = result.Rut;
                        titulo.Periodo = result.Periodo;
                        titulo.DatosCargados = true;
                        if (titulo.Descriptor == 'Renta Variable') {
                            if (!titulo.Conceptos.every((value: app.domain.ICartolaConcepto) => { return value.Concepto != 'Operaciones Simultaneas'; })) {
                                this.tieneSimultaneas = true;
                            };
                        }
                        else if (titulo.Descriptor == 'Renta Fija') {
                            if (!titulo.Conceptos.every((value: app.domain.ICartolaConcepto) => { return value.Concepto != 'Operaciones Pactos'; })) {
                                this.tienePactos = true;
                            };
                        }
                    })
                    .finally(() => this.cartolaTitulosLoadings[loadingIndex] = false);
            }
        }
    }
    angular.module('tannerPrivadoApp')
        .controller('MisInversionesNacionalesCtrl', MisInversionesNacionalesCtrl);
}