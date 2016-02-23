describe('constantService - ', function () {
    beforeEach(function () {
        module('tannerPrivadoApp');
    });

    var constantService;

    beforeEach(inject(function (_constantService_) {
        constantService = _constantService_;
    }));

    it('debería definir las constantes correctamente.', function () {
        expect(constantService.mvcHomeURI).toEqual('/Home/');
        expect(constantService.mvcSignOutURI).toEqual('/Account/SignOut');
        expect(constantService.templateFooterURI).toEqual('.build/html/common/templates/footer.html');
        expect(constantService.templatePaginationURI).toEqual('.build/html/common/templates/pagination.html');
        expect(constantService.templateLoadingURI).toEqual('.build/html/common/templates/loading.html');
        expect(constantService.apiAgenteURI).toEqual('/api/agente/');
        expect(constantService.apiFondosMutuosURI).toEqual('/api/fondoMutuo/');
        expect(constantService.apiCategoriaURI).toEqual('/api/categoria/');
        expect(constantService.apiProductoURI).toEqual('/api/producto/');
        expect(constantService.apiBlobsURI).toEqual('/api/containers/');
        expect(constantService.apiBalanceURI).toEqual('/api/balance/');
        expect(constantService.apiCartolaURI).toEqual('/api/cartola/');
        expect(constantService.apiDocumentoURI).toEqual('/api/documento/');
        expect(constantService.apiIndicesURI).toEqual('/api/indices/');
        expect(constantService.apiCircularizacionURI).toEqual('/api/circularizacion/');
    });
});