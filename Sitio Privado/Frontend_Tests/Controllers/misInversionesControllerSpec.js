describe('misInversionesCtrl - ', function () {

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

    it('Seleccionar sección 0.', function () {
        var seccionId = 0;
        misInversionesCtrl.seleccionarSeccion(seccionId);

        expect(misInversionesCtrl.seccionId).toBe(seccionId);
        expect(misInversionesCtrl.seccionURI).toEqual('.build/html/modules/mis-inversiones/templates/nacionales.html');
    });

    it('Seleccionar sección 2.', function () {
        var seccionId = 2;
        misInversionesCtrl.seleccionarSeccion(seccionId);

        expect(misInversionesCtrl.seccionId).toBe(seccionId);
        expect(misInversionesCtrl.seccionURI).toEqual('.build/html/modules/mis-inversiones/templates/fondos-mutuos.html');
    });

    it('Seleccionar sección 3.', function () {
        var seccionId = 3;
        misInversionesCtrl.seleccionarSeccion(seccionId);

        expect(misInversionesCtrl.seccionId).toBe(seccionId);
        expect(misInversionesCtrl.seccionURI).toEqual('.build/html/modules/mis-inversiones/templates/estado-documentos.html');
    });

    it('Seleccionar sección 4.', function () {
        var seccionId = 4;
        misInversionesCtrl.seleccionarSeccion(seccionId);

        expect(misInversionesCtrl.seccionId).toBe(seccionId);
        expect(misInversionesCtrl.seccionURI).toEqual('.build/html/modules/mis-inversiones/templates/circularizacion.html');
    });

    it('Setear plantillas.', function () {
        misInversionesCtrl.setTemplates();

        expect(misInversionesCtrl.templates[0]).toBe('nacionales.html');
        expect(misInversionesCtrl.templates[2]).toBe('fondos-mutuos.html');
        expect(misInversionesCtrl.templates[3]).toBe('estado-documentos.html');
        expect(misInversionesCtrl.templates[4]).toBe('circularizacion.html');
    });
});