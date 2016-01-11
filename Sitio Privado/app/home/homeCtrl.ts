module app.home {

    interface IHomeViewModel {
        usuario: app.domain.IUsuario;
    }

    class HomeCtrl implements IHomeViewModel {

        usuario: app.domain.IUsuario;

        static $inject = ['constantService', 'dataService'];
        constructor(private constantService: app.common.services.ConstantService,
            private dataService: app.common.services.DataService) {
            this.getUsuarioActual();
        }

        getUsuarioActual(): void {
            this.dataService.getSingle(this.constantService.mvcHomeURI + 'GetUsuarioActual').then((result: app.domain.IUsuario) => {
                this.usuario = result;
            });
        }
    }
    angular.module('tannerPrivadoApp')
        .controller('HomeCtrl', HomeCtrl);
}