$(document).ready(function () {
    $("#bar").sticky({ topSpacing: 0 });
});

$(document).ready(function () {
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

var rem = function () {
    remove();
}