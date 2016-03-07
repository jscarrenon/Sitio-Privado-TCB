module app.common.services {

    interface IExtras {
        getRutParteEntera(rut: string): string;
        abrirRuta(ruta: string, target: string): void;
		getFechaFormato(fecha: Date, formato: string): string;
    }

    export class ExtrasService implements IExtras {
    
        static $inject = ['$window','$filter'];
        constructor(private $window: ng.IWindowService,
            private $filter: ng.IFilterService) {
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

        //Uso de filtro angular para fecha. IMPORTANTE: usa valores de angular-locale_es-cl.js
        getFechaFormato(fecha: Date, formato: string = "dd-mm-aaaa"): string {
            if (formato == "dd/mm/aaaa") {
                var yyyy = fecha.getFullYear().toString();
                var mm = (fecha.getMonth() + 1).toString(); // getMonth() is zero-based
                var dd = fecha.getDate().toString();

                return (dd[1] ? dd : "0" + dd[0]) + "/" + (mm[1] ? mm : "0" + mm[0]) + "/" + yyyy;
            }
            else if (formato == "longDate") {
                return this.$filter('date')(fecha, formato);
            }
            else {
                return this.$filter('date')(fecha, "mediumDate");
            }
        }
    }

    angular.module('tannerPrivadoApp')
        .service('extrasService', ExtrasService);
}