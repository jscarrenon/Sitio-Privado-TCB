describe("modalInstanceCtrl - ", function () {

    beforeEach(function () {
        module('tannerPrivadoApp');
    });

    var $rootScope,
        modalInstanceCtrl; //controlador
    
    var uibModalInstance = { close: function () { }, dismiss: function () { } };

    beforeEach(inject(function (_$rootScope_, _$controller_) {
        modalInstanceCtrl = _$controller_("ModalInstanceCtrl", {
            $rootScope: _$rootScope_,
            $uibModalInstance: uibModalInstance
        });
    }));

    it("Cerrar al presionar aceptar", function () {
        spyOn(modalInstanceCtrl.$uibModalInstance, "close").and.callThrough();
        modalInstanceCtrl.ok();

        expect(modalInstanceCtrl.$uibModalInstance.close).toHaveBeenCalled();
    });

    it("Desechar el modal al cancelar.", function () {
        spyOn(modalInstanceCtrl.$uibModalInstance, "dismiss").and.callThrough();
        modalInstanceCtrl.cancelar();

        expect(modalInstanceCtrl.$uibModalInstance.dismiss).toHaveBeenCalled();
    });
});