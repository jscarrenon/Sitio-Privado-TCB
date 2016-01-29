describe("[FrontEnd] bodyCtrl Unit Tests", function () {
    //describe('', function () { });
    var bodyCtrl,
        $rootScope;

    beforeEach(function () {
        module('tannerPrivadoApp');
    });
    describe('BodyCtrl -> ', function () {
        it('Should exist.', function () {
            expect(0).toBe(0);
        });
    });
    describe('BodyCtrl -> seleccionarSeccion( :id)', function () {
        var id = 5;
        beforeEach(inject(function (_$controller_, _$rootScope_, _constantService_, _dataService_, _authService_) {

            bodyCtrl = _$controller_('BodyCtrl', {
                $rootScope: _$rootScope_
            });
        }));
        it('Should assign the id.', function () {
            //spyOn(bodyCtrl, 'seleccionarSeccion');
            bodyCtrl.seleccionarSeccion(5);
            expect(bodyCtrl.seccionId).toBe(5);
        });
    });
    //describe('another thing', function () {

    //});
});