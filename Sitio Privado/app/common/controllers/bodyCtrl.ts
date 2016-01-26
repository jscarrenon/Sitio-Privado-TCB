module App.Common.Controllers {

    interface IBodyViewModel {
        seccionId: number;
        seleccionarSeccion(id: number): void;
    }

    export class BodyCtrl implements IBodyViewModel {

        seccionId: number;

        static $inject = ['constantService', 'dataService', 'authService'];
        constructor(private constantService: App.Common.Services.ConstantService,
            private dataService: App.Common.Services.DataService,
            private authService: App.Common.Services.AuthService) {

            this.seccionId = 0;
            this.seleccionarSeccion(this.seccionId);
        }

        seleccionarSeccion(id: number): void {
            this.seccionId = id;
        }

        private static _module: ng.IModule;
        public static get module(): ng.IModule {
            if (this._module) {
                return this._module;
            }
            this._module = angular.module('bodyCtrl', []);
            this._module.controller('bodyCtrl', BodyCtrl);
            return this._module;
        }
    }
}