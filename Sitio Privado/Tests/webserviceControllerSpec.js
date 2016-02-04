describe("[FrontEnd] WebserviceCtrl Unit Tests", function () {
    
    beforeEach(function () {
        module("tannerPrivadoApp");
    });

    describe("Método/s agente.", function () {

        var $rootScope, $rootScope,
            postWebservice_deferred, // deferred used for promises
            constantService, dataService, // controller dependencies
            webserviceController; // the controller

        beforeEach(inject(function (_$controller_, _$rootScope_, _$q_, _constantService_, _dataService_) {

            $q = _$q_;
            $rootScope = _$rootScope_;
            constantService = _constantService_;
            dataService = _dataService_;

            postWebservice_deferred = $q.defer();
            spyOn(constantService, 'apiAgenteURI').and.returnValue('/api/agente/');
            spyOn(dataService, 'postWebService').and.returnValue(postWebservice_deferred.promise);

            webserviceController = _$controller_("WebserviceCtrl", {
                $rootScope: _$rootScope_,
                    constantService: _constantService_,
                    dataService: _dataService_
            });
        }));

        var agenteInput_stub = {
            _rut : "18948509-3",
            _sec: 0
        };

        var agente_stub = {
            Codigo: 837,
            Nombre: "Agente de prueba",
            Sucursal: "Sucursal de prueba",
            PathImg: "//agentes/img_agente_prueba.png",
            Email: "aprueba@tanner.cl",
            Telefono: "099990948",
            FechaInicioAcreditacion: "28 Noviembre 2014",
            FechaExpiracionAcreditacion: "28 Noviembre 2016",
            Descriptor: "Descriptor agente de prueba"
        };

        it("getAgente() - Obtención correcta de la información del agente.", function () {

            webserviceController.getAgente(agenteInput_stub);
            postWebservice_deferred.resolve(agente_stub);
            $rootScope.$digest();

                //Esperamos que la información del agente sea correcta.
            expect(webserviceController.agente).toBe(agente_stub);

       });
    });

    describe("Método/s de categorias.", function () {
 
        var $rootScope, $rootScope,
            postWebservice_deferred, get_deferred,// deferred used for promises
            constantService, dataService, // controller dependencies
            webserviceController; // the controller

        beforeEach(inject(function (_$controller_, _$rootScope_, _$q_, _constantService_, _dataService_) {

            $q = _$q_;
            $rootScope = _$rootScope_;
            constantService = _constantService_;
            dataService = _dataService_;

            postWebservice_deferred = $q.defer();
            get_deferred = $q.defer();
            spyOn(constantService, 'apiCategoriaURI').and.returnValue('apiCategoriaURI');
            spyOn(dataService, 'postWebService').and.returnValue(postWebservice_deferred.promise);
            spyOn(dataService, 'get').and.returnValue(get_deferred.promise);

            webserviceController = _$controller_("WebserviceCtrl", {
                $rootScope: _$rootScope_,
                constantService: _constantService_,
                dataService: _dataService_
            });
        }));
        
        var categoriaInput_stub = {
                ident_cat: 3
        };

        var categoriaClienteInput_stub = {
                rut_cli: "18948509-3"
        };

        var categorias_stub =[{
                    Identificador: 1,
                    Descriptor: "Descriptor categoria 1",
                    Comentario: "Comentario categoria 1",
                    Productos: [{
                        Identificador: 3,
                        Descriptor: "Producto id 3",
                        Categorias: null
                    }]
                },
                {
                    Identificador: 2,
                    Descriptor: "Descriptor categoria 2",
                    Comentario: "Comentario categoria 2",
                    Productos: [{
                        Identificador: 544,
                        Descriptor: "Producto id 544",
                        Categorias: null
                    }]
                },
                {
                    Identificador: 3,
                    Descriptor: "Descriptor categoria 3",
                    Comentario: "Comentario categoria 3",
                    Productos: [{
                        Identificador: 34,
                        Descriptor: "Producto id 34",
                        Categorias: null
                    }]
                }
            ];

        it("getCategoria() - Obtención correcta de una categoria.", function () {

            webserviceController.getCategoria(categoriaInput_stub.ident_cat);

            postWebservice_deferred.resolve(categorias_stub[2]);
            $rootScope.$digest();

            //Esperamos que la información del agente sea correcta.
            expect(webserviceController.categoria.Identificador).toBe(categoriaInput_stub.ident_cat);
            expect(webserviceController.categoria).toBe(categorias_stub[2]);

        });

        it("getCategorias() - Obtención correcta de lista de categorias.", function () {

            webserviceController.getCategorias();

            get_deferred.resolve(categorias_stub);
            $rootScope.$digest();

            //Esperamos que la información del agente sea correcta.
            expect(webserviceController.categorias.length).toBe(categorias_stub.length);
            expect(webserviceController.categorias[0]).toBe(categorias_stub[0]);
            expect(webserviceController.categorias[1]).toBe(categorias_stub[1]);
            expect(webserviceController.categorias[2]).toBe(categorias_stub[2]);

        });

        it("getCategoriaCliente() - Obtención correcta de la categoria de un cliente.", function () {

            webserviceController.getCategoriaCliente(categoriaClienteInput_stub.rut_cli);

            postWebservice_deferred.resolve(categorias_stub[0]);
            $rootScope.$digest();        

            //Esperamos que la información del agente sea correcta.
            expect(webserviceController.categoriaCliente).toBe(categorias_stub[0]);

        });
    });

    describe("Método/s de productos", function () {

        var $rootScope, $rootScope,
            postWebservice_deferred, get_deferred,// deferred used for promises
            constantService, dataService, // controller dependencies
            webserviceController; // the controller

        beforeEach(inject(function (_$controller_, _$rootScope_, _$q_, _constantService_, _dataService_) {

            $q = _$q_;
            $rootScope = _$rootScope_;
            constantService = _constantService_;
            dataService = _dataService_;

            postWebservice_deferred = $q.defer();
            get_deferred = $q.defer();
            spyOn(constantService, 'apiCategoriaURI').and.returnValue('apiCategoriaURI');
            spyOn(dataService, 'postWebService').and.returnValue(postWebservice_deferred.promise);
            spyOn(dataService, 'get').and.returnValue(get_deferred.promise);

            webserviceController = _$controller_("WebserviceCtrl", {
                $rootScope: _$rootScope_,
                constantService: _constantService_,
                dataService: _dataService_
            });
        }));

        var productoInput_stub =
        {
            ident_prd: 34
        };

        var productos_stub = [{
                    Identificador: 34,
                    Descriptor: "Producto id 34",
                    Categorias: [{ 
                        Identificador: 5,
                        Descriptor: "Descriptor categoria 1",
                        Comentario: "Comentario categoria 1",
                        Productos: null 
                    }]
                },
                {
                    Identificador: 334,
                    Descriptor: "Producto id 34",
                    Categorias: [{ 
                            Identificador: 3,
                        Descriptor: "Descriptor categoria 2",
                        Comentario: "Comentario categoria 2",
                        Productos: null 
                    }]
                },
                {
                    Identificador: 314,
                    Descriptor: "Producto id 34",
                    Categorias: [{ 
                        Identificador: 3,
                        Descriptor: "Descriptor categoria 3",
                        Comentario: "Comentario categoria 3",
                        Productos: null 
                    }]                 
                }
        ];

        it("getProducto() - Obtención correcta de la información del producto.", function () {

            webserviceController.getProducto(productoInput_stub.ident_prd);

            postWebservice_deferred.resolve(productos_stub[0]);
            $rootScope.$digest();

            //Esperamos que la información del agente sea correcta.
            expect(webserviceController.producto).toBe(productos_stub[0]);

        });

        it("getProductos() - Obtención correcta de lista de productos.", function () {

            webserviceController.getProductos();

            get_deferred.resolve(productos_stub);
            $rootScope.$digest();

                //Esperamos que la información del agente sea correcta.
            expect(webserviceController.productos).toBe(productos_stub);

        });
    });

    describe("Método/s de cartola.", function() {

        var $rootScope, $rootScope,
            postWebservice_deferred, get_deferred,// deferred used for promises
            constantService, dataService, // controller dependencies
            webserviceController; // the controller

        beforeEach(inject(function (_$controller_, _$rootScope_, _$q_, _constantService_, _dataService_) {

            $q = _$q_;
            $rootScope = _$rootScope_;
            constantService = _constantService_;
            dataService = _dataService_;

            postWebservice_deferred = $q.defer();
            get_deferred = $q.defer();
            spyOn(constantService, 'apiCategoriaURI').and.returnValue('apiCategoriaURI');
            spyOn(dataService, 'postWebService').and.returnValue(postWebservice_deferred.promise);
            spyOn(dataService, 'get').and.returnValue(get_deferred.promise);

            webserviceController = _$controller_("WebserviceCtrl", {
                $rootScope: _$rootScope_,
                constantService: _constantService_,
                dataService: _dataService_
            });
        }));

        var cartolaInput_stub = {
            _rut: "12345634-8",
            _secuencia: 1235678
        };

        var cartola_stub =
        {
            Rut: "12345634-8",
            Periodo: "24/02/2014 24/02/2016",
            SaldoCaja: 1243235,
            SaldoCajaPorcentaje: 80,
            TotalRentaFija: 5678854,
            TotalRentaFijaPorcentaje: 3445,
            InstrumentosRentaFija: 234,
            InstrumentosRentaFijaPorcentaje: 3223,
            FondosMutuosRentaFija: 432342,
            FondosMutuosRentaFijaPorcentaje: 167823,
            TotalRentaVariable: 43212,
            TotalRentaVariablePorcentaje: 167823,
            AccionesNacionales: 45671,
            AccionesNacionalesPorcentaje: 8905,
            FondosMutuosRentaVariable: 456890,
            FondosMutuosRentaVariablePorcentaje: 4722311,
            ForwardCompra: 457457,
            ForwardCompraPorcentaje: 2168,
            ForwardVenta: 234679,
            ForwardVentaPorcentaje: 9762,
            TotalInversiones: 34268,
            TotalInversionesPorcentaje: 5680943
        };

        it("getCartola() - Obtención correcta de la información de la cartola.", function () {

            webserviceController.getCartola(cartolaInput_stub.ident_prd);

            postWebservice_deferred.resolve(cartola_stub);
            $rootScope.$digest();

            //Esperamos que la información del agente sea correcta.
            expect(webserviceController.cartola).toBe(cartola_stub);
        });
    });
});