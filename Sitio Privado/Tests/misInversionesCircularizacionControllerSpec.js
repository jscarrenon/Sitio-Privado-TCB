describe("[FrontEnd] MisInversionesCircularizacionCtrl Unit Tests.", function () {

    beforeEach( function() {
        module("tannerPrivadoApp");
    });

    var $q, $rootScope,
        get_deferred, postWebService_deferred,
        misInversionesCircularizacionController,
        constantService, dataService, authService, extrasService;

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

    beforeEach(inject(function (_$q_, _$rootScope_, _constantService_, _dataService_, _authService_, _extrasService_) {
        $q = _$q_;
        $rootScope = _$rootScope_;
        constantService = _constantService_;
        dataService = _dataService_;            
        extrasService = _extrasService_;

        postWebService_deferred = $q.defer();

        spyOn(constantService, "mvcHomeURI").and.returnValue("/home/");
        spyOn(dataService, "getSingle").and.returnValue(postWebService_deferred.promise);
    }));

    beforeEach(inject(function (_authService_) {
        postWebService_deferred.resolve(usuario);
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
        }
    ));

    it("Seleccionar sección 0", function () {
        var id = 0;
        misInversionesCircularizacionController.seleccionarSeccion(id);

        expect(misInversionesCircularizacionController.seccionId).toBe(id);
        expect(misInversionesCircularizacionController.seccionURI).toBe('app/mis-inversiones/' + misInversionesCircularizacionController.templates[id]);
    });

    it("Seleccionar sección 1", function () {
        var id = 0;
        misInversionesCircularizacionController.seleccionarSeccion(id);

        expect(misInversionesCircularizacionController.seccionId).toBe(id);
        expect(misInversionesCircularizacionController.seccionURI).toBe('app/mis-inversiones/' + misInversionesCircularizacionController.templates[id]);
    });

    it("Seleccionar sección 2", function () {
        var id = 0;
        misInversionesCircularizacionController.seleccionarSeccion(id);

        expect(misInversionesCircularizacionController.seccionId).toBe(id);
        expect(misInversionesCircularizacionController.seccionURI).toBe('app/mis-inversiones/' + misInversionesCircularizacionController.templates[id]);
    });

    it("Seleccionar sección 3", function () {
        var id = 0;
        misInversionesCircularizacionController.seleccionarSeccion(id);

        expect(misInversionesCircularizacionController.seccionId).toBe(id);
        expect(misInversionesCircularizacionController.seccionURI).toBe('app/mis-inversiones/' + misInversionesCircularizacionController.templates[id]);
    });

    it("Seleccionar sección 4", function () {
        var id = 0;
        misInversionesCircularizacionController.seleccionarSeccion(id);

        expect(misInversionesCircularizacionController.seccionId).toBe(id);
        expect(misInversionesCircularizacionController.seccionURI).toBe('app/mis-inversiones/' + misInversionesCircularizacionController.templates[id]);
    });
    
    it("setTemplates()", function () {
            
        misInversionesCircularizacionController.setTemplates();

        expect(misInversionesCircularizacionController.templates[0]).toBe("circularizacion_pendiente.html");
        expect(misInversionesCircularizacionController.templates[1]).toBe("circularizacion_anterior.html");
        expect(misInversionesCircularizacionController.templates[2]).toBe("circularizacion_anual-2015.html");
        expect(misInversionesCircularizacionController.templates[3]).toBe("circularizacion_aprobar.html");           

    });
});

