
function parseDate(date) {
    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    if (date.getMonth() + 1 < 10) {
        month = "0" + month;
    }
    var day = date.getDate();
    var dateString = month + '/' + day + '/' + year;

    return dateString;
}