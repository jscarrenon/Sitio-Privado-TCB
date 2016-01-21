﻿module app.informacionFinanciera {

    interface IInformacionFinancieraViewModel {
        templates: string[];
        seccionURI: string;
        seccionId: number;
        seleccionarSeccion(id: number): void;
        setTemplates(): void;
        getContainer(input: string): void;
    }

    export class InformacionFinancieraCtrl implements IInformacionFinancieraViewModel {

        templates: string[];
        seccionURI: string;
        seccionId: number;

        container: app.domain.AzureFolder[];

        static $inject = ['constantService', 'dataService'];
        constructor(private constantService: app.common.services.ConstantService,
            private dataService: app.common.services.DataService) {

            this.setTemplates();
            this.seccionId = 0;
            this.seleccionarSeccion(this.seccionId);

            this.container = [];

            /*var blobs = [];
            blobs.push(new app.domain.AzureBlob("nuevo archivo de prueba con nombre largo", ""));
            blobs.push(new app.domain.AzureBlob("otro archivo", ""));
            blobs.push(new app.domain.AzureBlob("tercero", ""));
            
            this.container.push(new app.domain.AzureContainer("Hola", blobs));
            this.container.push(new app.domain.AzureContainer("Chao", null));*/

            //Timeout por error de script slicknav (a.mobileNav.on)
            /*setTimeout(function () {
                (<any>$('#menu2')).slicknav({
                    label: 'Información Financiera', //important: active section name
                    prependTo: '#sidemenu'
                });
            }, 800);*/
        }

        seleccionarSeccion(id: number): void {
            this.container = [];
            this.seccionId = id;
            this.seccionURI = 'app/informacion-financiera/' + this.templates[this.seccionId];
            //Change dictionary
            if (this.seccionId == 0) {
                this.getContainer('estatutos');
            }
            else if (this.seccionId == 5) {
                this.getContainer('otros');
            }

        }

        getContainer(input: string): void {
            this.dataService.get(this.constantService.apiBlobsURI + 'getContainer?name=' + input)
                .then((result: app.domain.AzureFolder[]) => { this.container = result;});
        }

        setTemplates(): void {
            this.templates = [];
            this.templates[0] = "estatutos.html";
            this.templates[1] = "estatutos.html";
            this.templates[2] = "estatutos.html";
            this.templates[3] = "estatutos.html";
            this.templates[4] = "estatutos.html";
            this.templates[5] = "otros-documentos.html";
        }
    }
    angular.module('tannerPrivadoApp')
        .controller('InformacionFinancieraCtrl', InformacionFinancieraCtrl);
}