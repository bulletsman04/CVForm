// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(document).ready(function() {
    
    var uri = 'api/joboffersapi';
    getOffers(uri);
    $('#searchButton').on('click',
        function(e) {
            getOffers(uri + '/' + $('#search').val());
        });

});

function getOffers(url) {
    $('#offers').empty();
    $.getJSON(url)
        .done(function (data) {
            $.each(data,
                function (key, offer) {
                    // ToDO: albo dodać '0' do miesiaca i wyłączyć funkcję albo użyć jakiejś biblioteki.
                    var date = new Date(offer.created);
                    var dateString = date.getFullYear() + '-' + (date.getMonth() + 1) + '-' + date.getDate();

                    $('<tr>' +
                        '<td>' +
                        offer.jobTitle +
                        '</td>' +
                        '<td>' +
                        offer.company.name +
                        '</td>' +
                        '<td>' +
                        offer.location +
                        '</td>' +
                        '<td>' +
                        dateString +
                        '</td>' +
                        '</tr>'
                    ).appendTo($('#offers'));
                });
        });
}