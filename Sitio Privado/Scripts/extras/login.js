jQuery(document).ready(function ($) {
    //BaseURL definida en Web.config
    //Cambiar de formulario para recuperar contraseña
    $('#textoRecuperarContr').click(function (event) {
        //Oculta campos de contraseña temporal
        $('.claveTemp').hide();
        //Oculta campo de error
        $('#recuperatePasswordError').hide();
        //Oculta instrucciones
        $('#recuperatePasswordInstrucciones').hide();
        //Cambia clase contenedor
        $('#container').attr('class', 'clientes_olvido_contrasenia');
        //Muestra formulario para recuperar contraseña
        $('#container').html($('#recuperatePasswordFormContainer').html());
        //Asignamos las funciones de los formularios
        formsActions(); 
    });
    //Procesar loginForm
    var $loginForm = $("#loginForm");
    // Attach a submit handler to the form
    $loginForm.submit(function (event) {
        //Detiene submit normal
        event.preventDefault();
        //Oculta formulario
        $loginForm.hide();
        //Despliega loader
        $('#contenedorLoaderPestania').show();
        //Enviar datos usando post
        var posting = $.ajax({
            type: 'POST',
            url: baseURL + '/api/users/signInExternal',
            data: $loginForm.serialize(),
            crossDomain: true
        });
        // En caso de éxito
        posting.done(function (data) {
            //Loader sigue desplegado y formulario oculto
            //Envía nuevo formulario 
            formSignIn(data).submit();
        });
        //En caso de error
        posting.fail(function (jqXHR, textStatus, errorThrown) {
            //Cambia clase contenedor
            $('#container').attr('class', 'clientes_login_error');
            //Oculta loader
            $('#contenedorLoaderPestania').hide();
            //Muestra formulario
            $loginForm.show();
            //Muestra error
            $('#loginError').html('<div></div><p>' + jqXHR.responseText + '</p>').show();
        });
    });
    //Fuciones para manipular formularios
    var formsActions = function() {
        //Procesar passwordRecoveryForm
        var $passwordRecoveryForm = $("#passwordRecoveryForm");
        // Attach a submit handler to the form
        $passwordRecoveryForm.submit(function (event) {
            //Detiene submit normal
            event.preventDefault();
            //Oculta formulario
            $passwordRecoveryForm.hide();
            //Despliega loader
            $('#contenedorLoaderRecoveryForm').show();
            //Enviar datos usando post
            var posting = $.ajax({
                type: 'POST',
                url: baseURL + '/api/users/passwordRecovery',
                data: $passwordRecoveryForm.serialize(),
                crossDomain: true
            });
            // En caso de éxito
            posting.done(function (data) {
                //Oculta loader
                $('#contenedorLoaderRecoveryForm').hide();
                //Muestra formulario
                $passwordRecoveryForm.show();
                //Cambiar form
                $('.recuperarContr').hide();
                $('.claveTemp').show();
                $('#claveTempInfo').hide();
                $('#recuperatePasswordError').hide();
                $('#recuperatePasswordInstrucciones').show();
            });
            //En caso de error
            posting.fail(function (jqXHR, textStatus, errorThrown) {
                //Cambia clase contenedor
                $('#container').attr('class', 'clientes_olvido_contrasenia_error');
                //Oculta loader
                $('#contenedorLoaderRecoveryForm').hide();
                //Muestra formulario
                $passwordRecoveryForm.show();
                //Cambia form
                $('.recuperarContr').show();
                $('.claveTemp').hide();
                $('#claveTempInfo').hide();
                //Muestra error
                $('#recuperatePasswordError').html('<div></div><p>' + jqXHR.responseText + '</p>').show();
                $('#recuperatePasswordInstrucciones').hide();
            });
        });
        //Botón que ingresa contraseña temporal
        var $tempPassButton = $("#submitClaveTempButton");
        $tempPassButton.click(function () {
            //Oculta formulario
            $passwordRecoveryForm.hide();
            //Despliega loader
            $('#contenedorLoaderRecoveryForm').show();
            //Enviar datos usando post
            var posting = $.ajax({
                type: 'POST',
                url: baseURL + '/api/users/signInExternal',
                data: $passwordRecoveryForm.serialize(),
                crossDomain: true
            });
            // En caso de éxito
            posting.done(function (data) {
                //Loader sigue desplegado y formulario oculto
                //Envía nuevo formulario 
                formSignIn(data).submit();
            });
            //En caso de error
            posting.fail(function (jqXHR, textStatus, errorThrown) {
                //Cambia clase contenedor
                $('#container').attr('class', 'clientes_olvido_contrasenia_error');
                //Oculta loader
                $('#contenedorLoaderRecoveryForm').hide();
                //Muestra formulario
                $passwordRecoveryForm.show();
                //Muestra error
                $('#recuperatePasswordError').html('<div></div><p>' + jqXHR.responseText + '</p>').show();
                $('#recuperatePasswordInstrucciones').hide();
            });
        });
    }
    //Formulario sign in
    var formSignIn = function (data) {
        return $('<form action="' + baseURL + '/Account/SignIn' + '" method="POST">' +
                '<input type="hidden" name="token" value="' + data + '">' +
                '</form>');
    }
});

//RUT
var formatRut = function (inputId) {
    var currentValue = inputId.value;
    currentValue = currentValue.replace(/[^K0-9\s]/gi, '');
    if (currentValue.length > 1) {
        var rut = currentValue.substring(0, currentValue.length - 1);
        var dv = currentValue.charAt(currentValue.length - 1);
        rut = addDots(rut);
        inputId.value = rut + "-" + dv;
    }
    else {
        inputId.value = currentValue;
    }
}

var addDots = function (nStr) {
    nStr += '';
    x = nStr.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + '.' + '$2');
    }
    return x1 + x2;
}