module app.misInversiones {

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

        static $inject = ['constantService', 'dataService'];
        constructor(private constantService: app.common.services.ConstantService,
            private dataService: app.common.services.DataService) {

            this.setTemplates();
            this.seccionId = 0;
            this.seleccionarSeccion(this.seccionId);

            //IMPORTANTE: aquí se debe usar librería js "slicknav", pero primero debe definirse el declaration file (.d.ts)
            /*$(function () {
                $('#menu2').slicknav({
                    label: 'Mis Inversiones', //important: active section name
                    prependTo: '#sidemenu'
                });
            });*/
        }

        seleccionarSeccion(id: number): void {
            this.seccionId = id;
            this.seccionURI = 'app/mis-inversiones/' + this.templates[this.seccionId];
        }    

        setTemplates(): void {
            this.templates = [];
            this.templates[0] = "nacionales.html";
            this.templates[1] = "fondos-privados.html";
            this.templates[2] = "fondos-mutuos.html";
            this.templates[3] = "estado-documentos.html";
            this.templates[4] = "circularizacion.html";
        }
    }
    angular.module('tannerPrivadoApp')
        .controller('MisInversionesCtrl', MisInversionesCtrl);
}