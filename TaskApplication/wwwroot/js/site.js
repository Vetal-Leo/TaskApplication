
// It is manipulating with attributes and styles.
$(function () {

    addemploeeButton.onclick = function () {

        $(this).css('visibility', 'hidden');
        $("tr:last").css('visibility', 'visible');
        $('#inputcheckbox').val(true);
    };

    $('#inputcheckbox').click(function () {
        if ($(this).is(':checked')) {
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
    });

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


    function Clear() {
        TODO;
    }
});