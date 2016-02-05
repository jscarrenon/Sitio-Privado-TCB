describe("homeCtrl - ", function () {

    beforeEach(function () {
        module("tannerPrivadoApp");
    });

    var constantService, dataService, // dependecias controlador
        homeCtrl; //controlador

    beforeEach(inject(function (_$controller_, _constantService_, _dataService_) {
        
        constantService = _constantService_;
        dataService = _dataService_;

        homeCtrl = _$controller_("HomeCtrl", {
            constantService: constantService,
            dataService: dataService
        });

    }));
});