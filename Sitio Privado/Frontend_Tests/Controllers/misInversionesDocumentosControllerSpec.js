describe('misInversionesDocumentosCtrl - ', function () {

    beforeEach(function () {
        module('tannerPrivadoApp');
    });

    var $q, $rootScope,
        $filter, $uibModal, constantService, dataService, authService, extrasService, // dependencias controlador
        getSingle_deferred, postWebService_deferred, // deferred 
        misInversionesDocumentosCtrl; // controlador

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

    var documentosFirmados_stub = {
        operaciones: [
            {
                Codigo: '070',
                Producto: 'Producto 70',
                Tipo: 'Tipo B',
                Folio: 'Folio 3',
                FechaCreacion: '30/04/2015',
                Leido: 'S',
                Firmado: 'true',
                TipoFirma: 'digital',
                FechaCreacion: '30/04/2015',
                Ruta: 'c:\\',
                NombreCliente: 'Cliente Cinco',
                RutaFirmado: 'c:\\Firma',
                Seleccionado: false
            },
            {
                Codigo: '088',
                Producto: 'Producto 88',
                Tipo: 'Tipo B',
                Folio: 'Folio 9',
                FechaCreacion: '30/04/2015',
                Leido: 'S',
                Firmado: 'true',
                TipoFirma: 'digital',
                FechaCreacion: '30/04/2015',
                Ruta: 'c:\\',
                NombreCliente: 'Cliente Cinco',
                RutaFirmado: 'c:\\Firma',
                Seleccionado: true
            },
            {
                Codigo: '039',
                Producto: 'Producto 39',
                Tipo: 'Tipo C',
                Folio: 'Folio 7',
                FechaCreacion: '30/04/2015',
                Leido: 'N',
                Firmado: 'true',
                TipoFirma: 'digital',
                FechaCreacion: '30/04/2015',
                Ruta: 'c:\\',
                NombreCliente: 'Cliente Cinco',
                RutaFirmado: 'c:\\Firma',
                Seleccionado: false
            }
        ],
        documentos: [
            {
                Codigo: '094',
                Producto: 'Producto 94',
                Tipo: 'Tipo X',
                Folio: 'Folio 2',
                FechaCreacion: '16/09/2015',
                Leido: 'N',
                Firmado: 'true',
                TipoFirma: 'digital',
                FechaFirma: '26/09/2015',
                Ruta: 'c:\\',
                NombreCliente: 'Cliente Cinco',
                RutaFirmado: 'c:\\Firma',
                Seleccionado: false
            },
            {
                Codigo: '033',
                Producto: 'Producto 3',
                Tipo: 'Tipo V',
                Folio: 'Folio 5',
                FechaCreacion: '25/10/2015',
                Leido: 'S',
                Firmado: 'true',
                TipoFirma: 'digital',
                FechaCreacion: '25/11/2015',
                Ruta: 'c:\\',
                NombreCliente: 'Cliente Cinco',
                RutaFirmado: 'c:\\Firma',
                Seleccionado: true
            }
        ]
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
     
    beforeEach(inject(function (_$q_, _$rootScope_, _$filter_, _$uibModal_, _constantService_, _dataService_, _extrasService_) {
        $q = _$q_;
        $rootScope = _$rootScope_;
        $filter = _$filter_;
        $uibModal = _$uibModal_;
        constantService = _constantService_;
        dataService = _dataService_;
        extrasService = _extrasService_;

        getSingle_deferred = $q.defer();
        postWebService_deferred = $q.defer();

        spyOn($uibModal, 'open').and.returnValue(fakeModal);
        spyOn(dataService, 'getSingle').and.returnValue(getSingle_deferred.promise);
        spyOn(dataService, 'postWebService').and.returnValue(postWebService_deferred.promise);
    }));

    beforeEach(inject(function (_authService_) {
        getSingle_deferred.resolve(usuario_stub);
        $rootScope.$digest();
        authService = _authService_;
    }));

    beforeEach(inject(function (_$controller_) {
        misInversionesDocumentosCtrl = _$controller_('MisInversionesDocumentosCtrl', {
            $rootScope: $rootScope,
            constantService: constantService,
            dataService: dataService,
            authService: authService,
            extrasService: extrasService,
            $filter: $filter,
            $uibModal: $uibModal
        });
    }));

    it('debería seleccionar sección "estado-documentos_pendientes.html" si id es igual a 0.', function () {
        var id = 0;
        misInversionesDocumentosCtrl.seleccionarSeccion(id);

        expect(misInversionesDocumentosCtrl.seccionId).toBe(id);
        expect(misInversionesDocumentosCtrl.seccionURI).toBe('.build/html/modules/mis-inversiones/templates/estado-documentos_pendientes.html');
    });

    it('debería seleccionar sección "estado-documentos_firmados.html" si id es igual a 1.', function () {
        var id = 1;
        misInversionesDocumentosCtrl.seleccionarSeccion(id);

        expect(misInversionesDocumentosCtrl.seccionId).toBe(id);
        expect(misInversionesDocumentosCtrl.seccionURI).toBe('.build/html/modules/mis-inversiones/templates/estado-documentos_firmados.html');
    });

    it('debería setear las plantillas html.', function () {
        misInversionesDocumentosCtrl.setTemplates();

        expect(misInversionesDocumentosCtrl.templates[0]).toBe('estado-documentos_pendientes.html');
        expect(misInversionesDocumentosCtrl.templates[1]).toBe('estado-documentos_firmados.html');
    });

    it('debería configurar constantes de paginación.', function () {
        misInversionesDocumentosCtrl.configurarPaginacion();

        expect(misInversionesDocumentosCtrl.operacionesPendientesPaginaActual).toBe(1);
        expect(misInversionesDocumentosCtrl.operacionesPendientesPorPagina).toBe(15);
        expect(misInversionesDocumentosCtrl.documentosPendientesPaginaActual).toBe(1);
        expect(misInversionesDocumentosCtrl.documentosPendientesPorPagina).toBe(15);
        expect(misInversionesDocumentosCtrl.operacionesFirmadasPaginaActual).toBe(1);
        expect(misInversionesDocumentosCtrl.operacionesFirmadasPorPagina).toBe(15);
        expect(misInversionesDocumentosCtrl.documentosFirmadosPaginaActual).toBe(1);
        expect(misInversionesDocumentosCtrl.documentosFirmadosPorPagina).toBe(15);    
    });

    it('debería obtener documentos pendientes.', function () {
        misInversionesDocumentosCtrl.getDocumentosPendientes({ rut: usuario_stub.Rut });
        postWebService_deferred.resolve(documentosFirmados_stub);
        $rootScope.$digest();

        expect(misInversionesDocumentosCtrl.operacionesPendientes).toEqual(documentosFirmados_stub['operaciones']);
        expect(misInversionesDocumentosCtrl.documentosPendientes).toEqual(documentosFirmados_stub['documentos']);
    });

    it('debería obtener documentos firmados.', function () {

        misInversionesDocumentosCtrl.getDocumentosFirmados({ rut: usuario_stub.Rut });
        postWebService_deferred.resolve(documentosFirmados_stub);
        $rootScope.$digest();

        expect(misInversionesDocumentosCtrl.operacionesFirmadas).toEqual(documentosFirmados_stub['operaciones']);
        expect(misInversionesDocumentosCtrl.documentosFirmados).toEqual(documentosFirmados_stub['documentos']);
    });

    it('debería ver documento. Resultado verdadero.', function () {

        var archivo_stub = {
            Codigo: '070',
            Producto: 'Producto 70',
            Tipo: 'Tipo B',
            Folio: 'Folio 3',
            FechaCreacion: '30/04/2015',
            Leido: 'N',
            Firmado: 'true',
            TipoFirma: 'digital',
            FechaCreacion: '30/04/2015',
            Ruta: 'c:\\',
            NombreCliente: 'Cliente Cinco',
            RutaFirmado: 'c:\\Firma',
            Seleccionado: false
        };

        expect(archivo_stub.Leido).toBe('N');

        spyOn(extrasService, 'abrirRuta').and.callFake(function () {
            return true;
        });

        misInversionesDocumentosCtrl.verDocumento(archivo_stub);
        postWebService_deferred.resolve({ Resultado: true });
        $rootScope.$digest();

        expect(extrasService.abrirRuta).toHaveBeenCalled();
        expect(archivo_stub.Leido).toBe('S');
    });

    it('debería ver documento. Resultado negativo.', function () {

        var archivo_stub = {
            Codigo: '070',
            Producto: 'Producto 70',
            Tipo: 'Tipo B',
            Folio: 'Folio 3',
            FechaCreacion: '30/04/2015',
            Leido: 'N',
            Firmado: 'true',
            TipoFirma: 'digital',
            FechaCreacion: '30/04/2015',
            Ruta: 'c:\\',
            NombreCliente: 'Cliente Cinco',
            RutaFirmado: 'c:\\Firma',
            Seleccionado: false
        };

        expect(archivo_stub.Leido).toBe('N');

        spyOn(extrasService, 'abrirRuta').and.callFake(function () {
            return true;
        });
        
        misInversionesDocumentosCtrl.verDocumento(archivo_stub);
        postWebService_deferred.resolve({ Resultado: false });
        $rootScope.$digest();

        expect(extrasService.abrirRuta).toHaveBeenCalled();
        expect(archivo_stub.Leido).toBe('N');
    });

    it('debería firmar documentos. Declaración verdadera con operaciones y documentos pendientes.', function () {
        misInversionesDocumentosCtrl.declaracion = true;
        misInversionesDocumentosCtrl.operacionesPendientes = documentosFirmados_stub['operaciones'];
        misInversionesDocumentosCtrl.documentosPendientes = documentosFirmados_stub['documentos'];

        spyOn(misInversionesDocumentosCtrl, 'actualizarDocumentosPendientes');
        spyOn(misInversionesDocumentosCtrl, 'actualizarDocumentosFirmados');

        misInversionesDocumentosCtrl.firmarDocumentos();
        postWebService_deferred.resolve({ Documentos: documentosFirmados_stub['operaciones'] });
        $rootScope.$digest();

        postWebService_deferred.resolve({ Documentos: documentosFirmados_stub['documentos'] });
        $rootScope.$digest();

        expect(misInversionesDocumentosCtrl.actualizarDocumentosPendientes).toHaveBeenCalledTimes(2);
        expect(misInversionesDocumentosCtrl.actualizarDocumentosFirmados).toHaveBeenCalledTimes(2);
    });

    it('debería firmar documentos. Declaración verdadera sólo con operaciones pendientes.', function () {
        misInversionesDocumentosCtrl.declaracion = true;
        documentosFirmados_stub['operaciones'][1].Seleccionado = true;
        documentosFirmados_stub['documentos'][1].Seleccionado = false;
        misInversionesDocumentosCtrl.operacionesPendientes = documentosFirmados_stub['operaciones'];
        misInversionesDocumentosCtrl.documentosPendientes = documentosFirmados_stub['documentos'];
        

        spyOn(misInversionesDocumentosCtrl, 'actualizarDocumentosPendientes');
        spyOn(misInversionesDocumentosCtrl, 'actualizarDocumentosFirmados');

        misInversionesDocumentosCtrl.firmarDocumentos();
        postWebService_deferred.resolve({ Documentos: documentosFirmados_stub['operaciones'] });
        $rootScope.$digest();

        expect(misInversionesDocumentosCtrl.actualizarDocumentosPendientes).toHaveBeenCalledTimes(1);
        expect(misInversionesDocumentosCtrl.actualizarDocumentosFirmados).toHaveBeenCalledTimes(1);
    });

    it('debería firmar documentos. Declaración verdadera sólo con documentos pendientes.', function () {
        misInversionesDocumentosCtrl.declaracion = true;
        documentosFirmados_stub['operaciones'][1].Seleccionado = false;
        documentosFirmados_stub['documentos'][1].Seleccionado = true;
        misInversionesDocumentosCtrl.operacionesPendientes = documentosFirmados_stub['operaciones'];
        misInversionesDocumentosCtrl.documentosPendientes = documentosFirmados_stub['documentos'];        

        spyOn(misInversionesDocumentosCtrl, 'actualizarDocumentosPendientes');
        spyOn(misInversionesDocumentosCtrl, 'actualizarDocumentosFirmados');

        misInversionesDocumentosCtrl.firmarDocumentos();
        postWebService_deferred.resolve({ Documentos: documentosFirmados_stub['operaciones'] });
        $rootScope.$digest();

        expect(misInversionesDocumentosCtrl.firmarLoading).toBe(false);
        expect(misInversionesDocumentosCtrl.actualizarDocumentosPendientes).toHaveBeenCalledTimes(1);
        expect(misInversionesDocumentosCtrl.actualizarDocumentosFirmados).toHaveBeenCalledTimes(1);
    });

    it('debería firmar documentos. Declaración no verdadera.', function () {
        misInversionesDocumentosCtrl.declaracion = false;

        spyOn(misInversionesDocumentosCtrl, 'actualizarDocumentosPendientes');
        spyOn(misInversionesDocumentosCtrl, 'actualizarDocumentosFirmados');

        expect(misInversionesDocumentosCtrl.actualizarDocumentosPendientes).not.toHaveBeenCalled();
        expect(misInversionesDocumentosCtrl.actualizarDocumentosFirmados).not.toHaveBeenCalled();
    });

    it('debería obtener input de documentos pendientes y llamar a metodo getDocumentosPendientes().', function () {
        spyOn(misInversionesDocumentosCtrl, 'getDocumentosPendientes');
        misInversionesDocumentosCtrl.actualizarDocumentosPendientes();

        expect(misInversionesDocumentosCtrl.documentosPendientesInput.rut).toBe('12345656');
        expect(misInversionesDocumentosCtrl.getDocumentosPendientes).toHaveBeenCalled();
    });

    it('debería obtener input de documentos firmados y llamar a metodo getDocumentosFirmados().', function () {
        spyOn(misInversionesDocumentosCtrl, 'getDocumentosFirmados');
        misInversionesDocumentosCtrl.actualizarDocumentosFirmados();

        expect(misInversionesDocumentosCtrl.documentosFirmadosInput.rut).toBe('12345656');
        expect(misInversionesDocumentosCtrl.documentosFirmadosInput.fechaIni).toMatch(/^[A-Z][a-z][a-z]\s(\d{1})?\d{1}\,\s\d{4}$/);
        expect(misInversionesDocumentosCtrl.documentosFirmadosInput.fechaFin).toMatch(/^[A-Z][a-z][a-z]\s(\d{1})?\d{1}\,\s\d{4}$/);
        expect(misInversionesDocumentosCtrl.getDocumentosFirmados).toHaveBeenCalled();
    });

    it('debería seleccionar todas las operaciones pendientes.', function () {
        misInversionesDocumentosCtrl.todasOperaciones = true;
        misInversionesDocumentosCtrl.operacionesPendientes = documentosFirmados_stub['operaciones'];
        misInversionesDocumentosCtrl.toggleTodasOperaciones();

        expect(misInversionesDocumentosCtrl.operacionesPendientes[0].Seleccionado).toBe(true);
        expect(misInversionesDocumentosCtrl.operacionesPendientes[1].Seleccionado).toBe(true);
        expect(misInversionesDocumentosCtrl.operacionesPendientes[2].Seleccionado).toBe(true);
    });

    it('debería deseleccionar todas las operaciones pendientes.', function () {
        misInversionesDocumentosCtrl.todasOperaciones = false;
        misInversionesDocumentosCtrl.operacionesPendientes = documentosFirmados_stub['operaciones'];
        misInversionesDocumentosCtrl.toggleTodasOperaciones();

        expect(misInversionesDocumentosCtrl.operacionesPendientes[0].Seleccionado).toBe(false);
        expect(misInversionesDocumentosCtrl.operacionesPendientes[1].Seleccionado).toBe(false);
        expect(misInversionesDocumentosCtrl.operacionesPendientes[2].Seleccionado).toBe(false);
    });

    it('debería seleccionar todos los documentos pendientes.', function () {
        misInversionesDocumentosCtrl.todosDocumentos = true;
        misInversionesDocumentosCtrl.documentosPendientes = documentosFirmados_stub['documentos'];
        misInversionesDocumentosCtrl.toggleTodosDocumentos();

        expect(misInversionesDocumentosCtrl.documentosPendientes[0].Seleccionado).toBe(true);
        expect(misInversionesDocumentosCtrl.documentosPendientes[1].Seleccionado).toBe(true);
    });

    it('debería deseleccionar todos los documentos pendientes.', function () {
        misInversionesDocumentosCtrl.todosDocumentos = false;
        misInversionesDocumentosCtrl.documentosPendientes = documentosFirmados_stub['documentos'];
        misInversionesDocumentosCtrl.toggleTodosDocumentos();

        expect(misInversionesDocumentosCtrl.documentosPendientes[0].Seleccionado).toBe(false);
        expect(misInversionesDocumentosCtrl.documentosPendientes[1].Seleccionado).toBe(false);
    });

    it('debería desactivar check todas las operaciones', function () {
        misInversionesDocumentosCtrl.todasOperaciones = true;
        misInversionesDocumentosCtrl.operacionesPendientes = documentosFirmados_stub['operaciones'];
        misInversionesDocumentosCtrl.opcionOperacionToggled();

        expect(misInversionesDocumentosCtrl.todasOperaciones).toBe(false);
    });

    it('debería desactivar check todos los documentos', function () {
        misInversionesDocumentosCtrl.todosDocumentos = true;
        misInversionesDocumentosCtrl.documentosPendientes = documentosFirmados_stub['documentos'];
        misInversionesDocumentosCtrl.opcionDocumentoToggled();

        expect(misInversionesDocumentosCtrl.todosDocumentos).toBe(false);
    });

    it('debería abrir modal de confirmación.', function () {
        misInversionesDocumentosCtrl.confirmacion();

        expect($uibModal.open).toHaveBeenCalled();
    });

    it('debería validar fechas. Fechas de DESDE y HASTA válidas.', function () {
        misInversionesDocumentosCtrl.validarFechas();

        expect(misInversionesDocumentosCtrl.errorFechas).toBe(null);
    });

    it('debería validar fechas. Fecha DESDE es indefinida.', function () {
        misInversionesDocumentosCtrl.fechaFirmadosInicio = undefined;
        misInversionesDocumentosCtrl.validarFechas();

        expect(misInversionesDocumentosCtrl.errorFechas).toBe('La fecha "desde" es inválida.');
    });

    it('debería validar fechas. Fecha de HASTA es indefinida.', function () {
        misInversionesDocumentosCtrl.fechaFirmadosFin = undefined;
        misInversionesDocumentosCtrl.validarFechas();

        expect(misInversionesDocumentosCtrl.errorFechas).toBe('La fecha "hasta" es inválida.');
    });

    it('debería validar fechas. Fechas de DESDE y HASTA indefinidas.', function () {
        misInversionesDocumentosCtrl.fechaFirmadosInicio = undefined;
        misInversionesDocumentosCtrl.fechaFirmadosFin = undefined;
        misInversionesDocumentosCtrl.validarFechas();

        expect(misInversionesDocumentosCtrl.errorFechas).toBe('La fecha "desde" y la fecha "hasta" son inválidas.');
    });

    it('debería validar fechas. Fecha de DESDE mayor a fecha de HASTA.', function () {
        misInversionesDocumentosCtrl.fechaFirmadosInicio = new Date("2016-03-23");
        misInversionesDocumentosCtrl.fechaFirmadosFin = new Date("2016-03-22");
        misInversionesDocumentosCtrl.validarFechas();

        expect(misInversionesDocumentosCtrl.errorFechas).toBe('La fecha "desde" es mayor a la fecha "hasta".');
    });
});