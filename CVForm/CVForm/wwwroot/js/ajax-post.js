$(document).ready(function () {
    var getUrl = window.location;
    //var baseurl = getUrl.origin;
    //var uri = baseurl + '/api/jobapplicationsapi';
    $("#jobOfferFormEdit").submit(function (e) {
        editOffer.call(this,e);
    });
});

function editOffer(e) {
    var form = $(this);

    var url = form.attr('action');
    $("#edit-btn-container").find("#feedback").remove();
    $("#edit-btn-container").append("<div class='loader'></div>");
    $.ajax({
        type: "POST",
        url: url,
        data: form.serialize(), 
        success: function () {
            $("#edit-btn-container").find(".loader").remove();
            $("#edit-btn-container").append("<span id='feedback' class='js-edited-success'> Success!</span>");
        },
        error: function () {
            $("#edit-btn-container").find(".loader").remove();
            $("#edit-btn-container").append("<span id='feedback' class='js-edited-failure'> An error occured!</span>");
        }
    });

     e.preventDefault();
}