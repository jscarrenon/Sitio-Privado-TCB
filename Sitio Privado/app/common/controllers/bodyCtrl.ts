module app.common.controllers {

    interface IBodyViewModel {
        seccionId: number;
        seleccionarSeccion(id: number): void;
    }

    export class BodyCtrl implements IBodyViewModel {

        seccionId: number;

        static $inject = ['constantService', 'dataService', 'authService'];
        constructor(private constantService: app.common.services.ConstantService,
            private dataService: app.common.services.DataService,
            private authService: app.common.services.AuthService) {

            this.seccionId = 0;
            this.seleccionarSeccion(this.seccionId);


            setTimeout(function () {
                uglipop({
                    class: 'modal-style modal1',
                    source: 'html',
                    content: '<div class="pretitle">Pendiente</div><div class="title">Circularización Anual de Custodia 2015</div><div class="text">Estimado Cliente,<br/> En conformidad a lo dispuesto en la Circular 1962 de la Superintendencia de Valores y Seguros (SVS), solicitamos a usted revisar los informes de saldos, que de acuerdo a nuestros registros se encuentran depositados a su nombre en custodia y/o garantía de Tanner Corredores de Bolsa S.A. al día 30 de Junio de 2015.</div><div class="button green"><a href="#" class="clink">Continuar</a></div><button class="modal-close" onclick="rem();"></button>'
                });
            }, 100);
        }

        seleccionarSeccion(id: number): void {
            this.seccionId = id;
        }
    }

    angular.module('tannerPrivadoApp')
        .controller('BodyCtrl', BodyCtrl);
}