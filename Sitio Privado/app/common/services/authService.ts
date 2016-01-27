module app.common.services {

    interface IAuth {
        autenticado: boolean;
        usuario: app.domain.IUsuario;
        cerrarSesion(): void;
    }

    export class AuthService implements IAuth {
        autenticado: boolean;
        usuario: app.domain.IUsuario;
                
        static $inject = ['constantService', 'dataService', 'extrasService'];
        constructor(private constantService: ConstantService,
            private dataService: DataService,
            private extrasService: ExtrasService) {
            this.getUsuarioActual();
        }

        getUsuarioActual(): void {
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

        cerrarSesion(): void {
            this.autenticado = false;
            this.usuario = null;
            this.extrasService.abrirRuta(this.constantService.mvcSignOutURI, "_self");
        }
    }

    angular.module('tannerPrivadoApp')
        .service('authService', AuthService);
}