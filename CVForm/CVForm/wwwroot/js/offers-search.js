﻿$(document).ready(function () {

    var currentPage = 1;
    var searchString = "";
    getOffers(searchString, currentPage);
    $('#searchButton').on('click',
        function (e) {
            searchString = $('#search').val();
            currentPage = 1;
            getOffers(searchString,currentPage);
        });
    $('#offers').on('click', '.pagesSwitchContent a', function (e) {
        e.preventDefault();
        var pageNo = parseInt($(this).html());
        currentPage = pageNo;
        getOffers(searchString,currentPage);
    });

});

function getOffers(searchString, currentPage) {
    var $loading = "<div class='loading'>Please wait...</div>";
    $('#offers').prepend($loading);

    var uri = 'api/joboffersapi';

    if (searchString !== "") {
        uri += '/' + searchString;
    }

    uri += '/' + currentPage;

    $.getJSON(uri)
        .done(function (data) {

            $('#offers').empty();

            $.each(data.offers,
                function (key, offer) {
                    var date = new Date(offer.created);
                    var dateString = parseDate(date);

                    $('<tr>' +
                        '<td><a href="/JobOffer/Details/' + offer.id + '">' +
                        offer.jobTitle +
                        '</a></td>' +
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

            var totalPage = parseInt(data.totalPage);
            var $pagesSwitch = $('<tr/>');
            var $pagesSwitchContent = $('<td/>').attr('colspan', 4).addClass('pagesSwitchContent');

            if (totalPage > 0) {
                for (var i = 1; i <= totalPage ; i++) {
                    var $page = $('<span/>').addClass((i == currentPage) ? "current" : "");
                    $page.html((i == currentPage) ? i : "<a href='#'>" + i + "</a>");
                    $pagesSwitchContent.append($page);
                }
                $pagesSwitch.append($pagesSwitchContent);
            }
            $('#offers').append($pagesSwitch);

        });
}