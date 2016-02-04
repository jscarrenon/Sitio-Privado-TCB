﻿beforeEach(function () {
    module("tannerPrivadoApp");
});

describe("[FrontEnd] MisInversionesFondosMutuosCtrl Unit Tests", function () {
        
    var $rootScope, $routeParams,
        postWebService_deferred, getSingle_deferred,// deferred used for promises
        constantService, dataService, authService, extrasService,// controller dependencies
        fondosMutuosController; // the controller

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
            
        beforeEach(inject(function (_$rootScope_, _$q_, _constantService_, _dataService_, _extrasService_, _$routeParams_) {
            $q = _$q_
            constantService = _constantService_;
            dataService = _dataService_;
            $routeParams = _$routeParams_;
            $rootScope = _$rootScope_;
            extrasService = _extrasService_;

            getSingle_deferred = $q.defer();
            postWebService_deferred = $q.defer();

            spyOn(constantService, 'apiFondosMutuosURI').and.returnValues('/api/fondoMutuo/', '/api/fondoMutuo/');
            spyOn(constantService, 'mvcHomeURI').and.returnValue('/Home/');
            spyOn(constantService, 'apiCircularizacionURI').and.returnValue('/api/circularizacion/');

            spyOn(dataService, 'getSingle').and.returnValue(getSingle_deferred.promise);
            spyOn(dataService, 'postWebService').and.returnValues(postWebService_deferred.promise, postWebService_deferred.promise, postWebService_deferred.promise);
            spyOn(extrasService, 'getRutParteEntera');

        }));

        beforeEach(inject(function (_authService_) {

            getSingle_deferred.resolve(usuario);
            $rootScope.$digest();
            authService = _authService_;

        }));

        beforeEach(inject(function (_$controller_) {           
            
            fondosMutuosController = _$controller_("MisInversionesFondosMutuosCtrl", {
                $rootScope: $rootScope,
                constantService: constantService,
                dataService: dataService,
                authService: authService,
                extrasService: extrasService,
                $routeParams: $routeParams
            });

            //en el constructor del controlador se ejecuta getFondosMutuos(), por eso se utiliza el defer.resolve
            postWebService_deferred.resolve(fondosMutuos_stub);
            $rootScope.$digest();

        }));     
        
        it("getFondosMutuos() - Arreglos de fondos mutuos correctamente definidos y asignados.", function () {
            
            //postWebService_deferred = $q.defer();
            var getFondosMutuosTotalSpy = spyOn(fondosMutuosController, 'getFondosMutuosTotal');
            fondosMutuosController.getFondosMutuos(fondoMutuoInput_stub);

            //se resuelva el resultado del defer de getFondosMutuos()
            postWebService_deferred.resolve(fondosMutuos_stub);
            $rootScope.$digest();
            
            //Esperamos que las listas de fondos mutuos (RF y RV) estén correctas.
            expect(fondosMutuosController.fondosMutuosRF).toBe(fondosMutuos_stub["fondosMutuosRF"]);
            expect(fondosMutuosController.fondosMutuosRV).toBe(fondosMutuos_stub["fondosMutuosRV"]);
            expect(getFondosMutuosTotalSpy).toHaveBeenCalled();

        });

        it("getFondosMutuosTotal() - Totales de fondos mutuos sumados correctamente.", function () {

            fondosMutuosController.fondosMutuosRF = fondosMutuos_stub["fondosMutuosRF"];
            fondosMutuosController.fondosMutuosRV = fondosMutuos_stub["fondosMutuosRV"];
            fondosMutuosController.getFondosMutuosTotal();

            //Esperamos que la sumatoria total de los fondos mutuos (RF y RV) sean correctas.
            expect(fondosMutuosController.fondosMutuosRFTotal).toBe(123945 + 34567);
            expect(fondosMutuosController.fondosMutuosRVTotal).toBe(785233);            

        });
    });