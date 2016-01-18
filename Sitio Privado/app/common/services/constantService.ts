module app.common.services {

    interface IConstant {
        mvcHomeURI: string;
        templateFooterURI: string;
        apiAgenteURI: string;
        apiCategoriaURI: string;
    }

    export class ConstantService implements IConstant {
        mvcHomeURI: string;
        templateFooterURI: string;
        apiAgenteURI: string;
        apiCategoriaURI: string;

        constructor() {
            this.mvcHomeURI = '/Home/';
            this.templateFooterURI = 'app/common/templates/footer.html';
            this.apiAgenteURI = '/api/agente/';
            this.apiCategoriaURI = '/api/categoria/';
        }
    }

    angular.module('tannerPrivadoApp')
        .service('constantService', ConstantService);
}