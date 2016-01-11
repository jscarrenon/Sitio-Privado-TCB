module app.common.services {

    interface IConstant {
        apiPostURI: string;
        mvcHomeURI: string;
    }

    export class ConstantService implements IConstant {
        apiPostURI: string;
        mvcHomeURI: string;

        constructor() {
            this.apiPostURI = '/api/posts/';
            this.mvcHomeURI = '/Home/';
        }
    }

    angular.module('tannerPrivadoApp')
        .service('constantService', ConstantService);
}