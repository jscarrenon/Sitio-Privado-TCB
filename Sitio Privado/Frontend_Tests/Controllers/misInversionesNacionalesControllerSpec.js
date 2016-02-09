﻿describe("misInversionesNacionalesCtrl", function () {

    beforeEach(function () {
        module("tannerPrivadoApp");
    });

    var $q, $rootScope,
        constantService, dataService, authService, extrasService,
        getSingle_deferred, postWebService_deferred,
        misInversionesNacionalesCtrl;

    var usuario_stub = {
        Autenticado: true,
        Nombres: "",
        Apellidos: "",
        Rut: "1234565-6",
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
    
    var balanceInput_stub = { rut: usuario_stub.Rut };

    var cartolaInput_stub = { _rut: usuario_stub.Rut };

    var balance_stub = {
        Enlace: "enlace/a/balance"
    };

    var cartola_stub = {
        Rut: usuario_stub.Rut,
        Periodo: "perioodsf",
        Conceptos: []
    };

    beforeEach(inject(function (_$q_, _$rootScope_, _constantService_, _dataService_, _extrasService_) {

        $q = _$q_;
        $rootScope = _$rootScope_;
        constantService = _constantService_;
        dataService = _dataService_;
        extrasService = _extrasService_;

        getSingle_deferred = $q.defer();
        postWebService_deferred = $q.defer();

        spyOn(constantService, "apiBalanceURI").and.returnValue("/api/balance/");
        spyOn(constantService, "apiCartolaURI").and.returnValue("/api/cartola/");
        spyOn(constantService, "mvcHomeURI").and.returnValue("/Home/");

        spyOn(dataService, "getSingle").and.returnValues(getSingle_deferred.promise, getSingle_deferred.promise);
        spyOn(dataService, "postWebService").and.returnValues(postWebService_deferred.promise, postWebService_deferred.promise, postWebService_deferred.promise, postWebService_deferred.promise);

    }));

    beforeEach(inject(function (_authService_) {

        spyOn(_authService_, "getCircularizacionPendiente");
        spyOn(_authService_, "getDocumentosPendientes");
        getSingle_deferred.resolve(usuario_stub);
        $rootScope.$digest();

        authService = _authService_;

    }));

    beforeEach(inject(function (_$controller_) {

        misInversionesNacionalesCtrl = _$controller_("MisInversionesNacionalesCtrl", {
            $rootScope: $rootScope,
            constantService: constantService,
            dataService: dataService,
            authService: authService,
            extrasService: extrasService
        });
        
        
    }));

    it("Obtener balance.", function () {

        misInversionesNacionalesCtrl.getBalance(balanceInput_stub);
        postWebService_deferred.resolve(balance_stub);
        $rootScope.$digest();

        expect(misInversionesNacionalesCtrl.balance).toBe(balance_stub);

    });

    it("Obtener cartola.", function () {

        misInversionesNacionalesCtrl.getCartola(cartolaInput_stub);
        postWebService_deferred.resolve(cartola_stub);
        $rootScope.$digest();

        expect(misInversionesNacionalesCtrl.cartola).toBe(cartola_stub);

    });
});