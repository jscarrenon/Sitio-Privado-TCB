module app.common.services {

    interface IExtras {
        getRutParteEntera(rut: string): string;
        abrirRuta(ruta: string): void;
    }

    export class ExtrasService implements IExtras {
    
        static $inject = ['$window'];
        constructor(private $window: ng.IWindowService) {
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
    }

    angular.module('tannerPrivadoApp')
        .service('extrasService', ExtrasService);
}