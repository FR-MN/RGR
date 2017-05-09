(function () {
    var $ref = $('.show-image');
     
    

    $ref.on('click', function (e) {
        var temp = $(this).attr("href");
        alert(temp);

        var imageid = $(this).attr("href");
        e.preventDefault();
        $.ajax({
            type: "POST",
            url: "/Images/GetFullImage",

            data: JSON.stringify("sadasdasdasdasd"),
            contentType: "application/json; charset=utf-8",
            dataType: "json",

            success: successFunc,
            error: errorFunc
        });


    })

    function successFunc(data, status) {
        alert(data.id);
    }
    function errorFunc(errorData) {
        alert('Ошибка' + errorData.responseText);
    }

})();