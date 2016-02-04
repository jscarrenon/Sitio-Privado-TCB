describe("", function () {

    beforeEach(function () {
        module("tannerPrivadoApp");
    });

    var $q, $window, $filter,
        extrasService;

    beforeEach(inject(function (_$q_, _$window_, _$filter_, _extrasService_) {
        $q = _$q_;
        $window = _$window_;
        $filter = _$filter_;
        extrasService = _extrasService_;
    }));

    it("getRutParteEntera rut válido", function () {
        var getRutParteEnteraReturn = extrasService.getRutParteEntera("14536748-9");
        expect(getRutParteEnteraReturn).toBe("14536748");
    });

    it("getRutParteEntera rut nulo", function () {
        var rut;
        var getRutParteEnteraReturn = extrasService.getRutParteEntera(rut);
        expect(getRutParteEnteraReturn).toBe("");
    });

    it("Abrir ruta.", function () {
        extrasService.abrirRuta("http://www.google.com", "_blank");
        //var windowOpenSpy = spyOn(extrasService, "$window.open");

        spyOn($window, 'open').and.callFake(function () {
            return true;
        });

        expect($window.open).toHaveBeenCalled();
    });

    it("Formatear fecha. Formato de fecha de entrada: dd/mm/aaaa", function () {
        spyOn(extrasService, "getFechaFormat").and.callThrough();
        var formatoSalida = extrasService.getFechaFormato("02-04-2016", "dd/mm/aaaa");
        expect(formatoSalida).toBe("02/04/2016");
    });

    it("Formatear fecha. Formato de fecha de entrada: longdate", function () {
        
        var formatoSalida = extrasService.getFechaFormato("29/02/2016", "dd-mm-aaaa");
        expect(formatoSalida).toBe("29/02/2016");
    });

    it("Formatear fecha. Formato de fecha de entrada: mediumdate", function () {
        var formatoSalida = extrasService.getFechaFormato("02/04/2016", "dd-mm-aaaa");
        expect(formatoSalida).toBe("02/04/2016");
    });
});