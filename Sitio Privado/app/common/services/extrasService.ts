module App.Common.Services {

    interface IExtras {
        getRutParteEntera(rut: string): string;
        abrirRuta(ruta: string): void;
    }

    export class ExtrasService implements IExtras {

        static $inject = ['$window', '$filter'];
        constructor(private $window: ng.IWindowService,
            private $filter: ng.IFilterDate) {
        }

        getRutParteEntera(rut: string) {
            if (rut) {
                var index: number;
                index = rut.indexOf("-");
                if (index > -1) {
                    return rut.substr(0, index);
                }
            }
            return "";
        }

        abrirRuta(ruta: string): void {
            this.$window.open(ruta);
        }

        private static _module: ng.IModule;
        public static get module(): ng.IModule {
            if (this._module) {
                return this._module;
            }
            this._module = angular.module('extrasService', []);
            this._module.service('extrasService', ExtrasService);
            return this._module;
        }
    }
}