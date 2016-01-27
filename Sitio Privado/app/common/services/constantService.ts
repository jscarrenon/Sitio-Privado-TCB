module app.common.services {

    interface IConstant {
        mvcHomeURI: string;
        templateFooterURI: string;
        apiAgenteURI: string;
        apiFondosMutuosURI: string;
        apiCategoriaURI: string;
        apiProductoURI: string;
        apiBlobsURI: string;
        apiBalanceURI: string;
        apiCartolaURI: string;
        apiDocumentoURI: string;
        apiIndicesURI: string;
    }

    export class ConstantService implements IConstant {
        mvcHomeURI: string;
        templateFooterURI: string;
        apiAgenteURI: string;
        apiFondosMutuosURI: string;
        apiCategoriaURI: string;
        apiProductoURI: string;
        apiBlobsURI: string;
        apiBalanceURI: string;
        apiCartolaURI: string;
        apiDocumentoURI: string;
        apiIndicesURI: string;

        constructor() {
            this.mvcHomeURI = '/Home/';
            this.templateFooterURI = 'app/common/templates/footer.html';
            this.apiAgenteURI = '/api/agente/';
            this.apiFondosMutuosURI = '/api/fondoMutuo/';
            this.apiCategoriaURI = '/api/categoria/';
            this.apiProductoURI = '/api/producto/';
            this.apiBlobsURI = '/api/containers/'
            this.apiBalanceURI = '/api/balance/';
            this.apiCartolaURI = '/api/cartola/';
            this.apiDocumentoURI = '/api/documento/';
            this.apiIndicesURI = '/api/indices/';
        }
    }

    angular.module('tannerPrivadoApp')
        .service('constantService', ConstantService);
}
