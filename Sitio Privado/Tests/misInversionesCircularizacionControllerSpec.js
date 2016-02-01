describe("[FrontEnd] MisInversionesCircularizacionCtrl Unit Tests.", function() {
    beforeEach( function() {
        module("tannerPrivadoApp");
    });

    describe("", function () {
        var $q, get_deferred, misInversionesCircularizacionController, constantService, dataService, $routeParams;

        beforeEach(inject(function (_$controller_, _$rootScope_) {
            
            misInversionesCircularizacionController = _$controller_("MisInversionesCircularizacionCtrl", {
                $rootScope: _$rootScope_
            });

        }));

        beforeEach(function () {
            misInversionesCircularizacionController.setTemplates();
            misInversionesCircularizacionController.seccionId = 0;
            misInversionesCircularizacionController.seleccionarSeccion(misInversionesCircularizacionController.seccionId);
        });

        it("seleccionarSeccion(id: number)", function () {

            for (id = 0; id < misInversionesCircularizacionController.templates.length; id++) {
                misInversionesCircularizacionController.seleccionarSeccion(id);

                expect(misInversionesCircularizacionController.seccionId).toBe(id);
                expect(misInversionesCircularizacionController.seccionURI).toBe('app/mis-inversiones/' + misInversionesCircularizacionController.templates[id]);
            }
        });

        it("setTemplates()", function () {
            
            misInversionesCircularizacionController.setTemplates();

            expect(misInversionesCircularizacionController.templates[0]).toBe("circularizacion_pendiente.html");
            expect(misInversionesCircularizacionController.templates[1]).toBe("circularizacion_anterior.html");
            expect(misInversionesCircularizacionController.templates[2]).toBe("circularizacion_anual-2015.html");
            expect(misInversionesCircularizacionController.templates[3]).toBe("circularizacion_aprobar.html");           

        });
    });
});

