describe('misInversionesCircularizacionCtrl - ', function () {

    beforeEach( function() {
        module('tannerPrivadoApp');
    });

    var $q, $rootScope,
        constantService, dataService, authService, extrasService, // dependecias controlador
        getSingle_deferred, postWebService_deferred, // defers
        misInversionesCircularizacionCtrl; // controlador

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

    beforeEach(inject(function (_$q_, _$rootScope_, _constantService_, _dataService_, _extrasService_) {
        $q = _$q_;
        $rootScope = _$rootScope_;
        constantService = _constantService_;
        dataService = _dataService_;            
        extrasService = _extrasService_;

        getSingle_deferred = $q.defer();
        postWebService_deferred = $q.defer();

        spyOn(extrasService, 'getRutParteEntera');
        spyOn(extrasService, 'getFechaFormato').and.returnValue('24/02/2016');        
        spyOn(dataService, 'getSingle').and.returnValue(getSingle_deferred.promise);
        spyOn(dataService, 'postWebService').and.returnValue(postWebService_deferred.promise);        
    }));

    beforeEach(inject(function (_authService_) {
        getSingle_deferred.resolve(usuario_stub);
        $rootScope.$digest();
        authService = _authService_;
    }));

    beforeEach(inject(function (_$controller_) {
        misInversionesCircularizacionCtrl = _$controller_('MisInversionesCircularizacionCtrl', {
                $rootScope: $rootScope,
                constantService: constantService,
                dataService: dataService,
                authService: authService,
                extrasService: extrasService
            });
    }));

    it('Seleccionar sección 0.', function () {
        var id = 0;

        misInversionesCircularizacionCtrl.seleccionarSeccion(id);

        expect(misInversionesCircularizacionCtrl.seccionId).toBe(id);
        expect(misInversionesCircularizacionCtrl.seccionURI).toBe('app/mis-inversiones/circularizacion_pendiente.html');
    });

    it('Seleccionar sección 1.', function () {
        var id = 1;
        var getArchivoSpy = spyOn(misInversionesCircularizacionCtrl, 'getArchivo');

        misInversionesCircularizacionCtrl.seleccionarSeccion(id);

        expect(misInversionesCircularizacionCtrl.seccionId).toBe(id);
        expect(getArchivoSpy).toHaveBeenCalled();
        expect(misInversionesCircularizacionCtrl.seccionURI).toBe('app/mis-inversiones/circularizacion_anual.html');
    });

    it('Seleccionar sección 2.', function () {
        var id = 2;

        misInversionesCircularizacionCtrl.seleccionarSeccion(id);

        expect(misInversionesCircularizacionCtrl.seccionId).toBe(id);
        expect(misInversionesCircularizacionCtrl.seccionURI).toBe('app/mis-inversiones/circularizacion_aprobar.html');
    });
    
    it('Setear las plantillas.', function () {
        misInversionesCircularizacionCtrl.setTemplates();

        expect(misInversionesCircularizacionCtrl.templates[0]).toBe('circularizacion_pendiente.html');
        expect(misInversionesCircularizacionCtrl.templates[1]).toBe('circularizacion_anual.html');
        expect(misInversionesCircularizacionCtrl.templates[2]).toBe('circularizacion_aprobar.html');
    });

    it('Obtener pendiente, resultado verdadero.', function () {
        var circularizacionPendienteInput_stub = { rut: usuario_stub.Rut, fecha: '25/05/2015' };

        misInversionesCircularizacionCtrl.getPendiente(circularizacionPendienteInput_stub);
        postWebService_deferred.resolve({ Resultado: true });
        $rootScope.$digest();

        expect(misInversionesCircularizacionCtrl.pendienteResultado.Resultado).toBe(true);
    });

    it('Obtener pendiente, resultado falso.', function () {        
        var circularizacionPendienteInput_stub = { rut: usuario_stub.Rut, fecha: '25/05/2015' };

        misInversionesCircularizacionCtrl.getPendiente(circularizacionPendienteInput_stub);
        postWebService_deferred.resolve({ Resultado: false });        
        $rootScope.$digest();

        expect(misInversionesCircularizacionCtrl.pendienteResultado.Resultado).toBe(false);
    });


    it('Obtener archivo.', function () {
        var circularizacionArchivoInput_stub = { rut: usuario_stub.Rut, fecha: '25/05/2015' };
        var circularizacionArchivo_stub = {
            UrlCartola: '/url/de/cartola/',
            UrlCircularizacion: '/url/de/circularizacion/'
        };

        misInversionesCircularizacionCtrl.getArchivo(circularizacionArchivoInput_stub);
        postWebService_deferred.resolve(circularizacionArchivo_stub);
        $rootScope.$digest();

        expect(misInversionesCircularizacionCtrl.archivo).toEqual(circularizacionArchivo_stub);
    });

    it('Setear circularización como leída.  Resultado verdadero.', function () {
        var circularizacionLeidaInput_stub = { rut: usuario_stub.Rut, fecha: '25/05/2015' };

        misInversionesCircularizacionCtrl.setLeida(circularizacionLeidaInput_stub);
        postWebService_deferred.resolve({ Resultado: true });
        $rootScope.$digest();

        expect(misInversionesCircularizacionCtrl.leida).toBe(true);
    });

    it('Setear circularización como leída. Resultado falso.', function () {
        var circularizacionLeidaInput_stub = { rut: usuario_stub.Rut, fecha: '25/05/2015' };

        misInversionesCircularizacionCtrl.setLeida(circularizacionLeidaInput_stub);
        postWebService_deferred.resolve({ Resultado: false });
        $rootScope.$digest();

        expect(misInversionesCircularizacionCtrl.leidaResultado).toEqual({ Resultado: false });
        expect(misInversionesCircularizacionCtrl.leida).toBe(false);
    });

    it('Setear circularización como respondida. Resultado verdadero.', function () {
        var circularizacionRespondidaInput_stub = {
            rut_cli: usuario_stub.Rut,
            fecha: '25/05/2015',
            respuesta: 'Respuesta a circularización',
            comentario: 'Comentario de la respuesta de la circularización'
        };

        spyOn(misInversionesCircularizacionCtrl, 'seleccionarSeccion');
        spyOn(misInversionesCircularizacionCtrl, 'getPendiente');

        misInversionesCircularizacionCtrl.setRespondida(circularizacionRespondidaInput_stub);
        postWebService_deferred.resolve({ Resultado: true });
        $rootScope.$digest();

        expect(misInversionesCircularizacionCtrl.respondidaResultado).toEqual({ Resultado: true });
        expect(misInversionesCircularizacionCtrl.seleccionarSeccion).toHaveBeenCalledWith(0);
        expect(misInversionesCircularizacionCtrl.getPendiente).toHaveBeenCalled();
    });

    it('Setear circularización como respondida. Resultado falso.', function () {
        var circularizacionRespondidaInput_stub = {
            rut_cli: usuario_stub.Rut,
            fecha: '25/05/2015',
            respuesta: 'Respuesta a circularización',
            comentario: 'Comentario de la respuesta de la circularización'
        };

        misInversionesCircularizacionCtrl.setRespondida(circularizacionRespondidaInput_stub);
        postWebService_deferred.resolve({ Resultado: false });
        $rootScope.$digest();

        expect(misInversionesCircularizacionCtrl.respondidaResultado).toEqual({ Resultado: false });
    });

    it('Ver documento (Cartola). Documento abierto.', function () {        
        var TipoDocumento;
        (function (TipoDocumento) {
            TipoDocumento[TipoDocumento['Cartola'] = 0] = 'Cartola';
            TipoDocumento[TipoDocumento['Circularizacion'] = 1] = 'Circularizacion';
        })(TipoDocumento || (TipoDocumento = {}));

        misInversionesCircularizacionCtrl.archivo = {
            UrlCartola: 'www.google.com',
            UrlCircularizacion: 'www.google.com'
        };

        spyOn(extrasService, 'abrirRuta').and.callFake(function () {
            return true;
        });
        spyOn(misInversionesCircularizacionCtrl, 'setLeida');

        misInversionesCircularizacionCtrl.verDocumento(TipoDocumento.Cartola);        

        expect(extrasService.abrirRuta).toHaveBeenCalled();
        expect(misInversionesCircularizacionCtrl.setLeida).toHaveBeenCalled();
    });

    it('Ver documento (Circularización).', function () {
        
        var TipoDocumento;
        (function (TipoDocumento) {
            TipoDocumento[TipoDocumento['Cartola'] = 0] = 'Cartola';
            TipoDocumento[TipoDocumento['Circularizacion'] = 1] = 'Circularizacion';
        })(TipoDocumento || (TipoDocumento = {}));

        misInversionesCircularizacionCtrl.archivo = {
            UrlCartola: 'www.google.com',
            UrlCircularizacion: 'www.google.com'
        };

        spyOn(extrasService, 'abrirRuta').and.callFake(function () {
            return true;
        });
        spyOn(misInversionesCircularizacionCtrl, 'setLeida');

        misInversionesCircularizacionCtrl.verDocumento(TipoDocumento.Circularizacion);

        expect(extrasService.abrirRuta).toHaveBeenCalled();
        expect(misInversionesCircularizacionCtrl.setLeida).toHaveBeenCalled();
    });
    
    it('Responder.', function () {
        spyOn(misInversionesCircularizacionCtrl, 'setRespondida');
        misInversionesCircularizacionCtrl.responder();

        expect(misInversionesCircularizacionCtrl.setRespondida).toHaveBeenCalled();
    });
});

