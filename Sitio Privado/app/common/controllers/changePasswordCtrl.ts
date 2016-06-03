module app.common.controllers {

    interface IChangePasswordViewModel {
        changePasswordInput: app.domain.IChangePasswordInput;
        changePassword(): void;
        loading: boolean;
    }

    export class ChangePasswordCtrl implements IChangePasswordViewModel {

        changePasswordInput: app.domain.IChangePasswordInput;
        loading: boolean;

        static $inject = ['constantService', 'dataService', 'authService'];
        constructor(private constantService: app.common.services.ConstantService,
            private dataService: app.common.services.DataService,
            private authService: app.common.services.AuthService) {

            this.changePasswordInput = new app.domain.ChangePasswordInput();
        }

        changePassword(): void {
            this.loading = true;
            this.dataService.postWebService(this.constantService.apiUsersURI + 'changePassword', this.changePasswordInput)
                .then((result: any) => {
                    console.log(result);
                })
                .finally(() => this.loading = false);
        }
    }

    angular.module('tannerPrivadoApp')
        .controller('ChangePasswordCtrl', ChangePasswordCtrl);
}