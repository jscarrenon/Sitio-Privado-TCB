module app.common.services {

    interface IConstant {
        buildFolderURI: string;
        mvcHomeURI: string;
        mvcSignOutURI: string;
        templateFooterURI: string;
        templatePaginationURI: string;
        templateLoadingURI: string;
        templateCircularizacionModalURI: string;
        templateDocumentosPendientesModalURI: string;
        templateDocumentosConfirmacionModalURI: string;
        templateDocumentosRespuestaModalURI: string;
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
        templateSusConfFirmaElecDocModalURI: string;
        apiUsersURI: string;
        homeTanner: string;
        apiAutenticacionURI: string;
        apiOAuthURI: string;
        userClientId: string;
        userClientSecret: string;
        tannerAuthenticationAPI: string;
    }

    export class ConstantService implements IConstant {
        buildFolderURI: string;
        mvcHomeURI: string;
        mvcSignOutURI: string;
        templateFooterURI: string;
        templatePaginationURI: string;
        templateLoadingURI: string;
        templateCircularizacionModalURI: string;
        templateDocumentosPendientesModalURI: string;
        templateDocumentosConfirmacionModalURI: string;
        templateDocumentosRespuestaModalURI: string;
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
        templateSusConfFirmaElecDocModalURI: string;
        apiUsersURI: string;
        homeTanner: string;
        apiAutenticacionURI: string;
        apiOAuthURI: string;
        userClientId: string;
        userClientSecret: string;
        tannerAuthenticationAPI: string;

        constructor() {
            this.buildFolderURI = '.build/';
            this.mvcHomeURI = '/Home/';
            this.mvcSignOutURI = '/Authentication/signout';

            this.templateFooterURI = this.buildFolderURI + 'html/common/templates/footer.html';
            this.templatePaginationURI = this.buildFolderURI + 'html/common/templates/pagination.html';
            this.templateLoadingURI = this.buildFolderURI + 'html/common/templates/loading.html';
            this.templateCircularizacionModalURI = this.buildFolderURI + 'html/modules/mis-inversiones/templates/circularizacion_pendiente_modal.html';
            this.templateDocumentosPendientesModalURI = this.buildFolderURI + 'html/modules/mis-inversiones/templates/estado-documentos_pendientes_modal.html';
            this.templateDocumentosConfirmacionModalURI = this.buildFolderURI + 'html/modules/mis-inversiones/templates/estado-documentos_confirmacion.html';
            this.templateDocumentosRespuestaModalURI = this.buildFolderURI + 'html/modules/mis-inversiones/templates/estado-documentos_respuesta.html';
            this.templateSusConfFirmaElecDocModalURI = this.buildFolderURI + 'html/modules/mis-inversiones/templates/sus-conf-firma-electronica-docs-modal.html';

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
            this.apiUsersURI = 'api/users/';
            this.apiAutenticacionURI = '/api/authentication/';
            this.homeTanner = app.config.CONFIG.TANNER_PUBLIC_SITE_URL;
            this.apiOAuthURI = app.config.CONFIG.OAUTH2_URL;
            this.userClientId = app.config.CONFIG.CLIENT_ID;
            this.userClientSecret = app.config.CONFIG.CLIENT_SECRET;
            this.tannerAuthenticationAPI = app.config.CONFIG.TANNER_AUTHENTICATION_API;

        }
    }

    angular.module('tannerPrivadoApp')
        .service('constantService', ConstantService);
}
