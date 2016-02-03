describe("[FrontEnd] misInversionesCtrl unit Tests", function () {
    beforeEach(function () {
        module('tannerPrivadoApp');
    });

    var $rootScope, $q, constantService, dataService, authService, extrasService, $routeParams,
        misInversionesController;

    beforeEach(inject(function (_$controller_, _$q_, _$rootScope_, _constantService_, _dataService_, _authService_, _extrasService_, _$routeParams_) {
        $q = _$q_;
        $rootScope = _$rootScope_;
        constantService = _constantService_;
        dataService = _dataService_;
        authService = _authService_;
        extrasService = _extrasService_;
        $routeParams = _$routeParams_;

        misInversionesController = _$controller_('MisInversionesCtrl', {
            $rootScope: _$rootScope_,
            constantService: _constantService_,
            dataService: _dataService_,
            authService: _authService_,
            extrasService: _extrasService_,
            $routeParams: _$routeParams_
        });
    }));

    beforeEach(function () {
        misInversionesController.setTemplates();
        misInversionesController.seccionId = 0;
    });

    it("seleccionarSeccion(id: number)", function () {

        for (seccionId = 0; seccionId < misInversionesController.templates.length; seccionId++) {
            misInversionesController.seleccionarSeccion(seccionId);
            expect(misInversionesController.seccionId).toBe(seccionId);
            if (seccionId != 1) {

                //El valor de templates[1], se encuentra comentado.
                expect(misInversionesController.templates[seccionId]).toBeNull; 
            } else {                
                expect(misInversionesController.seccionURI).toBe('app/mis-inversiones/' + misInversionesController.templates[seccionId]);
            }            
        }
    });

    it("setTemplates", function () {
        misInversionesController.setTemplates();

        expect(misInversionesController.templates[0]).toBe("nacionales.html");
        expect(misInversionesController.templates[2]).toBe("fondos-mutuos.html");
        expect(misInversionesController.templates[3]).toBe("estado-documentos.html");
        expect(misInversionesController.templates[4]).toBe("circularizacion.html");
    });
});