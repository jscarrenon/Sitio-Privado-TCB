describe("authService - ", function () {

    beforeEach(function () {
        module("tannerPrivadoApp");
    });

    var $rootScope,
        postWebservice_deferred, // deferred used for promises
        authService, //service
        getSingle_deferred, postWebService_deferred,
        $httpBackend, constantService, dataService, extrasService, // service dependencies        
        usuario; 

    //Se inyectan todos los servicios menos el authService, ya que al inyectar el authService se llama su constructor y
    //se ejecuta la función getUsuarioActual(), por lo que hay que preparar un defer.
    beforeEach(inject(function (_$rootScope_, _$q_, _$httpBackend_, _constantService_, _dataService_, _extrasService_) {
        
        $q = _$q_;
        $httpBackend: _$httpBackend_;
        $rootScope = _$rootScope_;
        constantService = _constantService_;
        dataService = _dataService_;
        extrasService = _extrasService_;

        getSingle_deferred = $q.defer();
        postWebService_deferred = $q.defer();

        spyOn(constantService, 'mvcHomeURI').and.returnValue('/Home/');
        spyOn(constantService, 'apiCircularizacionURI').and.returnValue('/api/circularizacion/');

        //Se usa returnValues porque no es puede repetir el spyOn en la misma función.
        spyOn(dataService, 'getSingle').and.returnValues(getSingle_deferred.promise, getSingle_deferred.promise);
        spyOn(dataService, 'postWebService').and.returnValues(postWebService_deferred.promise, postWebService_deferred.promise);

        usuario = {
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
    }));

    //se injecta el authService solo para evitar el problema de la doble llamada.
    beforeEach(inject(function (_authService_) {

        //se resuelve el defer de la primera llamada (al inyectar el servicio)
        getSingle_deferred.resolve(usuario);
        $rootScope.$digest();
        authService = _authService_;

        //se prepara para la segunda llamada que es en el test "Obtener usuario actual".
        getSingle_deferred = $q.defer();        
        
    }));
        
    it("Obtener usuario actual. Usuario autenticado.", function () { //parameter name = service name
        
        authService.getUsuarioActual();
        getSingle_deferred.resolve(usuario);
        $rootScope.$digest();

        expect(authService.usuario).toEqual(usuario);
        expect(authService.autenticado).toEqual(usuario.Autenticado);
        expect(authService.autenticado).toEqual(usuario.Autenticado);

    });

    it("Obtener usuario actual. Usuario no autenticado.", function () { //parameter name = service name

        usuario.Autenticado = false;
        authService.getUsuarioActual();
        getSingle_deferred.resolve(usuario);
        $rootScope.$digest();        

        expect(authService.usuario).toEqual(usuario);
        expect(authService.autenticado).toEqual(usuario.Autenticado);
        expect(authService.circularizacionPendiente).toEqual(false);

    });
});