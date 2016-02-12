describe('authService - ', function () {

    beforeEach(function () {
        module('tannerPrivadoApp');
    });

    var $q, $rootScope,
        postWebservice_deferred, // deferred used for promises
        authService, //service
        getSingle_deferred, postWebService_deferred,
        constantService, dataService, extrasService, // service dependencies        
        usuario_stub,
        modalInstance;

    beforeEach(inject(function (_$rootScope_, _$q_, _constantService_, _dataService_, _extrasService_) {

        $q = _$q_;
        $rootScope = _$rootScope_;
        constantService = _constantService_;
        dataService = _dataService_;
        extrasService = _extrasService_;
        
        getSingle_deferred = $q.defer();
        postWebService_deferred = $q.defer();

        spyOn(dataService, 'getSingle').and.returnValue(getSingle_deferred.promise);
        spyOn(dataService, 'postWebService').and.returnValue(postWebService_deferred.promise);
        
        usuario_stub = {
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
    }));

    beforeEach(inject(function (_authService_) {        
        getSingle_deferred.resolve(usuario_stub);
        $rootScope.$digest();
        authService = _authService_;
    }));

    it('Obtener usuario actual. Usuario autenticado.', function () {
        authService.getUsuarioActual();
        getSingle_deferred.resolve(usuario_stub);
        $rootScope.$digest();

        expect(authService.usuario).toEqual(usuario_stub);
        expect(authService.autenticado).toEqual(usuario_stub.Autenticado);
        expect(authService.autenticado).toEqual(usuario_stub.Autenticado);
    });

    it('Obtener usuario actual. Usuario no autenticado.', function () {
        usuario_stub.Autenticado = false;
        authService.getUsuarioActual();
        getSingle_deferred.resolve(usuario_stub);
        $rootScope.$digest();

        expect(authService.usuario).toEqual(usuario_stub);
        expect(authService.autenticado).toEqual(usuario_stub.Autenticado);
        expect(authService.circularizacionPendiente).toEqual(false);
    });

    it('Cerrar sesión.', function () {
        spyOn(extrasService, 'abrirRuta').and.callFake(function () {
            return true;
        });

        authService.cerrarSesion();
        $rootScope.$digest();

        expect(authService.autenticado).toBe(false);
        expect(authService.circularizacionPendiente).toBe(false);
        expect(authService.documentosPendientes).toBe(0);
        expect(authService.usuario).toBe(null);
        expect(extrasService.abrirRuta).toHaveBeenCalled();
    });

    it('Obtener circularización pendiente.', function () {
        authService.getCircularizacionPendiente();
        postWebService_deferred.resolve({ Resultado: true });
        $rootScope.$digest();

        expect(authService.circularizacionPendiente).toBe(true);
    });

    it('Obtener circularización pendiente. (ninguna pendiente)', function () {
        authService.getCircularizacionPendiente();
        postWebService_deferred.resolve({ Resultado: false });
        $rootScope.$digest();

        expect(authService.circularizacionPendiente).toBe(false);
    });

    it('Obtener documentos pendientes.', function () {
        authService.getDocumentosPendientes();
        postWebService_deferred.resolve({ Resultado: 1 });
        $rootScope.$digest();

        expect(authService.documentosPendientes).toBeGreaterThan(0);
    });

    it('Obtener documentos pendientes. (sin documentos pendientes)', function () {
        authService.getDocumentosPendientes();
        postWebService_deferred.resolve({ Resultado: 0 });
        $rootScope.$digest();

        expect(authService.documentosPendientes).toBe(0);
    })
});