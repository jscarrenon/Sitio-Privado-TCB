describe("TestService", () => {

    var mock: ng.IMockStatic;
    var $httpBackend: ng.IHttpBackendService;
    var dataService; //AngularTest.Interfaces.IProductsService;
    var constantService; 
    var rootScopeFake;
    var controller;
    var $controller: ng.IControllerService;

    mock = angular.mock;
    beforeEach(mock.module('App.Common.Controllers.WebserviceCtrl'));

    beforeEach(() => inject(function (_$httpBackend_, $injector, $rootScope, _$controller_) {
        $httpBackend = _$httpBackend_;
        rootScopeFake = $rootScope.$new();
        dataService = $injector.get('dataService');
        constantService = $injector.get('constantService');
        $controller = _$controller_;
    }));

    afterEach(function () {
        $httpBackend.verifyNoOutstandingExpectation();
        $httpBackend.verifyNoOutstandingRequest();
    });

    it("Should Call API", function () {
        controller = $controller('webserviceCtrl', { constantService: constantService, dataService: dataService, $scope: rootScopeFake });

        spyOn(dataService, "getAllProducts");

        expect($httpBackend.expectGET('/api/Angular').respond([
            { "x": "xx", "xxx": "xxxx", "xxxxx": 5 },
            { "x": "xx", "xxx": "xxxx", "xxxxx": 5.5 },
            { "x": "xx", "xxx": "xxxx", "xxxxx": 6 },
            { "x": "xx", "xxx": "xxxx", "xxxxx": 0 }
        ])).toHaveBeenCalled;

        
        $httpBackend.flush();

    });
});