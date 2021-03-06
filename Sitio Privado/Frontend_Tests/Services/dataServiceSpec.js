﻿describe('dataService - ', function () {

    beforeEach(function () {
        module('tannerPrivadoApp');
    });

    var $q, $http, 
        dataService;

    var azureBlobA_stub = {
        Name: 'Nombre del blob A',
        Url: 'url/del/blob/a'
    };

    var azureBlobB_stub = {
        Name: 'Nombre del blob B',
        Url: 'url/del/blob/b'
    };

    var azureBlobArray_stub = [azureBlobA_stub, azureBlobB_stub];

    beforeEach(inject(function (_$q_, _$rootScope_, _$http_, _dataService_) {
        $q = _$q_;
        $http = _$http_;
        dataService = _dataService_;
        $rootScope = _$rootScope_;
        http_deferred = $q.defer();        
    }));

    it('debería llamar a método get() del servicio $http.', function () {
        spyOn($http, 'get').and.returnValue(http_deferred.promise);
        dataService.get('/api/containers/getContainer?name=');

        expect($http.get).toHaveBeenCalled();
    });

    it('debería llamar a método getSingle() del servicio $http.', function () {
        spyOn($http, 'get').and.returnValue(http_deferred.promise);
        dataService.getSingle('/api/containers/getContainer?name=');

        expect($http.get).toHaveBeenCalled();
    });

    it('debería llamar a método add() del servicio $http.', function () {
        spyOn($http, 'post').and.returnValue(http_deferred.promise);
        dataService.add('/api/containers/getContainer?name=');

        expect($http.post).toHaveBeenCalled();
    });

    it('debería llamar a método update() del servicio $http..', function () {
        spyOn($http, 'put').and.returnValue(http_deferred.promise);
        dataService.update('/api/containers/getContainer?name=');

        expect($http.put).toHaveBeenCalled();
    });

    it('debería llamar a método remove() del servicio $http.', function () {
        spyOn($http, 'delete').and.returnValue(http_deferred.promise);
        dataService.remove('/api/containers/getContainer?name=');

        expect($http.delete).toHaveBeenCalled();
    });

    it('debería llamar a método $http postWebService().', function () {
        spyOn($http, 'post').and.returnValue(http_deferred.promise);
        dataService.postWebService('/api/containers/getContainer?name=');

        expect($http.post).toHaveBeenCalled();
    });
});