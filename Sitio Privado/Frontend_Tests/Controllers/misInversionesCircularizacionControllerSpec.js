describe("misInversionesCircularizacionCtrl - ", function () {

    beforeEach( function() {
        module("tannerPrivadoApp");
    });

    var $q, $rootScope,
        constantService, dataService, authService, extrasService, // dependecias controlador
        getSingle_deferred, postWebService_deferred, // defers
        misInversionesCircularizacionController; // controlador

    var usuario = {
            Autenticado: true,
            Nombres: "",
            Apellidos: "",
            Rut: "12345656",
            DireccionComercial: "",
            DireccionParticular: "",
            Ciudad: "",
            Pais: "",
            TelefonoComercial: "",
            TelefonoParticular: "",
            Email: "",
            CuentaCorriente: "",
            Banco: "",
            NombreCompleto: "",
            CiudadPais: ""
    };

    beforeEach(inject(function (_$q_, _$rootScope_, _constantService_, _dataService_, _extrasService_) {

        $q = _$q_;
        $rootScope = _$rootScope_;
        constantService = _constantService_;
        dataService = _dataService_;            
        extrasService = _extrasService_;

        getSingle_deferred = $q.defer();
        postWebService_deferred = $q.defer();

        spyOn(extrasService, "getRutParteEntera");
        spyOn(extrasService, "getFechaFormato").and.returnValue("24/02/2016");
        spyOn(constantService, "mvcHomeURI").and.returnValue("/home/");
        spyOn(constantService, 'apiCircularizacionURI').and.returnValue('/api/circularizacion/');

        spyOn(dataService, 'getSingle').and.returnValue(getSingle_deferred.promise);
        spyOn(dataService, 'postWebService').and.returnValues(postWebService_deferred.promise, postWebService_deferred.promise, postWebService_deferred.promise);
        
    }));

    beforeEach(inject(function (_authService_) {

        getSingle_deferred.resolve(usuario);
        $rootScope.$digest();
        authService = _authService_;

    }));

    beforeEach(inject(
        function (_$controller_) {

            misInversionesCircularizacionController = _$controller_("MisInversionesCircularizacionCtrl", {
                $rootScope: $rootScope,
                constantService: constantService,
                dataService: dataService,
                authService: authService,
                extrasService: extrasService
            });

            postWebService_deferred.resolve({
                UrlCartola: "/cartola/",
                UrlCircularizacion: "/circularizacíon/"
            });
            $rootScope.$digest();

            postWebService_deferred.resolve(false);
            $rootScope.$digest();

        }
    ));

    it("Seleccionar sección 0", function () {

        var id = 0;
        misInversionesCircularizacionController.seleccionarSeccion(id);

        expect(misInversionesCircularizacionController.seccionId).toBe(id);
        expect(misInversionesCircularizacionController.seccionURI).toBe("app/mis-inversiones/circularizacion_pendiente.html");
    });

    it("Seleccionar sección 1", function () {

        var id = 1;
        var getArchivoSpy = spyOn(misInversionesCircularizacionController, "getArchivo");
        misInversionesCircularizacionController.seleccionarSeccion(id);

        expect(misInversionesCircularizacionController.seccionId).toBe(id);
        expect(getArchivoSpy).toHaveBeenCalled();
        expect(misInversionesCircularizacionController.seccionURI).toBe("app/mis-inversiones/circularizacion_anual.html");
    });

    it("Seleccionar sección 2", function () {

        var id = 2;
        misInversionesCircularizacionController.seleccionarSeccion(id);

        expect(misInversionesCircularizacionController.seccionId).toBe(id);
        expect(misInversionesCircularizacionController.seccionURI).toBe("app/mis-inversiones/circularizacion_aprobar.html");
    });
    
    it("Setear las plantillas.", function () {
            
        misInversionesCircularizacionController.setTemplates();

        expect(misInversionesCircularizacionController.templates[0]).toBe("circularizacion_pendiente.html");
        expect(misInversionesCircularizacionController.templates[1]).toBe("circularizacion_anual.html");
        expect(misInversionesCircularizacionController.templates[2]).toBe("circularizacion_aprobar.html");

    });
});

