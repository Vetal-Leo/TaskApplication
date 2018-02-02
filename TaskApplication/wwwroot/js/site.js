
// 
var userstatus = new Boolean(false);

$(function () {
    addemploeeButton.onclick = function () {

        $("#addemploeeButton").css('visibility', 'hidden');
        userstatus = true;
        $("tr:last").css('visibility', 'visible');
        $('#inputcheckbox').val(true);
       // alert(userstatus);//debugging alert;
    };

    $('#inputcheckbox').click(function () { Choice() });
    window.onload = function () {
        // alert($("#addemploeeButton").css('visibility'))//debugging alert;
        if ($("#addemploeeButton").css('visibility') === "visible") {
            userstatus = false;
        } else {
            userstatus = true;
        }
        // alert(userstatus);//debugging alert;
        if (userstatus) { Choice(); }
    };



    function Choice() {
        if ($('#inputcheckbox').is(':checked')) {
            $('#inputcheckbox').val(true);
            $('#inputdays').css('visibility', 'visible');
            $('#inputhours').css('visibility', 'hidden');
            $('#inputrate').attr("placeholder", " Введите ставку за день");
        } else {
            $('#inputcheckbox1').val(false);
            $('#inputcheckbox').val(false);
            $('#inputdays').css('visibility', 'hidden');
            $('#inputhours').css('visibility', 'visible');
            $('#inputrate').attr("placeholder", " Введите ставку за час");
        }
    };

    $('.calculate').click(function () {
        $.ajax({
            type: "Post",
            url: "/Index?handler=Calculate&emploeeId=" + this.id,
            contentType: false,
            processData: false,
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            success: function (data) {

                $('.emploee').text("Сотрудник: " + data[0] + " " + data[1]);
                $('.salary').text("Зарплата : " + data[2] + " гр.");
                $('.error').text("");
            },
            error: function (xhr, textStatus) {

                $('.error').text("Серверная ошибка! " + "Статус: " + xhr.status + " , " + textStatus);
                $('.emploee').text("");
                $('.salary').text("");
            }
        });
    });

});