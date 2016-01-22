/// <reference path="../Scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../Scripts/typings/angularjs/angular.d.ts" />
/// <reference path="../Scripts/typings/angularjs/angular-mocks.d.ts" />
/// <reference path="../app/common/controllers/webserviceCtrl.ts" />
/// <reference path="../app/domain/FondoMutuoInput.ts" />
/// <reference path="../app/domain/FondoMutuo.ts" />

describe('When using webserviceController ', function () {

    //initialize Angular
    beforeEach(module('tannerPrivadoApp'));

    var fondoMutuoInput = { rut_cli: 123456789 };

    var FondosMutuoArray = [
        {
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
            Descripcion: "Fondo F4",
            Tipo: "Tipo fondo F clase 4",
            CtaPisys: "235567",
            ValorCuota: 456788,
            SaldoCuota: 2346,
            Csbis: "S",
            Renta: "RV",
            Pesos: 785233
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
        }
    ];
    
    //parse out the scope for use in our unit tests.
    var scope;
    beforeEach(inject(['constantService', 'dataService']));
    
    it('initial value is 5', function () {
        scope.getFondosMutuos(fondoMutuoInput).expect(scope.getFondosMutuos(scope.FondosMutuosRF)).toBe([FondosMutuoArray(0), FondosMutuoArray(2)]);
        scope.getFondosMutuos(fondoMutuoInput).expect(scope.getFondosMutuos(scope.FondosMutuosRV)).toBe([FondosMutuoArray(1)]);
    });
});