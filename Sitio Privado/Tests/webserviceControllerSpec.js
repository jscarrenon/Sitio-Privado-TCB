/// <chutzpah_reference path="../Scripts/extras/jquery/jquery-1.12.0.js" >
/// <chutzpah_reference path="../bower_components/angular/angular.js" >
/// <chutzpah_reference path="../bower_components/angular-route/angular-route.js" >
/// <chutzpah_reference path="../node_modules/angular-mocks/angular-mock.js" >
/// <reference path="../Scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../Scripts/typings/angularjs/angular.d.ts" />
/// <reference path="../Scripts/typings/angularjs/angular-mocks.d.ts" />
/// <reference path="../app/common/controllers/webserviceCtrl.ts" />
/// <reference path="../app/app.ts" />
/// <reference path="../app/domain/FondoMutuoInput.ts" />
/// <reference path="../app/domain/FondoMutuo.ts" />
describe('When using webserviceController ', function () {
    //initialize Angular
    beforeEach(angular.mock.module('tannerPrivadoApp'));
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
        expect(0).toBe(0);
    });
});
//# sourceMappingURL=webserviceControllerSpec.js.map