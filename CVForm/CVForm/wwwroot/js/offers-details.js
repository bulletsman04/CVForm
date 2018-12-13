$(document).ready(function () {
    var getUrl = window.location;
    var baseurl = getUrl.origin;
    var uri = baseurl+'/api/jobapplicationsapi';
    var offerId = document.getElementById("offer-id").innerHTML;
    getOffers(uri,offerId);
});

function getOffers(url,offerId) {
    $.getJSON(url+'/'+offerId)
        .done(function (data) {
            $.each(data,
                function (key, application) {

                    $('<tr>' +
                        '<td><a href="/Apply/Details?offerId=' + offerId + '&applicationId='+application.id+'">' +
                        application.firstName + ' ' + application.lastName +
                        '</a></td>' +
                        '<td>' +
                        application.emailAddress +
                        '</td>' +
                        '<td>' +
                        application.phoneNumber +
                        '</td>' +
                        '</tr>'
                    ).appendTo($('#applications'));
                });
        });
}