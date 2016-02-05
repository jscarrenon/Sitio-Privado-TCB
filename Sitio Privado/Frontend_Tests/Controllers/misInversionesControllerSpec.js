describe("misInversionesCtrl - ", function () {

    beforeEach(function () {
        module('tannerPrivadoApp');
    });

    var $rootScope, $q,
        constantService, dataService, authService, extrasService, $routeParams, // dependecias controlador
        misInversionesCtrl; // controlador

    beforeEach(inject(function (_$controller_, _$q_, _$rootScope_, _constantService_, _dataService_, _authService_, _extrasService_, _$routeParams_) {

        $q = _$q_;
        $rootScope = _$rootScope_;
        constantService = _constantService_;
        dataService = _dataService_;
        authService = _authService_;
        extrasService = _extrasService_;
        $routeParams = _$routeParams_;

        misInversionesCtrl = _$controller_('MisInversionesCtrl', {
            $rootScope: _$rootScope_,
            constantService: _constantService_,
            dataService: _dataService_,
            authService: _authService_,
            extrasService: _extrasService_,
            $routeParams: _$routeParams_
        });
    }));

    it("seleccionarSeccion(id: number)", function () {

        for (seccionId = 0; seccionId < misInversionesCtrl.templates.length; seccionId++) {
            misInversionesCtrl.seleccionarSeccion(seccionId);
            expect(misInversionesCtrl.seccionId).toBe(seccionId);
            if (seccionId != 1) {

                //El valor de templates[1], se encuentra comentado.
                expect(misInversionesCtrl.templates[seccionId]).toBeNull; 
            } else {                
                expect(misInversionesCtrl.seccionURI).toBe('app/mis-inversiones/' + misInversionesCtrl.templates[seccionId]);
            }            
        }
    });

    it("setTemplates", function () {
        misInversionesCtrl.setTemplates();

        expect(misInversionesCtrl.templates[0]).toBe("nacionales.html");
        expect(misInversionesCtrl.templates[2]).toBe("fondos-mutuos.html");
        expect(misInversionesCtrl.templates[3]).toBe("estado-documentos.html");
        expect(misInversionesCtrl.templates[4]).toBe("circularizacion.html");
    });
});