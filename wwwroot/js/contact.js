$(document).ready(function () {
    (function($) {
        "use strict";
    
    jQuery.validator.addMethod('answercheck', function (value, element) {
        return this.optional(element) || /^\bcat\b$/.test(value)
    }, "type the correct answer -_-");

    // validate contactForm form
    $(function() {
        $('#contactForm').validate({
            rules: {
                subject: {
                    required: true,
                    minlength: 4
                },
                firstname: {
                    required: true,
                    minlength: 2
                },
                lastname: {
                    required: true,
                    minlength: 4
                },
                phone: {
                    required: true,
                    minlength: 5
                },
                email: {
                    required: false,
                    email: true
                },
                message: {
                    required: true,
                    minlength: 20
                }
            },
            messages: {
                firstname: {
                    required: "Por favor, ingresa tu nombre",
                    minlength: "Tu nombre debe tener al menos 2 caracteres"
                },
                lastname: {
                    required: "Por favor, ingresa tu apellido",
                    minlength: "Tu apellido debe tener al menos 4 caracteres"
                },
                subject: {
                    required: "Por favor, ingresa tu asunto o tema",
                    minlength: "El asunto debe tener al menos 4 caracteres"
                },
                phone: {
                    required: "Por favor, ingresa un número de teléfono",
                    minlength: "El número de teléfono debe tener al menos 5 caracteres"
                },
                email: {
                    required: "",
                    email: "Por favor, ingrese un correo con formato válido"
                },
                message: {
                    required: "Por favor, ingresa una descripción del asunto o PQR",
                    minlength: "El mensaje debe tener al menos 20 caracteres"
                }
            },
            submitHandler: function (form) {
                $(form).ajaxSubmit({
                    type:"POST",
                    data: $(form).serialize(),
                    url: "/NewContactMessage",
                    success: function () {
                        console.log("Success");
                        $('#subject').val("");
                        $('#firstname').val("");
                        $('#lastname').val("");
                        $('#email').val("");
                        $('#phone').val("");
                        $('#message').val("");
                        alert("El mensaje fue enviado exitosamente. Pronto obtendrá respuesta al asunto.");
                    },
                    error: function () {
                        console.log("Error");
                        alert("No fue posible enviar el mensaje. Por favor, vuelva a intentar.");
                    }
                })
            }
        })
    })
        
 })(jQuery)
})