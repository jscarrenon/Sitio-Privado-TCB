module app.common.services {

    interface IConstant {
        mvcHomeURI: string;
        templateFooterURI: string;
        apiAgenteURI: string;
    }

    export class ConstantService implements IConstant {
        mvcHomeURI: string;
        templateFooterURI: string;
        apiAgenteURI: string;

        constructor() {
            this.mvcHomeURI = '/Home/';
            this.templateFooterURI = 'app/common/templates/footer.html';
            this.apiAgenteURI = '/api/agente/';
        }
    }

    angular.module('tannerPrivadoApp')
        .service('constantService', ConstantService);
}