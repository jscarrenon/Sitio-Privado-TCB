describe('misInversionesFondosMutuosCtrl - ', function () {
        
    beforeEach(function () {
        module('tannerPrivadoApp');
    });

    var $rootScope, $routeParams,
        postWebService_deferred, getSingle_deferred, // deferred used for promises
        constantService, dataService, authService, extrasService, // dependencias controlador
        fondosMutuosCtrl; // controlador

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

    var fondoMutuoInput_stub = { rut_cli: 123456789 };

    var fondosMutuosRF = [{
        Descripcion: 'Fondo A1',
        Tipo: 'Tipo fondo A clase 1',
        CtaPisys: '235567',
        ValorCuota: 101000,
        SaldoCuota: 5004999,
        Csbis: 'S',
        Renta: 'RF',
        Pesos: 123945
    },
    {
        Descripcion: 'Fondo C5',
        Tipo: 'Tipo fondo C clase 5',
        CtaPisys: '235567',
        ValorCuota: 12345,
        SaldoCuota: 3468,
        Csbis: 'N',
        Renta: 'RF',
        Pesos: 34567
    }];

    var fondosMutuosRV = [{
        Descripcion: 'Fondo F4',
        Tipo: 'Tipo fondo F clase 4',
        CtaPisys: '235567',
        ValorCuota: 456788,
        SaldoCuota: 2346,
        Csbis: 'S',
        Renta: 'RV',
        Pesos: 785233
    }];

    var fondosMutuos_stub = {
        fondosMutuosRF: fondosMutuosRF, fondosMutuosRV: fondosMutuosRV
    };
            
    beforeEach(inject(function (_$rootScope_, _$q_, _constantService_, _dataService_, _extrasService_, _$routeParams_) {
        $q = _$q_
        constantService = _constantService_;
        dataService = _dataService_;
        $routeParams = _$routeParams_;
        $rootScope = _$rootScope_;
        extrasService = _extrasService_;

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
        fondosMutuosCtrl = _$controller_('MisInversionesFondosMutuosCtrl', {
            $rootScope: $rootScope,
            constantService: constantService,
            dataService: dataService,
            authService: authService,
            extrasService: extrasService,
            $routeParams: $routeParams
        });
    }));     
        
    it('Arreglos de fondos mutuos correctamente definidos y asignados.', function () {
        spyOn(fondosMutuosCtrl, 'getFondosMutuosTotal');

        fondosMutuosCtrl.getFondosMutuos(fondoMutuoInput_stub);
        postWebService_deferred.resolve(fondosMutuos_stub);
        $rootScope.$digest();

        expect(fondosMutuosCtrl.fondosMutuosRF).toBe(fondosMutuos_stub['fondosMutuosRF']);
        expect(fondosMutuosCtrl.fondosMutuosRV).toBe(fondosMutuos_stub['fondosMutuosRV']);
        expect(fondosMutuosCtrl.getFondosMutuosTotal).toHaveBeenCalled();
    });

    it('Totales de fondos mutuos sumados correctamente.', function () {
        fondosMutuosCtrl.fondosMutuosRF = fondosMutuos_stub['fondosMutuosRF'];
        fondosMutuosCtrl.fondosMutuosRV = fondosMutuos_stub['fondosMutuosRV'];
        fondosMutuosCtrl.getFondosMutuosTotal();

        expect(fondosMutuosCtrl.fondosMutuosRFTotal).toBe(123945 + 34567);
        expect(fondosMutuosCtrl.fondosMutuosRVTotal).toBe(785233);
    });
});