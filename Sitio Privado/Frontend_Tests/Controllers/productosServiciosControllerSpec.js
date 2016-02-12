describe('productosServiciosCtrl - ', function () {
    beforeEach(function () {
        module('tannerPrivadoApp');
    });

    var $q, $rootScope,
        constantService, dataService, authService, extrasService, $anchorScroll,
        get_deferred, getSingle_deferred, postWebService_deferred,
        productosServiciosCtrl;

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

    beforeEach(inject(function (_$q_, _$rootScope_, _constantService_, _dataService_, _extrasService_, _$anchorScroll_) {
        $q = _$q_;
        $rootScope = _$rootScope_;
        constantService = _constantService_;
        dataService = _dataService_;
        extrasService = _extrasService_;
        $anchorScroll = _$anchorScroll_;

        getSingle_deferred = $q.defer();
        postWebService_deferred = $q.defer();
        get_deferred = $q.defer();

        spyOn(dataService, 'get').and.returnValue(get_deferred.promise);
        spyOn(dataService, 'getSingle').and.returnValue(getSingle_deferred.promise);
        spyOn(dataService, 'postWebService').and.returnValue(postWebService_deferred.promise);
    }));

    beforeEach(inject(function (_authService_) {
        getSingle_deferred.resolve(usuario_stub);
        $rootScope.$digest();
        authService = _authService_;
    }));

    beforeEach(inject(function (_$controller_) {
        productosServiciosCtrl = _$controller_("ProductosServiciosCtrl", {
            $rootScope: $rootScope,
            constantService: constantService,
            dataService: dataService,
            authService: authService,
            extrasService: extrasService,
            $anchorScroll: $anchorScroll
        });
    }));

    it('Obtener categorias Cliente', function () {
        var categoriaClienteInput_stub = { rut_cli: usuario_stub.Rut };
        var categoriaCliente_stub = {
            Identificador: 1,
            Descriptor: 'Descriptor',
            Comentario: '',
            Productos: null
        };  

        spyOn(productosServiciosCtrl, 'getProductos');

        productosServiciosCtrl.getCategoriaCliente(categoriaClienteInput_stub);
        postWebService_deferred.resolve(categoriaCliente_stub);
        $rootScope.$digest();

        expect(productosServiciosCtrl.categoriaCliente).toEqual(categoriaCliente_stub);
        expect(productosServiciosCtrl.getProductos).toHaveBeenCalled();
    });

    it('getProductos', function () {

        var categoria_stub = [
            {
                Identificador: 1,
                Descriptor: 'Descriptor 1',
                Comentario: '',
                Productos: null
            },
            {
                Identificador: 2,
                Descriptor: 'Descriptor 2',
                Comentario: '',
                Productos: null
            }
        ];

        var productos_stub = [
            {
                Identificador: 1,
                Descriptor: 'Decriptor producto 1',
                Comentario: 'Comentario producto 1',
                Riesgo: 'Bajo',
                Categorias: []
            },
            {
                Identificador: 3,
                Descriptor: 'Decriptor producto 3',
                Comentario: 'Comentario producto 3',
                Riesgo: 'Alto',
                Categorias: []
            },
            {
                Identificador: 4,
                Descriptor: 'Decriptor producto 4',
                Comentario: 'Comentario producto 4',
                Riesgo: 'Medio',
                Categorias: categoria_stub
            }
        ];            
        var categoriaCliente_stub = {
            Identificador: 1,
            Descriptor: 'Descriptor 1',
            Comentario: '',
            Productos: []
        };
        
        productosServiciosCtrl.categoriaCliente = categoriaCliente_stub;

        productosServiciosCtrl.getProductos();
        get_deferred.resolve(productos_stub);
        $rootScope.$digest();

        expect(productosServiciosCtrl.productos).toEqual([productos_stub[2]]);
    });

    it('Realizar scroll automatico.', function () {
        var id_stub = 'div_container_a';

        productosServiciosCtrl.scrollTo(id_stub);
        
        expect(productosServiciosCtrl.$anchorScroll).toBeDefined();
    });

    it('Setear imagenes.', function () {
        productosServiciosCtrl.setImagenes();

        expect(productosServiciosCtrl.imagenProducto['1']).toBe('moneda-ext');
        expect(productosServiciosCtrl.imagenProducto['2']).toBe('irf');
        expect(productosServiciosCtrl.imagenProducto['3']).toBe('distribucion-fm');
        expect(productosServiciosCtrl.imagenProducto['5']).toBe('pactos');
        expect(productosServiciosCtrl.imagenProducto['6']).toBe('operaciones-prestamo');
        expect(productosServiciosCtrl.imagenProducto['7']).toBe('operaciones-simultaneas');
        expect(productosServiciosCtrl.imagenProducto['8']).toBe('renta-fija-inter');
        expect(productosServiciosCtrl.imagenProducto['9']).toBe('operaciones-simultaneas');
        expect(productosServiciosCtrl.imagenProducto['10']).toBe('operaciones-prestamo');
        expect(productosServiciosCtrl.imagenProducto['11']).toBe('moneda-ext');
        expect(productosServiciosCtrl.imagenProducto['12']).toBe('renta-internacional');
        expect(productosServiciosCtrl.imagenProducto['13']).toBe('admin-cartera');
    });
});