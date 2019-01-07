// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


function parseDate(date) {
    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    if (date.getMonth() + 1 < 10) {
        month = "0" + month;
    }
    var day = date.getDate();
    var dateString = day + '.' + month + '.' + year;

    return dateString;
}