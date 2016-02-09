module app.common.directives {

    interface ILoadingDirectiveScope extends ng.IScope {
        param: boolean;
        extraClass: string;
        isLoading(): boolean;
    }


    class LoadingDirective implements ng.IDirective {
        restrict = 'E';
        templateUrl = this.constantService.templateLoadingURI;
        replace = true;
        scope = {
            param: '=',
            extraClass: '='
        }

        constructor(private constantService: app.common.services.ConstantService) {
        }

        link = (scope: ILoadingDirectiveScope, element: ng.IAugmentedJQuery, attrs: ng.IAttributes) => {

            scope.isLoading = function () {
                return scope.param;
            };

            scope.$watch(scope.isLoading, function (v) {
                if (v) {
                    element.show();
                } else {
                    element.hide();
                }
            });

        }

        static factory(): ng.IDirectiveFactory {
            const directive = (constantService: app.common.services.ConstantService) => new LoadingDirective(constantService);
            directive.$inject = ['constantService'];
            return directive;
        }
    }

    angular.module('tannerPrivadoApp')
        .directive('loading', LoadingDirective.factory());

}

