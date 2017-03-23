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
        token: string;
        refreshToken: string;
        expiresIn: number;
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
        token: string;
        refreshToken: string;
        expiresIn: number;

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
            this.token = $location.search().accessToken;
            this.refreshToken = $location.search().refreshToken;
            this.expiresIn = $location.search().expiresIn;
            
            //if (this.token != undefined && this.refreshToken != undefined &&
            //    this.expiresIn != undefined) {
            //    authenticationService.validateToken(this.token, this.refreshToken, this.expiresIn)
            //        .then(function (result) {
            //            this.usuario = result;
            //            if (this.usuario == null || this.usuario == undefined)
            //                $window.location.href = constantService.homeTanner;

            //         $window.location.assign("/");
            //        });
            //}
            //$window.location.href = constantService.homeTanner;
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