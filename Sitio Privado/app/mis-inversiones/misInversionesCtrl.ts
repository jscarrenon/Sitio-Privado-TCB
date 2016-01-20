module app.misInversiones {

    export interface IMisInversionesRouteParams extends ng.route.IRouteParamsService {
        seccion?: string;
    }

    interface IMisInversionesViewModel {
        templates: string[];
        seccionURI: string;
        seccionId: number;
        seleccionarSeccion(id: number): void;
        setTemplates(): void;
        balance: app.domain.IBalance;
        getBalance(input: app.domain.IBalanceInput): void;
    }

    export class MisInversionesCtrl implements IMisInversionesViewModel {

        templates: string[];
        seccionURI: string;
        seccionId: number;
        balance: app.domain.IBalance;
        balanceInput: app.domain.IBalanceInput;

        static $inject = ['constantService', 'dataService', 'authService', '$routeParams'];
        constructor(private constantService: app.common.services.ConstantService,
            private dataService: app.common.services.DataService,
            private authService: app.common.services.AuthService,
            private $routeParams: IMisInversionesRouteParams) {

            this.setTemplates();
            this.seccionId = 0;

            if (this.$routeParams.seccion) {
                if (this.$routeParams.seccion == 'estado-documentos') {
                    this.seccionId = 3;
                }
            }            

            this.seleccionarSeccion(this.seccionId);

            this.balanceInput = new app.domain.BalanceInput(this.authService.usuario.Rut);
            this.getBalance(this.balanceInput);

            //Solucionar problema de script slickav (a.mobileNav.on) porque afecta el resto del controlador KUNDER
            //Timeout por error de script slicknav (a.mobileNav.on)
            /*setTimeout(function () {
                (<any>$('#menu2')).slicknav({
                    label: 'Mis Inversiones', //important: active section name
                    prependTo: '#sidemenu'
                });

            }, 800); */
        }

        seleccionarSeccion(id: number): void {
            this.seccionId = id;
            this.seccionURI = 'app/mis-inversiones/' + this.templates[this.seccionId];
        }    

        setTemplates(): void {
            this.templates = [];
            this.templates[0] = "nacionales.html";
            //this.templates[1] = "fondos-privados.html"; NO APLICA (Link externo)
            this.templates[2] = "fondos-mutuos.html";
            this.templates[3] = "estado-documentos.html";
            this.templates[4] = "circularizacion.html";
        }

        getBalance(input: app.domain.IBalanceInput): void {
            this.dataService.postWebService(this.constantService.apiBalanceURI + 'getSingle', input)
                .then((result: app.domain.IBalance) => {
                    this.balance = result;
                });
        }
    }
    angular.module('tannerPrivadoApp')
        .controller('MisInversionesCtrl', MisInversionesCtrl);
}