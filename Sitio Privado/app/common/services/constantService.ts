module app.common.services {

    interface IConstant {
        buildFolderURI: string;
        mvcHomeURI: string;
        mvcSignOutURI: string;
        templateFooterURI: string;
        templatePaginationURI: string;
        templateLoadingURI: string;
        apiAgenteURI: string;
        apiFondosMutuosURI: string;
        apiCategoriaURI: string;
        apiProductoURI: string;
        apiBlobsURI: string;
        apiBalanceURI: string;
        apiCartolaURI: string;
        apiDocumentoURI: string;
        apiIndicesURI: string;
        apiCircularizacionURI: string;
    }

    export class ConstantService implements IConstant {
        buildFolderURI: string;
        mvcHomeURI: string;
        mvcSignOutURI: string;
        templateFooterURI: string;
        templatePaginationURI: string;
        templateLoadingURI: string;
        apiAgenteURI: string;
        apiFondosMutuosURI: string;
        apiCategoriaURI: string;
        apiProductoURI: string;
        apiBlobsURI: string;
        apiBalanceURI: string;
        apiCartolaURI: string;
        apiDocumentoURI: string;
        apiIndicesURI: string;
        apiCircularizacionURI: string;

        constructor() {
            this.buildFolderURI = '.build/';
            this.mvcHomeURI = '/Home/';
            this.mvcSignOutURI = '/Account/SignOut';
            this.templateFooterURI = this.buildFolderURI + 'html/common/templates/footer.html';
            this.templatePaginationURI = this.buildFolderURI + 'html/common/templates/pagination.html';
            this.templateLoadingURI = this.buildFolderURI + 'html/common/templates/loading.html';
            this.apiAgenteURI = '/api/agente/';
            this.apiFondosMutuosURI = '/api/fondoMutuo/';
            this.apiCategoriaURI = '/api/categoria/';
            this.apiProductoURI = '/api/producto/';
            this.apiBlobsURI = '/api/containers/'
            this.apiBalanceURI = '/api/balance/';
            this.apiCartolaURI = '/api/cartola/';
            this.apiDocumentoURI = '/api/documento/';
            this.apiIndicesURI = '/api/indices/';
            this.apiCircularizacionURI = '/api/circularizacion/';
        }
    }

    angular.module('tannerPrivadoApp')
        .service('constantService', ConstantService);
}
