﻿module app.common.services {

    interface IDataService {
        get(resource: string): ng.IPromise<app.domain.EntityBase[]>;
        getSingle(resource: string): ng.IPromise<app.domain.EntityBase>;
        add(resource: string, entity: app.domain.IEntity): ng.IPromise<app.domain.EntityBase>;
        update(resource: string, entity: app.domain.IEntity): ng.IPromise<app.domain.EntityBase>;
        remove(resource: string): ng.IPromise<any>;
        postWebService(resource: string, input: app.domain.InputBase, accessTpken: string): ng.IPromise<app.domain.EntityBase>; //Recibe input para llamado webservice
    }

    export class DataService implements IDataService {

        private httpService: ng.IHttpService;
        private qService: ng.IQService;

        static $inject = ['$http', '$q', '$localForage'];
        constructor($http: ng.IHttpService, $q: ng.IQService, private $localForage) {
            this.httpService = $http;
            this.qService = $q;
        }

        get(resource: string): ng.IPromise<app.domain.EntityBase[]> {
            var self = this;

            var deferred = self.qService.defer();
            return this.$localForage.getItem('accessToken')
                .then((responseToken) => {
                    self.httpService.get(resource, { headers: { Authorization: 'Bearer ' + responseToken } })
                        .then(function (result: any) {
                            deferred.resolve(result.data);
                        }, function (error) {
                            deferred.reject(error);
                        });

                    return deferred.promise;
                });

        }

        getSingle(resource: string): ng.IPromise<app.domain.EntityBase> {

            var self = this;

            var deferred = self.qService.defer();
            return this.$localForage.getItem('accessToken')
                .then((responseToken) => {
                    self.httpService.get(resource).then(function (result: any) {
                        deferred.resolve(result.data);
                    }, function (error) {
                        deferred.reject(error);
                    });

                    return deferred.promise;
                });
        }

        add(resource: string, entity: app.domain.IEntity): ng.IPromise<app.domain.EntityBase> {
            var self = this;
            var deferred = self.qService.defer();

            self.httpService.post(resource, entity)
                .then(function (result) {
                    deferred.resolve(result.data);
                }, function (error) {
                    deferred.reject(error);
                });

            return deferred.promise;
        }

        update(resource: string, entity: app.domain.IEntity): ng.IPromise<app.domain.EntityBase> {
            var self = this;
            var deferred = self.qService.defer();

            self.httpService.put(resource, entity)
                .then(function (data) {
                    deferred.resolve(data);
                }, function (error) {
                    deferred.reject(error);
                });

            return deferred.promise;
        }

        remove(resource: string): ng.IPromise<any> {
            var self = this;

            var deferred = self.qService.defer();

            self.httpService.delete(resource)
                .then(function (data) {
                    deferred.resolve(data);
                }, function (error) {
                    deferred.reject(error);
                });

            return deferred.promise;
        }

        postWebService(resource: string, input?: app.domain.InputBase, accessToken?: string): ng.IPromise<app.domain.EntityBase> {
            var self = this;
            var deferred = self.qService.defer();

            self.httpService.post(resource, input, { headers: { Authorization: 'Bearer ' + accessToken } })
                .then(function (result) {
                    deferred.resolve(result.data);
                }, function (error) {
                    deferred.reject(error);
                });

            return deferred.promise;
        }
    }

    angular.module('tannerPrivadoApp')
        .service('dataService', DataService);
} 