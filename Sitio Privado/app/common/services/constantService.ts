module app.common.services {

    interface IConstant {
        mvcHomeURI: string;
        templateFooterURI: string;
        apiAgenteURI: string;
        apiFondosMutuosURI: string;
        apiCategoriaURI: string;
        apiProductoURI: string;
    }

    export class ConstantService implements IConstant {
        mvcHomeURI: string;
        templateFooterURI: string;
        apiAgenteURI: string;
        apiFondosMutuosURI: string;
        apiCategoriaURI: string;
        apiProductoURI: string;

        constructor() {
            this.mvcHomeURI = '/Home/';
            this.templateFooterURI = 'app/common/templates/footer.html';
            this.apiAgenteURI = '/api/agente/';
            this.apiFondosMutuosURI = '/api/fondoMutuo/';
            this.apiCategoriaURI = '/api/categoria/';
            this.apiProductoURI = '/api/producto/';
        }
    }

    angular.module('tannerPrivadoApp')
        .service('constantService', ConstantService);
}