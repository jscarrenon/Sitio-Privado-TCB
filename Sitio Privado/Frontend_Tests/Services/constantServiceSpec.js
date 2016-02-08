describe("constantService - ", function () {
    beforeEach(function () {
        module("tannerPrivadoApp");
    });

    var constantService;

    beforeEach(inject(function (_constantService_) {
        constantService = _constantService_;
    }));

    it("Constantes definidas correctamente.", function () {
        expect(constantService.mvcHomeURI).toBe('/Home/');
        expect(constantService.mvcSignOutURI).toBe('/Account/SignOut');
        expect(constantService.templateFooterURI).toBe('app/common/templates/footer.html');
        expect(constantService.templatePaginationURI).toBe('app/common/templates/pagination.html');
        expect(constantService.apiAgenteURI).toBe('/api/agente/');
        expect(constantService.apiFondosMutuosURI).toBe('/api/fondoMutuo/');
        expect(constantService.apiCategoriaURI).toBe('/api/categoria/');
        expect(constantService.apiProductoURI).toBe('/api/producto/');
        expect(constantService.apiBlobsURI).toBe('/api/containers/');
        expect(constantService.apiBalanceURI).toBe('/api/balance/');
        expect(constantService.apiCartolaURI).toBe('/api/cartola/');
        expect(constantService.apiDocumentoURI).toBe('/api/documento/');
        expect(constantService.apiIndicesURI).toBe('/api/indices/');
        expect(constantService.apiCircularizacionURI).toBe('/api/circularizacion/');
    });
});