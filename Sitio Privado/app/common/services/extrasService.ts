module app.common.services {

    interface IExtras {
        getRutParteEntera(rut :string): string;
    }

    export class ExtrasService implements IExtras {
                
        constructor() {
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
    }

    angular.module('tannerPrivadoApp')
        .service('extrasService', ExtrasService);
}