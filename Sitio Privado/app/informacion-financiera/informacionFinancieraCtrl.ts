module App.informacionFinanciera {

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

        container: App.Domain.AzureFolder[];

        static $inject = ['constantService', 'dataService'];
        constructor(private constantService: App.Common.Services.ConstantService,
            private dataService: App.Common.Services.DataService) {

            this.setTemplates();
            this.seccionId = 0;
            this.seleccionarSeccion(this.seccionId);

            this.container = [];

            /*var blobs = [];
            blobs.push(new App.Domain.AzureBlob("nuevo archivo de prueba con nombre largo", ""));
            blobs.push(new App.Domain.AzureBlob("otro archivo", ""));
            blobs.push(new App.Domain.AzureBlob("tercero", ""));
            
            this.container.push(new App.Domain.AzureContainer("Hola", blobs));
            this.container.push(new App.Domain.AzureContainer("Chao", null));*/

            //Timeout por error de script slicknav (a.mobileNav.on)
            /*setTimeout(function () {
                (<any>$('#menu2')).slicknav({
                    label: 'Información Financiera', //important: active section name
                    prependTo: '#sidemenu'
                });
            }, 800);*/
        }

        seleccionarSeccion(id: number): void {
            this.seccionId = id;
            this.seccionURI = 'app/informacion-financiera/' + this.templates[this.seccionId];
            //Change dictionary
            if (this.seccionId == 0) {
                this.getContainer('estatutos');
            }
            else {
                this.container = [];
            }
        }

        getContainer(input: string): void {
            this.dataService.get(this.constantService.apiBlobsURI + 'getContainer?name=' + input)
                .then((result: App.Domain.AzureFolder[]) => { this.container = result; });
        }

        setTemplates(): void {
            this.templates = [];
            this.templates[0] = "estatutos.html";
            this.templates[1] = "estatutos.html";
            this.templates[2] = "estatutos.html";
            this.templates[3] = "estatutos.html";
            this.templates[4] = "estatutos.html";
            this.templates[5] = "estatutos.html";
        }
    }
    angular.module('tannerPrivadoApp')
        .controller('InformacionFinancieraCtrl', InformacionFinancieraCtrl);
}