module app.common.controllers {

    interface IChangePasswordViewModel {
        changePasswordInput: app.domain.IChangePasswordInput;
        changePassword(): void;
        loading: boolean;
        errorMessage: string;
        clearMessages(): void;
        processSuccess: boolean;
        errorResponseMessages: { [key: number]: string };
    }

    export class ChangePasswordCtrl implements IChangePasswordViewModel {

        changePasswordInput: app.domain.IChangePasswordInput;
        loading: boolean;
        errorMessage: string;
        processSuccess: boolean;
        errorResponseMessages: { [key: number]: string };

        static $inject = ['constantService', 'dataService', '$localForage','authService','$scope'];
        constructor(private constantService: app.common.services.ConstantService,
            private dataService: app.common.services.DataService,
            private $localForage,
            private authService: app.common.services.AuthService,
            private $scope: any) {

            this.changePasswordInput = new app.domain.ChangePasswordInput();

            this.errorResponseMessages = {
                40001: "Error de validación.",
                40005: "No se pudo autenticar el usuario.",
                40099: "Contraseñas ingresadas no coinciden", // Special case
                50000: "Error interno del servidor.",
            }
        }

        changePassword(): void {
            this.clearMessages();

            if (!this.changePasswordInput.OldPassword || !this.changePasswordInput.NewPassword || !this.changePasswordInput.PasswordValidation) {
                this.errorMessage = this.errorResponseMessages[40001];
                return;
            }

            if (this.changePasswordInput.NewPassword !== this.changePasswordInput.PasswordValidation) {
                this.errorMessage = this.errorResponseMessages[40099];
                return;
            }

            this.loading = true;
            this.$localForage.getItem('accessToken')
                .then((responseToken) => {
                    this.dataService.postWebService(this.constantService.apiOAuthURI + this.constantService.apiUsersURI + 'changePassword', this.changePasswordInput, responseToken)
                        .then((result: any) => {
                            this.processSuccess = true;
                            //Clear fields
                            this.changePasswordInput.OldPassword = "";
                            this.changePasswordInput.NewPassword = "";
                            this.changePasswordInput.PasswordValidation = "";

                            //Set form pristine
                            this.$scope.form.$setPristine();
                        })
                        .catch((result: any) => {
                            this.processSuccess = false;
                            if (result.data && result.data["errorCode"] && this.errorResponseMessages[result.data["errorCode"]]) {
                                this.errorMessage = this.errorResponseMessages[result.data["errorCode"]];
                            }
                            else {
                                this.errorMessage = result.data;
                            }
                        })
                        .finally(() => this.loading = false);
                });
        }

        clearMessages(): void {
            this.processSuccess = false;
            this.errorMessage = "";
        }
    }

    angular.module('tannerPrivadoApp')
        .controller('ChangePasswordCtrl', ChangePasswordCtrl);
}