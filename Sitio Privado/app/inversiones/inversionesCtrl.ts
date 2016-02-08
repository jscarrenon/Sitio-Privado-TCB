module app.inversiones {

    interface IProductosServiciosViewModel {
        categorias: app.domain.ICategoria[];
        getCategorias(): void;
    }

    class InversionesCtrl implements IProductosServiciosViewModel{

        categorias: app.domain.ICategoria[];
        imagenes: {};

        static $inject = ['constantService', 'dataService'];
        constructor(private constantService: app.common.services.ConstantService,
            private dataService: app.common.services.DataService) {

            this.getCategorias();
        }

        getCategorias(): void {
            this.dataService.get(this.constantService.apiCategoriaURI + 'getList')
                .then((result: app.domain.ICategoria[]) => {
                    this.categorias = result.filter((value: domain.ICategoria) => {
                        return value.Descriptor == 'Conservador' || value.Descriptor == 'Moderado' || value.Descriptor == 'Agresivo';
                    });
                });
        }

        setImagenes(): void {
            this.imagenes['Conservador'] = 'conservador';
            this.imagenes['Moderado'] = 'moderado';
            this.imagenes['Agresivo'] = 'agresivo';
        }
        
    }
    angular.module('tannerPrivadoApp')
        .controller('InversionesCtrl', InversionesCtrl);
}