$(document).ready(function() {

    jQuery.validator.addMethod("SalaryToValidation", function (value, element) {
        var salaryFrom = parseFloat($("#SalaryFrom").val());
        return this.optional(element) || salaryFrom <= 0 || parseFloat(value) > salaryFrom;
    }, "Upper bound of salary must be bigger than lower bound");

    $.validator.addMethod("minDate", function (value, element) {
        var curDate = new Date();
        var inputDate = new Date(value);
        if (inputDate >= curDate)
            return true;
        return false;
    }, "Date cannot be a past date");   


    var validator = $("#jobOfferForm").validate({
        rules: {
            JobTitle: {
                required: true
            },
            Company: {
                required: true
            },
            ValidUntil: {
                required: true,
                minDate: true
            },
            Description: {
                required: true,
                minlength: 20
            },
            Location: {
                required: true
            },
            SalaryFrom: {
                required: true,
                number: true,
                min: 0
                
            },
            SalaryTo: {
                required: true,
                number: true,
                min: 0,
                SalaryToValidation: true
            }
        },
        errorPlacement: function (error, element) {
            var potentialParent = element.parent("div").parent("div");
            potentialParent.length == 0 ? error.appendTo(element.parent("div")) : error.appendTo(element.parent("div").parent("div"));
        },
        errorClass: "js-invalid",
        validClass: "js-success"
    });

    

});