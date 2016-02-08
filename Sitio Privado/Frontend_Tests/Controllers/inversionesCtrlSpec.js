describe("inversionesCtrl - ", function () {

    beforeEach(function () {
        module("tannerAppPrivado");
    });

    var $q, $rootScope,
        constantService, dataService, // dependencias del controlador
        inversionesCtrl; // controlador

    beforeEach(inject(function (_$controller_, _$q_, _$rootScope_, _constantService_, _dataService_) {

        $q = _$q_;
        $rootScope = _$rootScope_;
        constantService = _constantService_;
        dataService = _dataService_;

        inversionesCtrl = _$controller_("InversionesCtrl", {
            $rootScope: $rootScope,
            constantService: constantService,
            dataService: dataService
        });

    }));
});