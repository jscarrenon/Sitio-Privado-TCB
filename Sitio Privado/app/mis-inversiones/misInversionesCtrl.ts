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
    }

    export class MisInversionesCtrl implements IMisInversionesViewModel {

        templates: string[];
        seccionURI: string;
        seccionId: number;

        static $inject = ['constantService', 'dataService', 'authService', 'extrasService', '$routeParams'];
        constructor(protected constantService: app.common.services.ConstantService,
            protected dataService: app.common.services.DataService,
            protected authService: app.common.services.AuthService,
            protected extrasService: app.common.services.ExtrasService,
            protected $routeParams: IMisInversionesRouteParams) {

            this.setTemplates();
            this.seccionId = 0;

            if (this.$routeParams.seccion) {
                if (this.$routeParams.seccion == 'estado-documentos') {
                    this.seccionId = 3;
                }
            }            

            this.seleccionarSeccion(this.seccionId);

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
    }
    angular.module('tannerPrivadoApp')
        .controller('MisInversionesCtrl', MisInversionesCtrl);
}