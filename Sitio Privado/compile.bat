@echo off
tsc.cmd app/app.ts app/common/controllers/webserviceCtrl.ts test/webserviceControllerSpec.ts --sourcemap --declaration --outFile _out/merged.js