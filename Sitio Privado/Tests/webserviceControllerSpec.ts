/// <reference path="../Scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../Scripts/typings/angularjs/angular.d.ts" />
/// <reference path="../Scripts/typings/angularjs/angular-route.d.ts" />
/// <reference path="../Scripts/typings/angularjs/angular-mocks.d.ts" />
/// <reference path="../Scripts/typings/jasmine/jasmine.d.ts" />
angular.module("tannerPrivadoApp", ['ngRoute']);

namespace Tests{
    describe("Tests unitarios controlador", () => {
        var app = angular.module('tannerPrivadoApp');
        var $http: angular.IHttpService;
        var $httpBackend: angular.IHttpBackendService;
        var controller: App.Common.Controllers.WebserviceCtrl;
        var constantService: App.Common.Services.ConstantService;
        var dataService: App.Common.Services.DataService;

        beforeEach(() => {
            angular.mock.inject((_$http_: angular.IHttpService, _$httpBackend_: angular.IHttpBackendService) => {
                $http = _$http_;
                $httpBackend = _$httpBackend_;
                controller = new App.Common.Controllers.WebserviceCtrl(constantService, dataService);
            });
        });

        it("should runs tests", () => {
            expect(0).toBe(0);
        });
    });
}
    
