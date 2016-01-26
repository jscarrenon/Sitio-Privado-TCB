/// <reference path="../Scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../Scripts/typings/angularjs/angular.d.ts" />
/// <reference path="../Scripts/typings/angularjs/angular-route.d.ts" />
/// <reference path="../Scripts/typings/angularjs/angular-mocks.d.ts" />
/// <reference path="../Scripts/typings/jasmine/jasmine.d.ts" />
angular.module("tannerPrivadoApp", ['ngRoute']);

namespace Tests{
    describe("Tests unitarios controlador", () => {

        var $http: angular.IHttpService;
        var $httpBackend: angular.IHttpBackendService;
        var controller: app.common.controllers.WebserviceCtrl;
        var constantService: app.common.services.ConstantService;
        var dataService: app.common.services.DataService;

        beforeEach(() => {
            angular.mock.inject((_$http_: angular.IHttpService, _$httpBackend_: angular.IHttpBackendService) => {
                $http = _$http_;
                $httpBackend = _$httpBackend_;
                controller = new app.common.controllers.WebserviceCtrl(constantService, dataService);
            });
        });

        it("should runs tests", () => {
            expect(0).toBe(0);
        });
    });
}
    
