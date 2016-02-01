module app.informacionFinanciera {

    export interface IInformacionFinancieraRouteParams extends ng.route.IRouteParamsService {
        seccion?: string;
    }

    interface IInformacionFinancieraViewModel extends app.common.interfaces.ISeccion {
        setContainerNames(): void;
        getContainer(input: string): void;
    }

    export class InformacionFinancieraCtrl implements IInformacionFinancieraViewModel {

        templates: string[];
        seccionURI: string;
        seccionId: number;
        containerNames: { [id: number]: string };

        container: app.domain.AzureFolder[];
        selectedYear: app.domain.AzureFolder[];
        selectedYearIndex: number;

        static $inject = ['constantService', 'dataService', '$routeParams'];
        constructor(private constantService: app.common.services.ConstantService,
            private dataService: app.common.services.DataService,
            private $routeParams: IInformacionFinancieraRouteParams) {

            this.setTemplates();
            this.setContainerNames();
            this.seccionId = 0;

            if (this.$routeParams.seccion) {
                if (this.$routeParams.seccion == 'estatutos') {
                    this.seccionId = 0;
                }
                else if (this.$routeParams.seccion == 'documentos-normativos') {
                    this.seccionId = 1;
                }
                else if (this.$routeParams.seccion == 'servicios-custodia') {
                    this.seccionId = 2;
                }
                else if (this.$routeParams.seccion == 'indices-liquidez') {
                    this.seccionId = 3;
                }
                else if (this.$routeParams.seccion == 'comite-regulacion') {
                    this.seccionId = 4;
                }
                else if (this.$routeParams.seccion == 'otros-documentos') {
                    this.seccionId = 5;
                }
            }  

            this.seleccionarSeccion(this.seccionId);

            //Timeout por error de script slicknav
            setTimeout(function () {
                (<any>$('#menu2')).slicknav({
                    label: 'Información Financiera',
                    prependTo: '#sidemenu'
                });

            }, 100);
        }

        seleccionarSeccion(id: number): void {
            this.container = [];
            this.seccionId = id;
            this.selectedYearIndex = 0;
            this.selectedYear = [];
            this.seccionURI = 'app/informacion-financiera/' + this.templates[this.seccionId];
            if (id != 3) {
                this.getContainer(this.containerNames[id]);
            }
        }

        selectYear(index: number): void {
            this.selectedYearIndex = index;
            this.selectedYear = this.container[index].Folders;
        }

        getContainer(input: string): void {
            this.dataService.get(this.constantService.apiBlobsURI + 'getContainer?name=' + input)
                .then((result: app.domain.AzureFolder[]) => {
                    if (input == 'documentos-normativos') {
                        result.sort((a, b) => this.sortYears(a,b));
                        this.selectedYear = result[0].Folders;
                    }
                    this.container = result;
                });
        }

        sortYears(a: app.domain.AzureFolder, b: app.domain.AzureFolder): number {
            var re = /\D/g;
            return (parseInt(b.Name.replace(re, ""), 10) - parseInt(a.Name.replace(re, ""), 10));
        }

        setTemplates(): void {
            this.templates = [];
            this.templates[0] = "estatutos.html";
            this.templates[1] = "documentos-normativos.html";
            this.templates[2] = "servicios-custodia.html";
            this.templates[3] = "indices-liquidez.html";
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