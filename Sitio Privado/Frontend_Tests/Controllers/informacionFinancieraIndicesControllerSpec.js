describe('informacionFinancieraIndicesCtrl - ', function () {

    beforeEach(function () {
        module('tannerPrivadoApp');
    });

    var $q, $rootScope,
        constantService, dataService, extrasService, // dependecias controlador
        postWebService_deferred, // defers
        informacionFinancieraIndicesCtrl; // controlador

    var indices_input_stub = { xfecha: '24/11/2015' };

    var indices_stub = {
        RutTCB: '',
        DescriptorTCB: '',
        ActivosSieteDias: 34155,
        PasivosSieteDias: 11577,
        ActivosIntermediacion: 82792,
        AcreedoresIntermediacion: 47,
        TotalPasivosExigibles: 34654,
        PatrimonioLiquido: 2554,
        MontoCoberturaPatrimonial: 345768,
        PatrimonioDepurado: 1593467654,
        FechaConsulta: '25 de noviembre de 2015',
        LiquidezGeneral: 21323546,
        LiquidezIntermediacion: 123567821,
        RazonEndeudamiento: 345765,
        RazonCoberturaPatrimonial: 2344
    };

    beforeEach(inject(function (_$controller_, _$rootScope_, _$q_, _constantService_, _dataService_, _extrasService_) {
        $q = _$q_;
        $rootScope = _$rootScope_;
        constantService = _constantService_;
        dataService = _dataService_;
        extrasService = _extrasService_;

        postWebService_deferred = $q.defer();
        spyOn(dataService, 'postWebService').and.returnValues(postWebService_deferred.promise, postWebService_deferred.promise);
        spyOn(extrasService, 'getFechaFormato').and.returnValue('24/11/2015', '25/11/2015');

        informacionFinancieraIndicesCtrl = _$controller_('InformacionFinancieraIndicesCtrl', {
            $rootScope: _$rootScope_,
            constantService: _constantService_,
            dataService: _dataService_,
            extrasService: _extrasService_
        });

        postWebService_deferred.resolve(indices_stub);
        $rootScope.$digest();
        postWebService_deferred = $q.defer();
    }));

    it('debería obtener indices.', function () {
        informacionFinancieraIndicesCtrl.getIndices(indices_input_stub);
        postWebService_deferred.resolve(indices_stub);
        $rootScope.$digest();

        expect(informacionFinancieraIndicesCtrl.indices).toBe(indices_stub);
    });

    it('debería actualizar indices correctamente.', function () {
        var spy = spyOn(informacionFinancieraIndicesCtrl, 'getIndices');
        informacionFinancieraIndicesCtrl.actualizarIndices();

        expect(informacionFinancieraIndicesCtrl.indicesInput.xfecha).toBe(indices_input_stub.xfecha);
        expect(spy).toHaveBeenCalled();
    });

    it('debería validar fecha correctamente si la fehca está definida.', function () {
        informacionFinancieraIndicesCtrl.validarFecha();

        expect(informacionFinancieraIndicesCtrl.errorFecha).toBeDefined();
    });

    it('no debería validar fecha si la fecha está indefinida', function () {
        informacionFinancieraIndicesCtrl.fecha = undefined;
        informacionFinancieraIndicesCtrl.validarFecha();

        expect(informacionFinancieraIndicesCtrl.errorFecha).toBe('La fecha es inválida.');
    });
});