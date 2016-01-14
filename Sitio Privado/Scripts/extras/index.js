$(document).ready(function () {
    $("#bar").sticky({ topSpacing: 0 });
    $("#tabbar").sticky({ topSpacing: 0 });
});

var modal1 = function () {
    uglipop({
        class: 'modal-style modal1',
        source: 'html',
        content: '<div class="pretitle">Pendiente</div><div class="title">Circularización Anual de Custodia 2015</div><div class="text">Estimado Cliente,<br/> En conformidad a lo dispuesto en la Circular 1962 de la Superintendencia de Valores y Seguros (SVS), solicitamos a usted revisar los informes de saldos, que de acuerdo a nuestros registros se encuentran depositados a su nombre en custodia y/o garantía de Tanner Corredores de Bolsa S.A. al día 30 de Junio de 2015.</div><div class="button green"><a href="#" class="clink">Continuar</a></div><button class="modal-close" onclick="rem();"></button>'
    });
}

var modal2 = function () {
    uglipop({
        class: 'modal-style modal2',
        source: 'html',
        content: '<div class="pretitle">Pendiente</div><div class="title">Documentos Pendientes de Firma</div><div class="text">Estimado Cliente,<br/> Usted tiene documentos (operaciones y/o contratos) pendientes de ser firmados electrónicamente. Por favor proceda a revisarlos.</div><div class="button green"><a href="#" class="clink">Revisar</a></div><button class="modal-close" onclick="rem();"></button>'
    });
}

var modal3 = function () {
    uglipop({
        class: 'modal-style modal3',
        source: 'html',
        content: '<div class="title">Confirmar Firma Electrónica</div><div class="text">¿Desea confirmar y firmar electrónicamente la(s) operacion(es) y/o contrato(s) seleccionados?</div><button class="pop" onclick="modal4();">Confirmar</button><button class="modal-close" onclick="rem();"></button>'
    });
}

var modal4 = function () {
    uglipop({
        class: 'modal-style modal4',
        source: 'html',
        content: '<div class="title">Ingresar Clave</div><div class="text">Ingrese su clave de acceso para finalizar el proceso de firma electrónica.</div><form><div class="input-field">RUT (Ej: 12123123-K)<input type="text" placeholder="Ingrese su RUT"></div><div class="input-field">Contraseña<input type="password" placeholder="Ingrese su contraseña"></div></form><div class="button green"><a href="#" class="clink">Finalizar</a></div><button class="modal-close" onclick="rem();"></button>'
    });
}

var rem = function () {
    remove();
}