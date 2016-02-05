describe("bodyCtrl - ", function () {
    
    beforeEach(function () {
        module('tannerPrivadoApp');
    });

    var $q, $rootScope,
        constantService, dataService, authService, // dependecias controlador
        getSingle_deferred, postWebService_deferred, // defers 
        bodyCtrl; // controlador

    var usuario_stub = {
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
    
    beforeEach(inject(function (_$q_, _$rootScope_, _constantService_, _dataService_) {

        $q = _$q_;
        $rootScope = _$rootScope_;
        constantService = _constantService_;
        dataService = _dataService_;

        getSingle_deferred = $q.defer();
        postWebService_deferred = $q.defer();

        spyOn(constantService, "mvcHomeURI").and.returnValue("/home/");
        spyOn(constantService, "apiAgenteURI").and.returnValue("/api/agente/");

        spyOn(dataService, "getSingle").and.returnValues(getSingle_deferred.promise, getSingle_deferred.promise);
        spyOn(dataService, "postWebService").and.returnValues(postWebService_deferred.promise, postWebService_deferred.promise, postWebService_deferred.promise);

    }));

    beforeEach(inject(function (_authService_) {

        getSingle_deferred.resolve(usuario_stub);
        $rootScope.$digest();
        authService = _authService_;
        getSingle_deferred = $q.defer();

    }));

    beforeEach(inject(function (_$controller_) {

        bodyCtrl = _$controller_('BodyCtrl', {
            $rootScope: $rootScope,
            constantService: constantService,
            dataService: dataService,
            authService: authService
        });

    }));

    it('Seleccionar sección 5 (por ejemplo)', function () {

        var id = 5;

        bodyCtrl.seleccionarSeccion(id);
        expect(bodyCtrl.seccionId).toBe(id);

    });
});