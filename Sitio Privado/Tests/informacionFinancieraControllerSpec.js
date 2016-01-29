describe("[FrontEnd] informacionFinancieraCrtl Unit Tests.", function() {
    beforeEach( function() {
        module("tannerPrivadoApp");
    });

    describe("", function () {
        var $q, _defer, informacionFinancieraCtrl, constantService, dataService, $routeParams;

        beforeEach(inject(function (_$controller_, _$rootScope_, _$q_, _constantService_, _dataService_, _$routeParams_) {
            $q = _$q_;
            constantService = _constantService_;
            dataService = _dataService_;
            $routeParams = _$routeParams_;

            informacionFinancieraCtrl = _$controller_("InformacionFinancieraCtrl", {
                $rootScope: _$rootScope_,
                constantService: _constantService_,
                dataService: _dataService_,
                $routeParams: $routeParams
            });
        }));

        it("seleccionarSeccion(id: number)", function(){
            /*this.container = [];
            this.seccionId = id;
            this.selectedYearIndex = 0;
            this.selectedYear = [];
            this.seccionURI = 'app/informacion-financiera/' + this.templates[this.seccionId];
            this.getContainer(this.containerNames[id]);*/
        });


        it("selectYear(index: number)", function () {
            //this.selectedYearIndex = index;
            //this.selectedYear = this.container[index].Folders;
        });

        it("getContainer(input: string)", function () {
            /*this.dataService.get(this.constantService.apiBlobsURI + 'getContainer?name=' + input)
                .then((result: app.domain.AzureFolder[]) => {
                    if (input == 'documentos-normativos') {
                        result.sort((a, b) => this.sortYears(a,b));
                        this.selectedYear = result[0].Folders;
                    }
                    this.container = result;
                });*/
        });

        it("sortYears(a: app.domain.AzureFolder, b: app.domain.AzureFolder)", function () {
            //var re = /\D/g;
            //return -(parseInt(a.Name.replace(re, ""), 10) - parseInt(b.Name.replace(re, ""), 10));
        });

        it("setTemplates()", function () {
            /*this.templates = [];
            this.templates[0] = "estatutos.html";
            this.templates[1] = "documentos-normativos.html";
            this.templates[2] = "servicios-custodia.html";
            this.templates[3] = "indices-liquidez.html";
            this.templates[4] = "comite-regulacion.html";
            this.templates[5] = "otros-documentos.html";*/
        });

        it("setContainerNames()", function () {
            /*this.containerNames = {};
             this.containerNames[0] = 'estatutos';
             this.containerNames[1] = 'documentos-normativos';
             this.containerNames[2] = 'servicios-custodia';
             this.containerNames[4] = 'comite-regulacion';
             this.containerNames[5] = 'otros-documentos';*/
        });
    });
});