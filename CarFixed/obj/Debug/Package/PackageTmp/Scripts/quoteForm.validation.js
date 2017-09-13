/*============= QUOTE FORM =============*/

$('#_FrmSubmit').validate({
    rules: {
        vrm: "required",
        Postcode: {
            required: true,
            regex: true,
        },
        Firstname:{
            required:true,
            minlength: 2,
        },
        Lastname:{
            required:true,
            minlength: 2,
        },
        Email:{
            required: true,
            email: true
        },
        Telephone: {
            ukphone: true,
        },
		Password: {
			required: true,
			minlength: 6,
		    symbol: true
		},
	},
	messages: {
		vrm: "Please enter your Reg Number",
		Postcode: "Please enter your postcode",
		Firstname: {
            required: "Please enter your firstname",
            minlength: "Firstname should contain more that 2 characters"
        },
		Lastname: {
            required: "Please enter your lastname",
            minlength: "Lastname should contain more that 2 characters"
        },
	}
});

$.validator.addMethod("regex", function (value, element) {
	var uppercase = value.toUpperCase();
	return this.optional(element) || /([A-Z]){1,2}\d{1,2}([A-Z]?){1}\s?\d{1,2}([A-Z]){2}/g.test(uppercase);
	//return regex.test(value);
}, "Please enter a valid postcode.");

$.validator.addMethod("symbol", function (value, element) {
    var uppercase = value.toUpperCase();
    return this.optional(element) || /(?=.*[!£"@#$%^&~*=><?+-])/g.test(uppercase);
}, "Password must contain a non-letter symbol.");

$.validator.addMethod("ukphone", function (value, element) {
    var uppercase = value.toUpperCase();
    return this.optional(element) || /^(\+?44|0)([\d]){10}/g.test(uppercase);
}, "Please enter the valid UK number.");