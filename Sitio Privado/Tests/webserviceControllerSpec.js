/// <reference path="../Scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../Scripts/typings/angularjs/angular.d.ts" />
/// <reference path="../Scripts/typings/angularjs/angular-route.d.ts" />
/// <reference path="../Scripts/typings/angularjs/angular-mocks.d.ts" />
/// <reference path="../Scripts/typings/jasmine/jasmine.d.ts" />
angular.module("tannerPrivadoApp", ['ngRoute']);
var Tests;
(function (Tests) {
    describe("Tests unitarios controlador", function () {
        var app = angular.module('tannerPrivadoApp');
        var $http;
        var $httpBackend;
        var controller;
        var constantService;
        var dataService;
        beforeEach(function () {
            angular.mock.inject(function (_$http_, _$httpBackend_) {
                $http = _$http_;
                $httpBackend = _$httpBackend_;
                controller = new App.Common.Controllers.WebserviceCtrl(constantService, dataService);
            });
        });
        it("should runs tests", function () {
            expect(0).toBe(0);
        });
    });
})(Tests || (Tests = {}));
//# sourceMappingURL=webserviceControllerSpec.js.map