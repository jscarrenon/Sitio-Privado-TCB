module app.common.controllers {

    export interface IAuthenticationRouteParams extends ng.route.IRouteParamsService {
        accessToken?: string;
    }

    interface IAuthenticationViewModel {
        loading: boolean;
        usuario: app.domain.IUsuario;
        passwordErrors: string[];
        passwordValidationErrors: string[];
        errorMessage: string;
        tannerURL: string;
        clearMessages(): void;
        processSuccess: boolean;
    }
   
    export class AuthenticationCtrl implements IAuthenticationViewModel {

        loading: boolean;
        usuario: app.domain.IUsuario;
        passwordErrors: string[];
        passwordValidationErrors: string[];
        errorMessage: string;
        tannerURL: string;
        processSuccess: boolean;
        private authService: app.common.services.AuthService;
        private constService = $;

        static $inject = ['$window', '$location', 'constantService', 'dataService', 'authService', '$routeParams'];
        constructor(private $window: ng.IWindowService,
            private $location: ng.ILocationService,
            public constantService: app.common.services.ConstantService,
            private dataService: app.common.services.DataService,
            private authenticationService: app.common.services.AuthService,
            private $routeParams: IAuthenticationRouteParams) {

            this.usuario = null;
            this.passwordErrors = [];
            this.passwordValidationErrors = [];
            var token = $location.search().accessToken;
            
            authenticationService.validateToken(token)
                .then(function (result) {
                    this.usuario = result;
                    
                    if (this.usuario == null)
                        $window.location.href = constantService.homeTanner;
                    console.log(result);
                    this.usuario = result;
                    var token = $location.search().accessToken;
                    var refreshToken = $location.search().refreshToken;
                    var expiresIn = $location.search().expiresIn;
                    authenticationService.saveToken(token, refreshToken, expiresIn);
                    authenticationService.setTimerForRefreshToken();
                    $window.location.assign("/");
                  
                });  
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