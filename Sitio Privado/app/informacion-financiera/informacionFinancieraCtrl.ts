module app.informacionFinanciera {

    interface IInformacionFinancieraViewModel {
        templates: string[];
        seccionURI: string;
        seccionId: number;
        seleccionarSeccion(id: number): void;
        setTemplates(): void;
        getFolders(): app.domain.AzureContainer[];
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
        }

        getFolders(): app.domain.AzureContainer[] {
        //TODO: call service
            var container = [];
            var blobs = [];
            blobs[0] = new app.domain.AzureBlob("nuevo archivo de prueba con nombre largo", null);
            blobs[1] = new app.domain.AzureBlob("otro archivo", null);
            blobs[2] = new app.domain.AzureBlob("tercero", null);
            container[0] = new app.domain.AzureContainer("Hola", blobs);
            container[1] = new app.domain.AzureContainer("Chao", null);
            
            return container;
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