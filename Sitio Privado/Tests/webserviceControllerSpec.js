describe("[FrontEnd] WebserviceCtrl Unit Tests", function () {
    
    beforeEach(function () {
        module("tannerPrivadoApp");
    });

    // Declare describe-scoped variables 
    var $rootScope, $rootScope,
        postWebservice_deferred, // deferred used for promises
        constantService, dataService, // controller dependencies
        webserviceController; // the controller

    //beforeEach(inject(function (_$controller_, _$q_, _constantService_, _dataService_) {
    //beforeEach(inject(function($injector) {
    beforeEach(inject(function (_$controller_, _$rootScope_, _$q_, _constantService_, _dataService_) {
        $q = _$q_
        constantService = _constantService_;
        dataService = _dataService_;
        $rootScope = _$rootScope_;

        postWebservice_deferred = $q.defer();

        spyOn(constantService, 'apiFondosMutuosURI').and.returnValue('/api/fondoMutuo/');
        
        spyOn(dataService, 'postWebService').and.returnValue(postWebservice_deferred.promise);
        
        webserviceController = _$controller_("WebserviceCtrl", {
            $rootScope: _$rootScope_
        });
    }));

    // Tests for the sageDetail controller
    var fondoMutuoInput_stub = { rut_cli: 123456789 };

    var fondosMutuosRF = [{
        Descripcion: "Fondo A1",
        Tipo: "Tipo fondo A clase 1",
        CtaPisys: "235567",
        ValorCuota: 101000,
        SaldoCuota: 5004999,
        Csbis: "S",
        Renta: "RF",
        Pesos: 123945
    },
        {
        Descripcion: "Fondo C5",
        Tipo: "Tipo fondo C clase 5",
        CtaPisys: "235567",
        ValorCuota: 12345,
        SaldoCuota: 3468,
        Csbis: "N",
        Renta: "RF",
        Pesos: 34567
    }];

    var fondosMutuosRV = [{
        Descripcion: "Fondo F4",
        Tipo: "Tipo fondo F clase 4",
        CtaPisys: "235567",
        ValorCuota: 456788,
        SaldoCuota: 2346,
        Csbis: "S",
        Renta: "RV",
        Pesos: 785233
    }];

    var fondosMutuos_stub = {
        fondosMutuosRF: fondosMutuosRF, fondosMutuosRV: fondosMutuosRV
    };
    
    it("getFondosMutuos() - Arreglos de fondos mutuos correctamente definidos y asignados.", function () {        
        spyOn(webserviceController, 'getFondosMutuosTotal');

        webserviceController.getFondosMutuos(fondoMutuoInput_stub);

        postWebservice_deferred.resolve(fondosMutuos_stub);
        $rootScope.$digest();        

        //Esperamos que las listas de fondos mutuos (RF y RV) estén correctas.
        expect(webserviceController.fondosMutuosRF).toBe(fondosMutuos_stub["fondosMutuosRF"]);
        expect(webserviceController.fondosMutuosRV).toBe(fondosMutuos_stub["fondosMutuosRV"]);

        
    });

    it("getFondosMutuosTotal() - Totales de fondos mutuos sumados correctamente.", function () {
        webserviceController.fondosMutuosRF = fondosMutuos_stub["fondosMutuosRF"];
        webserviceController.fondosMutuosRV = fondosMutuos_stub["fondosMutuosRV"];

        webserviceController.getFondosMutuosTotal();
        //Esperamos que la sumatoria total de los fondos mutuos (RF y RV) sean correctas.
        expect(webserviceController.fondosMutuosRFTotal).toBe(123945 + 34567);
        expect(webserviceController.fondosMutuosRVTotal).toBe(785233);
    });

});