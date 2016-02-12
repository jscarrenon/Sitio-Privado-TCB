describe('inversionesCtrl - ', function () {

    beforeEach(function () {
        module('tannerPrivadoApp');
    });

    var $q, $rootScope,
        get_deferred,
        constantService, dataService, // dependencias del controlador
        inversionesCtrl; // controlador

    var categorias_stub = [
        {
            Identificador: 3,
            Descriptor: 'Conservador',
            Comentario: '',
            Productos: null
        }, {
            Identificador: 2,
            Descriptor: 'Agressive',
            Comentario: '',
            Productos: null
        },
        {
            Identificador: 1,
            Descriptor: 'Moderado',
            Comentario: '',
            Productos: null
        }, {
            Identificador: 4,
            Descriptor: 'Agresivo',
            Comentario: '',
            Productos: null
        }]

    beforeEach(inject(function (_$controller_, _$q_, _$rootScope_, _constantService_, _dataService_) {

        $q = _$q_;
        $rootScope = _$rootScope_;
        constantService = _constantService_;
        dataService = _dataService_;

        get_deferred = $q.defer();
        spyOn(dataService, 'get').and.returnValues(get_deferred.promise, get_deferred.promise);

        inversionesCtrl = _$controller_('InversionesCtrl', {
            $rootScope: $rootScope,
            constantService: constantService,
            dataService: dataService
        });
    }));

    it('Obtener categorias filtradas por descriptor y ordenadas por identificador.', function () {
        inversionesCtrl.getCategorias();
        get_deferred.resolve(categorias_stub);
        $rootScope.$digest();

        expect(inversionesCtrl.categorias).toEqual([categorias_stub[2], categorias_stub[0], categorias_stub[3]]);
    });
});