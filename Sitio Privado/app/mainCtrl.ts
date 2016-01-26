module App.MainController {
    'use strict'

    export interface IMainAppCtrl {
    }

    export class MainAppCtrl implements IMainAppCtrl {
        constructor() { }

        //#region 'Definicion modulo angular'
        private static _module: ng.IModule;
        public static get module(): ng.IModule {
            if (this._module) {
                return this._module;
            }
            this._module = angular.module('mainCtrl', []);
            this._module.controller('mainCtrl', MainAppCtrl);
            return this._module;
        }
        //#endregion
    }
}