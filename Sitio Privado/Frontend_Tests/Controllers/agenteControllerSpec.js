describe('agenteCtrl - ', function () {

    beforeEach(function () {
        module('tannerPrivadoApp');
    });

    var $q, $rootScope,
        constantService, dataService, authService, // dependecias controlador
        getSingle_deferred, postWebService_deferred, //defers
        agenteCtrl; //controlador
        
    var usuario_stub = {
        Autenticado: true,
        Nombres: '',
        Apellidos: '',
        Rut: '12345656-9',
        DireccionComercial: '',
        DireccionParticular: '',
        Ciudad: '',
        Pais: '',
        TelefonoComercial: '',
        TelefonoParticular: '',
        Email: '',
        CuentaCorriente: '',
        Banco: '',
        NombreCompleto: '',
        CiudadPais: ''
    };

    var agente_stub = {
        Codigo: 0284,
        Nombre: 'Agente de prueba',
        Sucursal: 'Sucursal de prueba',
        PathImg: '/path/de/prueba/',
        Email: 'adeprueba@tanner.cl',
        Telefono: '99000000',
        FechaInicioAcreditacion: '24 de noviembre 2015',
        FechaExpiracionAcreditacion: '24 de noviembre 2016',
        Descriptor: 'Este es un agente de prueba'
    };

    var agenteInput_stub = {
        _rut: '111333444-8',
        _sec: 0
    };

    beforeEach(inject(function (_$q_, _$rootScope_, _constantService_, _dataService_) {
        $q = _$q_;
        $rootScope = _$rootScope_;
        constantService = _constantService_;
        dataService = _dataService_;

        getSingle_deferred = $q.defer();
        postWebService_deferred = $q.defer();

        spyOn(dataService, 'getSingle').and.returnValue(getSingle_deferred.promise);
        spyOn(dataService, 'postWebService').and.returnValue(postWebService_deferred.promise);
                
    }));

    beforeEach(inject(function (_authService_) {
        getSingle_deferred.resolve(usuario_stub);
        $rootScope.$digest();
        authService = _authService_;
    }));

    beforeEach(inject(function (_$controller_) {
        agenteCtrl = _$controller_('AgenteCtrl', {
            $rootScope: $rootScope,
            constantService: constantService,
            dataService: dataService,
            authService: authService
        });
    }));

    it('Obtener datos agente.', function () {
        agenteCtrl.getAgente(agenteInput_stub);        
        postWebService_deferred.resolve(agente_stub);
        $rootScope.$digest();

        expect(agenteCtrl.agente).toBe(agente_stub);
    });
});