describe("extrasService - ", function () {

    beforeEach(function () {
        module("tannerPrivadoApp");
    });

    var $q, $window, $filter,
        extrasService;

    var fechaActual = new Date();

    beforeEach(inject(function (_$q_, _$window_, _$filter_, _extrasService_) {
        $q = _$q_;
        $window = _$window_;
        $filter = _$filter_;
        extrasService = _extrasService_;
    }));

    it("Obtener parte entera de rut - rut válido.", function () {
        var getRutParteEnteraReturn = extrasService.getRutParteEntera("14536748-9");
        expect(getRutParteEnteraReturn).toBe("14536748");
    });

    it("Obtener parte entera de rut - rut nulo.", function () {
        var rut;
        var getRutParteEnteraReturn = extrasService.getRutParteEntera(rut);
        expect(getRutParteEnteraReturn).toBe("");
    });

    it("Abrir ruta.", function () {

        spyOn($window, 'open').and.callFake(function () {
            return true;
        });

        extrasService.abrirRuta("http://www.google.com", "_blank");
        
        expect($window.open).toHaveBeenCalled();
    });

    it("Formatear fecha: dd/mm/aaaa.", function () {

        var formatoSalida = extrasService.getFechaFormato(fechaActual, "dd/mm/aaaa");

        expect(formatoSalida.length).toBeDefined();
        expect(formatoSalida.length).toBe(10);
        expect(formatoSalida).toMatch(/^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$/i);
    });

    it("Formatear fecha: longDate.", function () {
        
        var formatoSalida = extrasService.getFechaFormato(fechaActual, "longDate");
        
        expect(formatoSalida).toMatch(/^\w+\s(\d{1})?\d{1}\,\s\d{4}$/);
    });

    it("Formatear fecha: mediumDate.", function () {

        var formatoSalida = extrasService.getFechaFormato(fechaActual, "");

        expect(formatoSalida).toMatch(/^[A-Z][a-z][a-z]\s(\d{1})?\d{1}\,\s\d{4}$/);
    });
});