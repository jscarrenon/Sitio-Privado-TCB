describe("[FrontEnd] informacionFinancieraCrtl Unit Tests.", function() {
    beforeEach( function() {
        module("tannerPrivadoApp");
    });

    describe("", function () {
        var $q, get_deferred, informacionFinancieraController, constantService, dataService, $routeParams;

        beforeEach(inject(function (_$controller_, _$rootScope_, _$q_, _constantService_, _dataService_, _$routeParams_) {
            $q = _$q_;
            constantService = _constantService_;
            dataService = _dataService_;
            $routeParams = _$routeParams_;

            get_deferred = $q.defer;

            spyOn(constantService, 'apiBlobsURI').and.returnValue('');            

            informacionFinancieraController = _$controller_("InformacionFinancieraCtrl", {
                $rootScope: _$rootScope_,
                constantService: _constantService_,
                dataService: _dataService_,
                $routeParams: _$routeParams_
            });
        }));

        beforeEach(function () {
            informacionFinancieraController.setTemplates();
            informacionFinancieraController.setContainerNames();
            informacionFinancieraController.seccionId = 0;
            spyOn(informacionFinancieraController, 'sortYears');
        });

        it("Seleccionar sección.", function () {

            var objetoVacio = [];
            
            for (id = 0; id < informacionFinancieraController.templates.length; id++) {
                informacionFinancieraController.seleccionarSeccion(id);
                expect(informacionFinancieraController.container).toEqual(objetoVacio);
                expect(informacionFinancieraController.seccionId).toBe(id);
                expect(informacionFinancieraController.selectedYearIndex).toBe(0);
                expect(informacionFinancieraController.selectedYear).toEqual(objetoVacio);
                expect(informacionFinancieraController.seccionURI).toBe("app/informacion-financiera/" + informacionFinancieraController.templates[id]);
            }
        });

        it("selectYear(index: number)", function () {

            for (index = 0; index < informacionFinancieraController.container.length; index++){
                informacionFinancieraController.selectYear(index);
                expect(informacionFinancieraController.selectedYearIndex).toBe(index);
                expect(informacionFinancieraController.selectedYear).toBe(informacionFinancieraController.container[index].Folders);
            }            

        });

        it("getContainer(input: string)", function () {
            var container_stub = [
                {
                    Name: "Carpeta 1",
                    Blobs: [{
                            Name: "Nombre blob 1",
                            Url: "Url/Blob/1"
                        },
                        {
                            Name: "Nombre blob 2",
                            Url: "Url/Blob/2"
                        }],
                    Folders: [{
                        Name: "Carpeta X2",
                        Blobs: [{
                            Name: "Nombre blob X2",
                            Url: "Url/Blob/X2"
                        }],
                        Folders: null
                        }]
                },
                {
                    Name: "Carpeta 1",
                    Blobs: [{
                        Name: "Nombre blob 213",
                        Url: "Url/Blob/213"
                    },
                        {
                            Name: "Nombre blob 14",
                            Url: "Url/Blob/14"
                        }],
                    Folders: [{
                        Name: "Carpeta AMVNA",
                        Blobs: [{
                            Name: "Nombre blob 85NC",
                            Url: "Url/Blob/85NC"
                        }],
                        Folders: [
                            {
                                Name: "Carpeta 435",
                                Blobs: 
                                [{
                                    Name: "Nombre blob 823C",
                                    Url: "Url/Blob/823C"
                                }],
                                Folders: [{
                                Name: "Carpeta folder",
                                Blobs: [{
                                    Name: "Nombre blob 1245",
                                    Url: "Url/Blob/1245"
                                }],
                                Folders: null
                            }]
                            },
                            {
                                Name: "Carpeta 918",
                                Blobs: [{
                                    Name: "Nombre blob B34",
                                    Url: "Url/Blob/B34"
                                }],
                                Folders: null
                            }
                        ]
                    }]
                }
            ];

            spyOn(dataService, 'get').and.returnValue(get_deferred.promise);

            for (input = 0; input < informacionFinancieraController.containerNames.length; input++) {
                informacionFinancieraController.getContainer(input);
                get_deferred.resolve(container_stub);
                $rootScope.$digest();

                if (input == 'documentos-normativos') {
                    expect(informacionFinancieraController.selectedYear).toBe(container_stub[0].Folders);
                }
                expect(informacionFinancieraController.container).toBe(container_stub);
            }
        });

        it("setTemplates()", function () {

            informacionFinancieraController.setTemplates();

            expect(informacionFinancieraController.templates[0]).toBe("estatutos.html");
            expect(informacionFinancieraController.templates[1]).toBe("documentos-normativos.html");
            expect(informacionFinancieraController.templates[2]).toBe("servicios-custodia.html");
            expect(informacionFinancieraController.templates[3]).toBe("indices-liquidez.html");
            expect(informacionFinancieraController.templates[4]).toBe("comite-regulacion.html");
            expect(informacionFinancieraController.templates[5]).toBe("otros-documentos.html");

        });

        it("setContainerNames()", function () {
            
            informacionFinancieraController.setContainerNames();

            expect(informacionFinancieraController.containerNames[0]).toBe("estatutos");
            expect(informacionFinancieraController.containerNames[1]).toBe("documentos-normativos");
            expect(informacionFinancieraController.containerNames[2]).toBe("servicios-custodia");
            expect(informacionFinancieraController.containerNames[4]).toBe("comite-regulacion");
            expect(informacionFinancieraController.containerNames[5]).toBe("otros-documentos");

        });
    });
});