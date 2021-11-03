$(document).ready(function () {
    (function($) {
        "use strict";
    
    // validate certificationForm
    $(function() {
        $('#certificationForm').validate({
            rules: {
                certificationType: {
                    required: true
                },
                identificationType: {
                    required: true
                },
                identificationNumber: {
                    required: true,
                    minlength: 4
                },
                phone: {
                    required: true,
                    minlength: 5
                },
                email: {
                    required: true,
                    email: true
                }
            },
            messages: {
                certificationType: {
                    required: "Por favor, selecciona un tipo de certificación"
                },
                identificationType: {
                    required: "Por favor, selecciona un tipo de identificación"
                },
                identificationNumber: {
                    required: "Por favor, ingresa tu número de identificación",
                    minlength: "El número de identificación debe tener al menos 4 caracteres"
                },
                phone: {
                    required: "Por favor, ingresa un número de teléfono",
                    minlength: "El número de teléfono debe tener al menos 5 caracteres"
                },
                email: {
                    required: "Por favor, ingresa un correo electrónico",
                    email: "Por favor, ingrese un correo con formato válido"
                }
            },
            submitHandler: function (form) {
                $(form).ajaxSubmit({
                    type:"POST",
                    data: $(form).serialize(),
                    url: "/NewCertification",
                    success: function () {
                        console.log("Success");
                        $("#certificationType option[value='']").prop('selected', true);
                        $("#identificationType option[value='']").prop('selected', true);
                        $('#identificationNumber').val("");
                        $('#email').val("");
                        $('#phone').val("");
                        alert("Solicitud generada exitosamente. Pronto enviaremos su certificado.");
                    },
                    error: function () {
                        console.log("Error");
                        alert("No fue posible generar la solicitud. Por favor, vuelva a intentar.");
                    }
                })
            }
        })
    })
        
 })(jQuery)
})