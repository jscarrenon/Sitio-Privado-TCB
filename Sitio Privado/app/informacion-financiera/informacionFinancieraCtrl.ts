module app.informacionFinanciera {

    interface IInformacionFinancieraViewModel {
        templates: string[];
        seccionURI: string;
        seccionId: number;
        seleccionarSeccion(id: number): void;
        setTemplates(): void;
        setContainerNames(): void;
        getContainer(input: string): void;
    }

    export class InformacionFinancieraCtrl implements IInformacionFinancieraViewModel {

        templates: string[];
        seccionURI: string;
        seccionId: number;
        containerNames: { [id: number]: string };

        container: app.domain.AzureFolder[];

        static $inject = ['constantService', 'dataService'];
        constructor(private constantService: app.common.services.ConstantService,
            private dataService: app.common.services.DataService) {

            this.setTemplates();
            this.setContainerNames();
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
            //this.container = [];
            this.seccionId = id;
            this.seccionURI = 'app/informacion-financiera/' + this.templates[this.seccionId];
            this.getContainer(this.containerNames[id]);
        }

        getContainer(input: string): void {
            this.dataService.get(this.constantService.apiBlobsURI + 'getContainer?name=' + input)
                .then((result: app.domain.AzureFolder[]) => { this.container = result;});
        }

        setTemplates(): void {
            this.templates = [];
            this.templates[0] = "estatutos.html";
            this.templates[1] = "estatutos.html";
            this.templates[2] = "custodia.html";
            this.templates[3] = "estatutos.html";
            this.templates[4] = "comite-regulacion.html";
            this.templates[5] = "otros-documentos.html";
        }

        setContainerNames(): void {
            this.containerNames = {};
            this.containerNames[0] = 'estatutos';
            this.containerNames[1] = 'documentos-normativos';
            this.containerNames[2] = 'servicios-custodia';
            this.containerNames[4] = 'comite-regulacion';
            this.containerNames[5] = 'otros-documentos';
        }
    }
    angular.module('tannerPrivadoApp')
        .controller('InformacionFinancieraCtrl', InformacionFinancieraCtrl);
}