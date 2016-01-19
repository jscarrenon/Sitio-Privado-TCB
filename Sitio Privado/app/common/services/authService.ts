module app.common.services {

    interface IAuth {
        autenticado: boolean;
        usuario: app.domain.IUsuario;
    }

    export class AuthService implements IAuth {
        autenticado: boolean;
        usuario: app.domain.IUsuario;
                
        static $inject = ['constantService', 'dataService'];
        constructor(private constantService: ConstantService,
            private dataService: DataService) {
            this.getUsuarioActual();
        }

        getUsuarioActual(): void {
            console.log("getUsuarioActual");
            this.dataService.getSingle(this.constantService.mvcHomeURI + 'GetUsuarioActual').then((result: app.domain.IUsuario) => {
                this.usuario = result;
                if (this.usuario.Autenticado) {
                    this.autenticado = true;
                }
                else {
                    this.autenticado = false;
                }
            });
        }
    }

    angular.module('tannerPrivadoApp')
        .service('authService', AuthService);
}