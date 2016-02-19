describe('bodyCtrl - ', function () {
    
    beforeEach(function () {
        module('tannerPrivadoApp');
    });

    var $q, $rootScope, $scope,
        $uibModal, $location, constantService, dataService, authService, // dependecias controlador
        getSingle_deferred, postWebService_deferred, // defers 
        bodyCtrl; // controlador

    var usuario_stub = {
        Autenticado: true,
        Nombres: '',
        Apellidos: '',
        Rut: '12345656-9',
        DireccionComercial: '',
        DireccionParticular: '',
        Ciudad: '',
        Pais: '',
        TelefonoComercial: '',
        TelefonoParticular: '',
        Email: '',
        CuentaCorriente: '',
        Banco: '',
        NombreCompleto: '',
        CiudadPais: ''
    };
    
    var fakeModal = {
        result: {
            then: function (confirmCallback, cancelCallback) {
                //Store the callbacks for later when the user clicks on the OK or Cancel button of the dialog
                this.confirmCallBack = confirmCallback;
                this.cancelCallback = cancelCallback;
            }
        },
        close: function (item) {
            //The user clicked OK on the modal dialog, call the stored confirm callback with the selected item
            this.result.confirmCallBack(item);
        },
        dismiss: function (type) {
            //The user clicked cancel on the modal dialog, call the stored cancel callback
            this.result.cancelCallback(type);
        }
    };

    beforeEach(inject(function (_$q_, _$rootScope_, _$uibModal_, _$location_, _constantService_, _dataService_) {
        $q = _$q_;
        $rootScope = _$rootScope_.$new();
        $scope = $rootScope.$new();
        $uibModal = _$uibModal_;
        $location = _$location_;
        constantService = _constantService_;
        dataService = _dataService_;

        getSingle_deferred = $q.defer();
        postWebService_deferred = $q.defer();

        spyOn(dataService, 'getSingle').and.returnValues(getSingle_deferred.promise, getSingle_deferred.promise);
        spyOn(dataService, 'postWebService').and.returnValues(postWebService_deferred.promise, postWebService_deferred.promise, postWebService_deferred.promise);
        spyOn($uibModal, 'open').and.returnValue(fakeModal);
        spyOn($scope, '$watch');
    }));

    beforeEach(inject(function (_authService_) {
        getSingle_deferred.resolve(usuario_stub);
        $rootScope.$digest();
        authService = _authService_;
    }));

    beforeEach(inject(function (_$controller_) {
        bodyCtrl = _$controller_('BodyCtrl', {
            $rootScope: $rootScope,
            constantService: constantService,
            dataService: dataService,
            authService: authService,
            $scope: $scope,
            $uibModal: $uibModal,
            $location: $location
        });
    }));

    it('Seleccionar sección 5.', function () {
        var id = 5;

        bodyCtrl.seleccionarSeccion(id);
        expect(bodyCtrl.seccionId).toBe(id);
    });

    it('Abrir modal circularización pendiente.', function () {
        authService.circularizacionPendiente = true;
        bodyCtrl.crearInstanciaModal('circularizacion');

        expect(authService.circularizacionPendiente).toBeDefined();
        expect(authService.circularizacionPendiente).toBe(true);
        expect($uibModal.open).toHaveBeenCalled();
    });

    it('Abrir modal documentos pendientes.', function () {
        authService.documentosPendientes = 2;
        bodyCtrl.crearInstanciaModal('documentos');

        expect(authService.documentosPendientes).toBeDefined();
        expect(authService.documentosPendientes).toBeGreaterThan(0);
        expect($uibModal.open).toHaveBeenCalled();
    });

    it('No abrir modal si parametro de entrada es distinto a circularizacion o documentos.', function () {
        bodyCtrl.crearInstanciaModal('otros');

        expect(authService.circularizacionPendiente).toBeDefined();
        expect(authService.documentosPendientes).toBeDefined();
        expect($uibModal.open).not.toHaveBeenCalled();
    });

    it('Abrir modal, circularizacionPendiente = false, documentosPendientes = 0.', function () {
        expect(authService.circularizacionPendiente).toBeDefined();
        expect(authService.documentosPendientes).toBeDefined();
        expect($uibModal.open).not.toHaveBeenCalled();
    });

});