module app.common.controllers {

    interface IChangePasswordViewModel {
        changePasswordInput: app.domain.IChangePasswordInput;
        changePassword(): void;
        loading: boolean;
        passwordErrors: string[];
        passwordValidationErrors: string[];
        errorMessage: string;
        clearMessages(): void;
        processSuccess: boolean;
    }

    export class ChangePasswordCtrl implements IChangePasswordViewModel {

        changePasswordInput: app.domain.IChangePasswordInput;
        loading: boolean;
        passwordErrors: string[];
        passwordValidationErrors: string[];
        errorMessage: string;
        processSuccess: boolean;

        static $inject = ['constantService', 'dataService', 'authService'];
        constructor(private constantService: app.common.services.ConstantService,
            private dataService: app.common.services.DataService,
            private authService: app.common.services.AuthService) {

            this.changePasswordInput = new app.domain.ChangePasswordInput();
            this.passwordErrors = [];
            this.passwordValidationErrors = [];
        }

        changePassword(): void {
            this.clearMessages();
            this.loading = true;
            this.dataService.postWebService(this.constantService.apiUsersURI + 'changePassword', this.changePasswordInput)
                .then((result: any) => {
                    this.processSuccess = true;
                    //Update user data
                    this.authService.getUsuarioActual();
                })
                .catch((result: any) => {
                    this.processSuccess = false;
                    if (result.status == 400) {
                        this.errorMessage = "Error de validación."
                        //Model errors
                        var modelStateErrors = result.data.ModelState;
                        for (var prop in modelStateErrors) {
                            if (prop == "model.Password") {
                                this.passwordErrors = modelStateErrors[prop];
                            }
                            if (prop == "model.PasswordValidation") {
                                this.passwordValidationErrors = modelStateErrors[prop];
                            }
                        }
                    }
                    else {
                        this.errorMessage = result.data;
                    }
                })
                .finally(() => this.loading = false);
        }

        clearMessages(): void {
            this.processSuccess = false;
            this.errorMessage = "";
            this.passwordErrors = [];
            this.passwordValidationErrors = [];
        }
    }

    angular.module('tannerPrivadoApp')
        .controller('ChangePasswordCtrl', ChangePasswordCtrl);
}