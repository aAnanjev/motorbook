$(document).ready(function () {
    $('#driver-form').validate({
        rules: {
            FirstName: {
                required: true,
                minlength: 2,
                lettersonly: true
            },
            LastName: {
                required: true,
                minlength: 2,
                lettersonly: true
            },
            Email: {
                required: true,
                email: true,
            },
            PhoneNum: {
                required: true,
                ukphone: true,
            },
            Password: {
                required: true,
                minlength: 6,
                symbol: true,
            }
        },
        messages: {
            FirstName: {
                required: "Please enter your firstname",
                minlength: "Firstname should contain more that 2 characters"
            },
            LastName: {
                required: "Please enter your lastname",
                minlength: "Lastname should contain more that 2 characters"
            },
            PhoneNum: {
                required: "Please enter you phone number",
            },
        }
    });

    $.validator.addMethod("symbol", function (value, element) {
        var uppercase = value.toUpperCase();
        return this.optional(element) || /(?=.*[!£"@#$%^&~*=><?+-])/g.test(uppercase);
    }, "Password must contain a non-letter symbol.");

    jQuery.validator.addMethod("exactlength", function (value, element, param) {
        return this.optional(element) || value.length == param;
    }, jQuery.format("Please enter exactly {0} characters."));

    $.validator.addMethod("lettersonly", function (value, element) {
        return this.optional(element) || /^[a-zA-Z]+$/g.test(value);
        //return regex.test(value);
    }, "Please use letters only.");
    
    $.validator.addMethod("ukphone", function (value, element) {
        var uppercase = value.toUpperCase();
        return this.optional(element) || /^(\+?44|0)([\d]){10}/g.test(uppercase);
    }, "Please enter the valid UK number.");
});