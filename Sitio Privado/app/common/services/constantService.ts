module app.common.services {

    interface IConstant {
        mvcHomeURI: string;
        templateFooterURI: string;
    }

    export class ConstantService implements IConstant {
        mvcHomeURI: string;
        templateFooterURI: string;

        constructor() {
            this.mvcHomeURI = '/Home/';
            this.templateFooterURI = 'app/common/templates/footer.html';
        }
    }

    angular.module('tannerPrivadoApp')
        .service('constantService', ConstantService);
}