module app.common.services {

    interface IExtras {
        getRutParteEntera(rut: string): string;
        abrirRuta(ruta: string, target: string): void;
    }

    export class ExtrasService implements IExtras {
    
        static $inject = ['$window','$filter'];
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

        abrirRuta(ruta: string, target: string = "_blank"): void {
            this.$window.open(ruta, target);
        }

        //Aquí debería usarse filtro de angular - KUNDER
        getFechaFormato(fecha: Date): string {
            var yyyy = fecha.getFullYear().toString();
            var mm = (fecha.getMonth() + 1).toString(); // getMonth() is zero-based
            var dd = fecha.getDate().toString();
            return (dd[1] ? dd : "0" + dd[0]) + "-" + (mm[1] ? mm : "0" + mm[0]) + "-" + yyyy; // padding
        }
    }

    angular.module('tannerPrivadoApp')
        .service('extrasService', ExtrasService);
}