module app.common.services {

    interface IConstant {
        apiPostURI: string;
        mvcHomeURI: string;
        templateFooterURI: string;
    }

    export class ConstantService implements IConstant {
        apiPostURI: string;
        mvcHomeURI: string;
        templateFooterURI: string;

        constructor() {
            this.apiPostURI = '/api/posts/';
            this.mvcHomeURI = '/Home/';
            this.templateFooterURI = 'app/common/templates/footer.html';
        }
    }

    angular.module('tannerPrivadoApp')
        .service('constantService', ConstantService);
}