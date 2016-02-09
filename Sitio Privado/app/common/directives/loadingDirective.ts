module app.common.directives {

    interface ILoadingDirectiveScope extends ng.IScope {
        param: boolean;
        isLoading(): boolean;
    }


    class LoadingDirective implements ng.IDirective {
        restrict = 'E';
        template = '<div class="loading-spiner"><img src="../.build/img/loader-1.gif" /></div>';
        replace = true;
        scope = {
            param: '='
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
            const directive = () => new LoadingDirective();
            return directive;
        }
    }

    angular.module('tannerPrivadoApp')
        .directive('loading', LoadingDirective.factory());

}

