describe("[FrontEnd] informacionFinancieraCrtl Unit Tests.", function() {
    beforeEach( function() {
        module("tannerPrivadoApp");        
    });
    var $q, $rootScope, get_deferred, informacionFinancieraController, constantService, dataService, $routeParams;
    var container_stub;
    container_stub = [
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

    beforeEach(inject(function (_$controller_, _$rootScope_, _$q_, _constantService_, _dataService_, _$routeParams_) {
        
        $q = _$q_;
        $rootScope = _$rootScope_;
        constantService = _constantService_;
        dataService = _dataService_;
        $routeParams = _$routeParams_;

        get_deferred = $q.defer();

        spyOn(constantService, 'apiBlobsURI').and.returnValue('/api/containers/');
        spyOn(dataService, 'get').and.returnValues(get_deferred.promise, get_deferred.promise);

        informacionFinancieraController = _$controller_("InformacionFinancieraCtrl", {
            $rootScope: _$rootScope_,
            constantService: _constantService_,
            dataService: _dataService_,
            $routeParams: _$routeParams_
        });

        get_deferred.resolve(container_stub);
        $rootScope.$digest();
    }));

    it("Seleccionar sección 0.", function () {

        get_deferred = $q.defer();
        var id = 0;
        informacionFinancieraController.seleccionarSeccion(id);
        get_deferred.resolve(container_stub);
        $rootScope.$digest();

        expect(informacionFinancieraController.container).toEqual(container_stub);
        expect(informacionFinancieraController.seccionId).toBe(id);
        expect(informacionFinancieraController.selectedYearIndex).toBe(0);
        expect(informacionFinancieraController.selectedYear).toEqual([]);
        expect(informacionFinancieraController.seccionURI).toBe("app/informacion-financiera/estatutos.html");
        
    });

    it("Seleccionar sección 1.", function () {

        get_deferred = $q.defer();
        var id = 1;
        informacionFinancieraController.seleccionarSeccion(id);
        get_deferred.resolve(container_stub);
        $rootScope.$digest();

        expect(informacionFinancieraController.container).toEqual(container_stub);
        expect(informacionFinancieraController.seccionId).toBe(id);
        expect(informacionFinancieraController.selectedYearIndex).toBe(0);
        expect(informacionFinancieraController.selectedYear).toEqual(container_stub[0].Folders);
        expect(informacionFinancieraController.seccionURI).toBe("app/informacion-financiera/documentos-normativos.html");

    });

    it("Seleccionar sección 2.", function () {

        get_deferred = $q.defer();
        var id = 2;
        informacionFinancieraController.seleccionarSeccion(id);
        get_deferred.resolve(container_stub);
        $rootScope.$digest();

        expect(informacionFinancieraController.container).toEqual(container_stub);
        expect(informacionFinancieraController.seccionId).toBe(id);
        expect(informacionFinancieraController.selectedYearIndex).toBe(0);
        expect(informacionFinancieraController.selectedYear).toEqual([]);
        expect(informacionFinancieraController.seccionURI).toBe("app/informacion-financiera/servicios-custodia.html");

    });

    it("Seleccionar sección 3.", function () {
        get_deferred = $q.defer();
        var id = 3;
        informacionFinancieraController.seleccionarSeccion(id);
        get_deferred.resolve(container_stub);
        $rootScope.$digest();

        expect(informacionFinancieraController.container).toEqual([]);
        expect(informacionFinancieraController.seccionId).toBe(id);
        expect(informacionFinancieraController.selectedYearIndex).toBe(0);
        expect(informacionFinancieraController.selectedYear).toEqual([]);
        expect(informacionFinancieraController.seccionURI).toBe("app/informacion-financiera/indices-liquidez.html");

    });

    it("Seleccionar sección 4.", function () {

        get_deferred = $q.defer();
        var id = 4;
         informacionFinancieraController.seleccionarSeccion(id);
        get_deferred.resolve(container_stub);
        $rootScope.$digest();

        expect(informacionFinancieraController.container).toEqual([]);
        expect(informacionFinancieraController.seccionId).toBe(id);
        expect(informacionFinancieraController.selectedYearIndex).toBe(0);
        expect(informacionFinancieraController.selectedYear).toEqual([]);
        expect(informacionFinancieraController.seccionURI).toBe("app/informacion-financiera/comite-regulacion.html");

    });

    it("Seleccionar sección 5.", function () {

        get_deferred = $q.defer();
        var id = 5;
        informacionFinancieraController.seleccionarSeccion(id);
        get_deferred.resolve(container_stub);
        $rootScope.$digest();

        expect(informacionFinancieraController.container).toEqual(container_stub);
        expect(informacionFinancieraController.seccionId).toBe(id);
        expect(informacionFinancieraController.selectedYearIndex).toBe(0);
        expect(informacionFinancieraController.selectedYear).toEqual([]);
        expect(informacionFinancieraController.seccionURI).toBe("app/informacion-financiera/otros-documentos.html");

    });
        
    
    it("selectYear(index: number)", function () {

        var index = 0;

        informacionFinancieraController.selectYear(index);
        expect(informacionFinancieraController.selectedYearIndex).toBe(index);
        expect(informacionFinancieraController.selectedYear).toBe(informacionFinancieraController.container[index].Folders);
    });
    
    it("Obtener contenedor de estatutos", function () {
        var input = "estatutos";
        get_deferred = $q.defer();

        informacionFinancieraController.getContainer(input);

        get_deferred.resolve(container_stub);
        $rootScope.$digest();

        expect(informacionFinancieraController.container).toBe(container_stub);
    });
    
    it("Obtener contenedor de documentos normativos", function () {
        var input = "documentos-normativos";
        get_deferred = $q.defer();

        informacionFinancieraController.getContainer(input);

        get_deferred.resolve(container_stub);
        $rootScope.$digest();

        expect(informacionFinancieraController.selectedYear).toEqual(container_stub[0].Folders);
        expect(informacionFinancieraController.container).toBe(container_stub);
    });

    it("Obtener contenedor de servicios de custodia", function () {

        var input = "servicios-custodia";
        get_deferred = $q.defer();

        informacionFinancieraController.getContainer(input);

        get_deferred.resolve(container_stub);
        $rootScope.$digest();

        expect(informacionFinancieraController.container).toBe(container_stub);

    });

    it("Obtener contenedor de comité de regulación", function () {
        var input = "comite-regulacion";
        get_deferred = $q.defer();

        informacionFinancieraController.getContainer(input);

        get_deferred.resolve(container_stub);
        $rootScope.$digest();

        expect(informacionFinancieraController.container).toBe(container_stub);
    });

    it("Obtener contenedor de otros documentos", function () {
        var input = "otros-documentos";
        get_deferred = $q.defer();

        informacionFinancieraController.getContainer(input);

        get_deferred.resolve(container_stub);
        $rootScope.$digest();

        expect(informacionFinancieraController.container).toBe(container_stub);
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
        expect(informacionFinancieraController.containerNames[5]).toBe("otros-documentos");

    });
});