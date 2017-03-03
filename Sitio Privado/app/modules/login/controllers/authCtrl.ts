module app.common.controllers {

    export interface IAuthenticationRouteParams extends ng.route.IRouteParamsService {
        accessToken?: string;
    }

    interface IAuthenticationViewModel {
        loading: boolean;
        passwordErrors: string[];
        passwordValidationErrors: string[];
        errorMessage: string;
        clearMessages(): void;
        processSuccess: boolean;
    }
   
    export class AuthenticationCtrl implements IAuthenticationViewModel {

        loading: boolean;
        passwordErrors: string[];
        passwordValidationErrors: string[];
        errorMessage: string;
        processSuccess: boolean;

        static $inject = ['$window','$location', 'constantService', 'dataService', 'authService', '$routeParams'];
        constructor(private $window: any,
            private $location: any,
            private constantService: app.common.services.ConstantService,
            private dataService: app.common.services.DataService,
            private authService: app.common.services.AuthService,
            private $routeParams: IAuthenticationRouteParams) {

            this.passwordErrors = [];
            this.passwordValidationErrors = [];

            var token = $routeParams.accessToken;

            var tokenValidationResult = authService.validateToken(token);
            if (!tokenValidationResult) {
                $window.location.href = this.constantService.homeTanner;
            }

            console.log(tokenValidationResult);
        }

       

        clearMessages(): void {
            this.processSuccess = false;
            this.errorMessage = "";
            this.passwordErrors = [];
            this.passwordValidationErrors = [];
        }
    }

    angular.module('tannerPrivadoApp')
        .controller('AuthenticationCtrl', AuthenticationCtrl);
}