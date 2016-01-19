﻿module app.informacionFinanciera {

    interface IInformacionFinancieraViewModel {
        templates: string[];
        seccionURI: string;
        seccionId: number;
        seleccionarSeccion(id: number): void;
        setTemplates(): void;
    }

    export class InformacionFinancieraCtrl implements IInformacionFinancieraViewModel {

        templates: string[];
        seccionURI: string;
        seccionId: number;

        static $inject = ['constantService', 'dataService'];
        constructor(private constantService: app.common.services.ConstantService,
            private dataService: app.common.services.DataService) {

            this.setTemplates();
            this.seccionId = 0;
            this.seleccionarSeccion(this.seccionId);

            //Timeout por error de script slicknav (a.mobileNav.on)
            setTimeout(function () {
                (<any>$('#menu2')).slicknav({
                    label: 'Informacion Financiera', //important: active section name
                    prependTo: '#sidemenu'
                });
            }, 800);
        }

        seleccionarSeccion(id: number): void {
            this.seccionId = id;
            this.seccionURI = 'app/informacion-financiera/' + this.templates[this.seccionId];
        }

        setTemplates(): void {
            this.templates = [];
            this.templates[0] = "nacionales.html";
            this.templates[1] = "fondos-privados.html";
            this.templates[2] = "fondos-mutuos.html";
            this.templates[3] = "estado-documentos.html";
            this.templates[4] = "circularizacion.html";
            this.templates[5] = "circularizacion.html";
        }
    }
    angular.module('tannerPrivadoApp')
        .controller('InformacionFinancieraCtrl', InformacionFinancieraCtrl);
}