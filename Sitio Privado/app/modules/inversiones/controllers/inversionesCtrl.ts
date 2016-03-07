module app.inversiones {

    interface IProductosServiciosViewModel {
        categorias: app.domain.ICategoria[];
        getCategorias(): void;
        loading: boolean;
    }

    class InversionesCtrl implements IProductosServiciosViewModel{

        categorias: app.domain.ICategoria[];
        imagenes: {};
        loading: boolean;

        static $inject = ['constantService', 'dataService'];
        constructor(private constantService: app.common.services.ConstantService,
            private dataService: app.common.services.DataService) {

            this.getCategorias();
        }

        getCategorias(): void {
            this.loading = true;
            this.dataService.get(this.constantService.apiCategoriaURI + 'getList')
                .then((result: app.domain.ICategoria[]) => {
                    this.categorias = result.filter((value: domain.ICategoria) => {
                        return value.Descriptor == 'Conservador' || value.Descriptor == 'Moderado' || value.Descriptor == 'Agresivo';
                    });
                    this.categorias = this.categorias.sort((a: domain.ICategoria, b: domain.ICategoria) => {
                        return a.Identificador - b.Identificador;
                    });
                })
                .finally(() => this.loading = false);
        }
    }
    angular.module('tannerPrivadoApp')
        .controller('InversionesCtrl', InversionesCtrl);
}