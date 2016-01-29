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
        //it('should return some agent?', function () {
        //    var rut = { _rut: 123456789 };
        //    var agente={
        //        Codigo: 123,
        //        Nombre: "Juanin Juan Jarri",
        //        Sucursal: "Providencia",
        //        PathImg: "/some/path/somewhere",
        //        Email: "asd@asd.cl",
        //        Telefono: "123123123",
        //        FechaInicioAcreditacion: "23/12/2015",
        //        FechaExpiracionAcreditacion: "23/12/2030",
        //        Descriptor: "asdasdasdasdasdasd",
        //    };
        //    spyOn(agenteCtrl, 'getAgente');
        //});

    });
    //describe('another thing', function () {

    //});
});