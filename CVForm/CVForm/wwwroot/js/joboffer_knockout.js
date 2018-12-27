$(document).ready(function() {
    var ViewModel = function (jobTitle,company,location,salaryFrom, salaryTo, validUntil, description) {
        this.jobTitle = ko.observable(jobTitle);
        this.company = ko.observable(company);
        this.location = ko.observable(location);
        this.salaryFrom = ko.observable(salaryFrom);
        this.salaryTo = ko.observable(salaryTo);
        this.validUntil = ko.observable(validUntil);
        this.description = ko.observable(description);


        this.validDays = ko.computed(function () {
            var one_day = 1000 * 60 * 60 * 24;
            var curDate = new Date();
            var inputDate = new Date(this.validUntil());

            if (this.validUntil() === undefined)
                return "";

            return Math.ceil((inputDate - curDate)/one_day);
        }, this);

        this.created = ko.computed(function () {

            //ToDO: Change - but firstly extract globl function for parsing date of this type
            return new Date();
        }, this);

        this.companyName = ko.computed(function () {

           return $('#CompanyId option[value=' + this.company() + ']').text();
        }, this);
    };

    ko.applyBindings(new ViewModel());
});