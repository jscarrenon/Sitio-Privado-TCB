module App.Common.Services {

    interface IAuth {
        autenticado: boolean;
        usuario: App.Domain.IUsuario;
    }

    export class AuthService implements IAuth {
        autenticado: boolean;
        usuario: App.Domain.IUsuario;
                
        static $inject = ['constantService', 'dataService'];
        constructor(private constantService: ConstantService,
            private dataService: DataService) {
            this.getUsuarioActual();
        }

        getUsuarioActual(): void {
            this.dataService.getSingle(this.constantService.mvcHomeURI + 'GetUsuarioActual').then((result: App.Domain.IUsuario) => {
                this.usuario = result;
                if (this.usuario.Autenticado) {
                    this.autenticado = true;
                }
                else {
                    this.autenticado = false;
                }
            });
        }

        private static _module: ng.IModule;
        public static get module(): ng.IModule {
            if (this._module) {
                return this._module;
            }
            this._module = angular.module('authService', []);
            this._module.service('authService', AuthService);
            return this._module;
        }
    }
}