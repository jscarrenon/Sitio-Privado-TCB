describe("[FrontEnd] agenteCtrl Unit Tests", function () {
    //describe('', function () { });
    var agenteCtrl,
        $rootScope,
        constantService,
        dataService,
        authService;

    beforeEach(function () {
        module('tannerPrivadoApp');
    });
    describe('AgenteCtrl -> ', function () {
        it('Should exist.', function () {
            expect(0).toBe(0);
        });
    });
    describe('AgenteCtrl -> getAgente(input: app.domain.IAgenteInput)', function () {

        beforeEach(inject(function (_$controller_, _$rootScope_, _constantService_, _dataService_, _authService_, _$q_) {
            $q = _$q_;
            constantService = _constantService_;
            dataService = _dataService_;
            postWebservice_deferred = $q.defer();
            spyOn(dataService, 'postWebService').and.returnValue(postWebservice_deferred.promise);
            agenteCtrl = _$controller_('AgenteCtrl', {
                $rootScope: _$rootScope_
            });
        }));

    });
});