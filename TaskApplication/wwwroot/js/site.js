
// This is a global variable to determine the state: normal or the state of data input.
var userstatus = new Boolean(false);

$(function () {

    //This is the change on the page when you click on addemploeeButton.
    addemploeeButton.onclick = function () {

        $("#addemploeeButton").css('visibility', 'hidden');
        userstatus = true;
        $("tr:last").css('visibility', 'visible');
        $('#inputcheckbox').val(true);
        // alert(userstatus);//debugging alert;
    };

    //This is an event when the #inputcheckbox state changes.
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


    //This function opens the desired input for subsequent data entry.
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

    //This is a call to an asynchronous function to call the OnPostCalculate method,
    //which does all the calculations.
    $('.calculate').click(function () {

        var firstdate = $("#firstdate").val();
        var lastdate = $("#lastdate").val();
        $.ajax({
            type: "Post",
            url: "/Index?handler=Calculate&emploeeId=" + this.id + "&firstdate=" + firstdate +
            "&lastdate=" + lastdate,
            contentType: false,
            processData: false,
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            success: function (data) {
                if (!data[0]) {
                    $('.emploee').text("Сотрудник: " + data[2] + " " + data[3]);
                    $('.salary').text("Зарплата : " + data[5] + " гр.");
                    $('.status').text("Общее число отработанных часов : " + data[6]);
                    $('.error').text("");
                    $('.errordate').text("");
                    return;
                } else {
                    if (!data[1]) { $('.errordate').text(data[4]); return; }
                    $('.emploee').text("Сотрудник: " + data[2] + " " + data[3]);
                    $('.salary').text("Зарплата : " + data[5] + " гр.");
                    $('.status').text("Рассчитано без учета выходных дней за этот период. Общее число отработанных дней : " + data[6]);
                    $('.error').text("");
                    $('.errordate').text("");
                    return;
                }

            },
            error: function (xhr, textStatus) {

                $('.error').text("Серверная ошибка! " + "Статус: " + xhr.status + " , " + textStatus);
                $('.emploee').text("");
                $('.salary').text("");
                $('.errordate').text("");
            }
        });
    });

});