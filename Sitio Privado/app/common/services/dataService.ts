module App.Common.Services {

    interface IDataService {
        get(resource: string): ng.IPromise<App.Domain.EntityBase[]>;
        getSingle(resource: string): ng.IPromise<App.Domain.EntityBase>;
        add(resource: string, entity: App.Domain.IEntity): ng.IPromise<App.Domain.EntityBase>;
        update(resource: string, entity: App.Domain.IEntity): ng.IPromise<App.Domain.EntityBase>;
        remove(resource: string): ng.IPromise<any>;
        postWebService(resource: string, input: App.Domain.InputBase): ng.IPromise<App.Domain.EntityBase>; //Recibe input para llamado webservice
    }

    export class DataService implements IDataService {

        private httpService: ng.IHttpService;
        private qService: ng.IQService;

        static $inject = ['$http', '$q'];
        constructor($http: ng.IHttpService, $q: ng.IQService) {
            this.httpService = $http;
            this.qService = $q;
        }

        get(resource: string): ng.IPromise<App.Domain.EntityBase[]> {
            console.log(resource);
            var self = this;

            var deferred = self.qService.defer();

            self.httpService.get(resource).then(function (result: any) {
                deferred.resolve(result.data);
            }, function (error) {
                deferred.reject(error);
            });

            return deferred.promise;
        }

        getSingle(resource: string): ng.IPromise<App.Domain.EntityBase> {
            var self = this;

            var deferred = self.qService.defer();

            self.httpService.get(resource).then(function (result: any) {
                deferred.resolve(result.data);
            }, function (error) {
                deferred.reject(error);
            });

            return deferred.promise;
        }

        add(resource: string, entity: App.Domain.IEntity): ng.IPromise<App.Domain.EntityBase> {
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

        update(resource: string, entity: App.Domain.IEntity): ng.IPromise<App.Domain.EntityBase> {
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

        postWebService(resource: string, input: App.Domain.InputBase): ng.IPromise<App.Domain.EntityBase> {
            var self = this;
            var deferred = self.qService.defer();

            self.httpService.post(resource, input)
                .then(function (result) {
                    deferred.resolve(result.data);
                }, function (error) {
                    deferred.reject(error);
                });

            return deferred.promise;
        }

        private static _module: ng.IModule;
        public static get module(): ng.IModule {
            if (this._module) {
                return this._module;
            }
            this._module = angular.module('dataService', []);
            this._module.service('dataService', DataService);
            return this._module;
        }
    }
} 