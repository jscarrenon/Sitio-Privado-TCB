describe('informacionFinancieraCtrl - ', function () {

    beforeEach( function() {
        module('tannerPrivadoApp');        
    });

    var $q, $rootScope,
        constantService, dataService, $routeParams, // dependecias controlador
        get_deferred, // defers
        informacionFinancieraCtrl; //controlador
    
    var container_stub = [
            {
                Name: 'Carpeta 1',
                Blobs: [{
                    Name: 'Nombre blob 1',
                    Url: 'Url/Blob/1'
                },
                    {
                        Name: 'Nombre blob 2',
                        Url: 'Url/Blob/2'
                    }],
                Folders: [{
                    Name: 'Carpeta X2',
                    Blobs: [{
                        Name: 'Nombre blob X2',
                        Url: 'Url/Blob/X2'
                    }],
                    Folders: null
                }]
            },
            {
                Name: 'Carpeta 1',
                Blobs: [{
                    Name: 'Nombre blob 213',
                    Url: 'Url/Blob/213'
                },
                    {
                        Name: 'Nombre blob 14',
                        Url: 'Url/Blob/14'
                    }],
                Folders: [{
                    Name: 'Carpeta AMVNA',
                    Blobs: [{
                        Name: 'Nombre blob 85NC',
                        Url: 'Url/Blob/85NC'
                    }],
                    Folders: [
                        {
                            Name: 'Carpeta 435',
                            Blobs:
                            [{
                                Name: 'Nombre blob 823C',
                                Url: 'Url/Blob/823C'
                            }],
                            Folders: [{
                                Name: 'Carpeta folder',
                                Blobs: [{
                                    Name: 'Nombre blob 1245',
                                    Url: 'Url/Blob/1245'
                                }],
                                Folders: null
                            }]
                        },
                        {
                            Name: 'Carpeta 918',
                            Blobs: [{
                                Name: 'Nombre blob B34',
                                Url: 'Url/Blob/B34'
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

        spyOn(dataService, 'get').and.returnValues(get_deferred.promise, get_deferred.promise);

        informacionFinancieraCtrl = _$controller_('InformacionFinancieraCtrl', {
            $rootScope: _$rootScope_,
            constantService: _constantService_,
            dataService: _dataService_,
            $routeParams: _$routeParams_
        });
    }));

    it('debería seleccionar sección 0 si la id es igual a 0.', function () {
        var id = 0;

        informacionFinancieraCtrl.seleccionarSeccion(id);
        get_deferred.resolve(container_stub);
        $rootScope.$digest();

        expect(informacionFinancieraCtrl.container).toEqual(container_stub);
        expect(informacionFinancieraCtrl.seccionId).toBe(id);
        expect(informacionFinancieraCtrl.selectedYearIndex).toBe(0);
        expect(informacionFinancieraCtrl.selectedYear).toEqual([]);
        expect(informacionFinancieraCtrl.seccionURI).toEqual('.build/html/modules/informacion-financiera/templates/politicas-procedimientos.html');        
    });

    it('debería seleccionar sección 1 si la id es igual a 1.', function () {
        var id = 1;

        informacionFinancieraCtrl.seleccionarSeccion(id);
        get_deferred.resolve(container_stub);
        $rootScope.$digest();

        expect(informacionFinancieraCtrl.container).toEqual(container_stub);
        expect(informacionFinancieraCtrl.seccionId).toBe(id);
        expect(informacionFinancieraCtrl.selectedYearIndex).toBe(0);
        expect(informacionFinancieraCtrl.selectedYear).toEqual(container_stub[0].Folders);
        expect(informacionFinancieraCtrl.seccionURI).toEqual('.build/html/modules/informacion-financiera/templates/estados-financieros.html');
    });

    it('debería seleccionar sección 2 si la id es igual a 2.', function () {
        var id = 2;

        informacionFinancieraCtrl.seleccionarSeccion(id);
        get_deferred.resolve(container_stub);
        $rootScope.$digest();

        expect(informacionFinancieraCtrl.container).toEqual(container_stub);
        expect(informacionFinancieraCtrl.seccionId).toBe(id);
        expect(informacionFinancieraCtrl.selectedYearIndex).toBe(0);
        expect(informacionFinancieraCtrl.selectedYear).toEqual([]);
        expect(informacionFinancieraCtrl.seccionURI).toEqual('.build/html/modules/informacion-financiera/templates/servicios-custodia.html');
    });

    it('debería seleccionar sección 3 si la id es igual a 3.', function () {
        var id = 3;

        informacionFinancieraCtrl.seleccionarSeccion(id);
        get_deferred.resolve(container_stub);
        $rootScope.$digest();

        expect(informacionFinancieraCtrl.container).toEqual(container_stub);
        expect(informacionFinancieraCtrl.seccionId).toBe(id);
        expect(informacionFinancieraCtrl.selectedYearIndex).toBe(0);
        expect(informacionFinancieraCtrl.selectedYear).toEqual([]);
        expect(informacionFinancieraCtrl.seccionURI).toEqual('.build/html/modules/informacion-financiera/templates/indices-liquidez.html');
    });

    it('debería seleccionar sección 4 si la id es igual a 4.', function () {
        var id = 4;

        informacionFinancieraCtrl.seleccionarSeccion(id);
        get_deferred.resolve(container_stub);
        $rootScope.$digest();

        expect(informacionFinancieraCtrl.container).toEqual(container_stub);
        expect(informacionFinancieraCtrl.seccionId).toBe(id);
        expect(informacionFinancieraCtrl.selectedYearIndex).toBe(0);
        expect(informacionFinancieraCtrl.selectedYear).toEqual([]);
        expect(informacionFinancieraCtrl.seccionURI).toEqual('.build/html/modules/informacion-financiera/templates/comite-regulacion.html');
    });

    it('debería seleccionar sección 5 si la id es igual a 5.', function () {
        var id = 5;

        informacionFinancieraCtrl.seleccionarSeccion(id);
        get_deferred.resolve(container_stub);
        $rootScope.$digest();

        expect(informacionFinancieraCtrl.container).toEqual(container_stub);
        expect(informacionFinancieraCtrl.seccionId).toBe(id);
        expect(informacionFinancieraCtrl.selectedYearIndex).toBe(0);
        expect(informacionFinancieraCtrl.selectedYear).toEqual([]);
        expect(informacionFinancieraCtrl.seccionURI).toEqual('.build/html/modules/informacion-financiera/templates/otros-documentos.html');
    });
        
    
    it('debería seleccionar año (indice = 0).', function () {
        var index = 0;

        informacionFinancieraCtrl.container = container_stub;
        informacionFinancieraCtrl.selectYear(index);

        expect(informacionFinancieraCtrl.selectedYearIndex).toBe(index);
        expect(informacionFinancieraCtrl.selectedYear).toBe(informacionFinancieraCtrl.container[index].Folders);
    });
    
    it('debería obtener contenedor de políticas y procedimientos si input es igual a "politicas-procedimientos".', function () {
        var input = 'politicas-procedimientos';

        informacionFinancieraCtrl.getContainer(input);
        get_deferred.resolve(container_stub);
        $rootScope.$digest();

        expect(informacionFinancieraCtrl.container).toBe(container_stub);
    });
    
    it('debería obtener contenedor de estados financieros si input es igual a "estados-financieros".', function () {
        var input = 'estados-financieros';

        informacionFinancieraCtrl.getContainer(input);
        get_deferred.resolve(container_stub);
        $rootScope.$digest();

        expect(informacionFinancieraCtrl.selectedYear).toEqual(container_stub[0].Folders);
        expect(informacionFinancieraCtrl.container).toBe(container_stub);
    });

    it('debería obtener contenedor de servicios de custodia si input es igual a "servicios-custodia".', function () {
        var input = 'servicios-custodia';

        informacionFinancieraCtrl.getContainer(input);
        get_deferred.resolve(container_stub);
        $rootScope.$digest();

        expect(informacionFinancieraCtrl.container).toBe(container_stub);
    });

    it('debería obtener contenedor de comité de regulación si input es igual a "comite-regulacion".', function () {
        var input = 'comite-regulacion';

        informacionFinancieraCtrl.getContainer(input);
        get_deferred.resolve(container_stub);
        $rootScope.$digest();

        expect(informacionFinancieraCtrl.container).toBe(container_stub);
    });

    it('debería obtener contenedor de otros documentos si input es igual a "otros-documentos".', function () {
        var input = 'otros-documentos';

        informacionFinancieraCtrl.getContainer(input);
        get_deferred.resolve(container_stub);
        $rootScope.$digest();

        expect(informacionFinancieraCtrl.container).toBe(container_stub);
    });
    
    it('debería setear las plantillas.', function () {
        informacionFinancieraCtrl.setTemplates();

        expect(informacionFinancieraCtrl.templates[0]).toBe('politicas-procedimientos.html');
        expect(informacionFinancieraCtrl.templates[1]).toBe('estados-financieros.html');
        expect(informacionFinancieraCtrl.templates[2]).toBe('servicios-custodia.html');
        expect(informacionFinancieraCtrl.templates[3]).toBe('indices-liquidez.html');
        expect(informacionFinancieraCtrl.templates[4]).toBe('comite-regulacion.html');
        expect(informacionFinancieraCtrl.templates[5]).toBe('otros-documentos.html');
    });

    it('debería setear nombres a contenedores.', function () {
        informacionFinancieraCtrl.setContainerNames();

        expect(informacionFinancieraCtrl.containerNames[0]).toBe('politicas-procedimientos');
        expect(informacionFinancieraCtrl.containerNames[1]).toBe('estados-financieros');
        expect(informacionFinancieraCtrl.containerNames[2]).toBe('servicios-custodia');
        expect(informacionFinancieraCtrl.containerNames[5]).toBe('otros-documentos');
    });
});