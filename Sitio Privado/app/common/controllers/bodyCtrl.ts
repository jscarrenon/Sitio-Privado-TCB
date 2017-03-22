﻿module app.common.controllers {

    interface IBodyViewModel {
        seccionId: number;
        seleccionarSeccion(id: number): void;
        crearInstanciaModal(objeto: string): void;
    }

    export class BodyCtrl implements IBodyViewModel {

        seccionId: number;
        suscripcionFirmaElectronica: number;
        sites: Array<app.domain.SiteInformation>;
        multiples: boolean;
        static $inject = ['constantService', '$localForage', 'dataService', 'authService', 'extrasService', '$scope', '$uibModal', '$location', 'Analytics'];
        constructor(private constantService: app.common.services.ConstantService,
            private $localForage,
            private dataService: app.common.services.DataService,
            private authService: app.common.services.AuthService,
            private extrasService: app.common.services.ExtrasService,
            private $scope: ng.IScope,
            private $uibModal: ng.ui.bootstrap.IModalService,
            private $location: ng.ILocationService,
            private Analytics: ng.google.analytics.AnalyticsService) {
            this.suscripcionFirmaElectronica = 0;
            this.seccionId = 0;
            this.seleccionarSeccion(this.seccionId);

            this.$scope.$watch(() => this.authService.susFirmaElecDoc,
                (respuesta: number) => {
                    if (respuesta == 0) {
                        this.crearInstanciaModal("susConFirmaElectronicaDocs");
                    } else if (respuesta > 0) {
                        this.suscripcionFirmaElectronica = 1;
                    }
                });


            this.$scope.$watch(() => this.suscripcionFirmaElectronica, (value: number) => {
                if (value > 0) {
                    this.modalDocumentosPendientes();
                    this.modalCircularizacion();
                }
            });

            this.$scope.$on('$routeChangeSuccess', (event: any) => {
                Analytics.trackPage(Analytics.getUrl());
            });

            this.getUserSitesByToken();
        }

        seleccionarSeccion(id: number): void {
            this.seccionId = id;
        }

        crearInstanciaModal(objeto: string): void {
            var urlPlantilla = null;
            var ruta = null;

            switch (objeto) {
                case "circularizacion":
                    urlPlantilla = this.constantService.templateCircularizacionModalURI;
                    ruta = '/mis-inversiones/circularizacion';
                    break;
                case "documentos":
                    urlPlantilla = this.constantService.templateDocumentosPendientesModalURI;
                    ruta = '/mis-inversiones/estado-documentos';
                    break;
                case "susConFirmaElectronicaDocs":
                    urlPlantilla = this.constantService.templateSusConfFirmaElecDocModalURI;
                    ruta = '/mis-inversiones/estado-documentos';
                    break;
            };

            if (urlPlantilla != null && ruta != null) {
                var instanciaModal: ng.ui.bootstrap.IModalServiceInstance = this.$uibModal.open({
                    templateUrl: urlPlantilla,
                    controller: 'ModalInstanceCtrl as modal'
                });

                instanciaModal.result.then(_ => this.$location.path(ruta));
            }
        }

        modalDocumentosPendientes() {
            this.$scope.$watch(() => this.authService.documentosPendientes,
                (newValue: number, oldValue: number) => {
                    if ((newValue != oldValue) && newValue > 0) {
                        this.crearInstanciaModal("documentos");
                    }
                });
        }

        modalCircularizacion() {
            this.$scope.$watch(() => this.authService.circularizacionPendiente,
                (newValue: boolean, oldValue: boolean) => {
                    if ((newValue != oldValue) && newValue) {
                        this.crearInstanciaModal("circularizacion");
                    }
                });
        }

        getUserSitesByToken(): void {
            this.$localForage.getItem('accessToken')
                .then((responseToken) => {
                    this.$localForage.getItem('refreshToken')
                        .then((refreshTokenResult) => {
                            this.$localForage.getItem('expiresIn')
                                .then((expiresInResult) => {
                                    this.dataService.postWebService(this.constantService.apiAutenticacion + 'usersites', null, responseToken)
                                        .then((result: Array<app.domain.SiteInformation>) => {
                                            result.forEach((site) => {
                                                site.Url = site.Url + '?accessToken=' + responseToken + '&refreshToken=' + refreshTokenResult + '&expiresIn=' + expiresInResult;
                                            });
                                            this.sites = result;
                                            if (result.length > 1)
                                                this.multiples = true;

                                            return result;
                                        });
                                });
                        });
                });
        }

    }

    angular.module('tannerPrivadoApp')
        .controller('BodyCtrl', BodyCtrl);
}