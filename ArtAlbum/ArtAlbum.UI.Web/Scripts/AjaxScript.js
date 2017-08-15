(function () {
    var $subscribeBtn = $('#subscribe-btn'),
        userId = $('#userid').html();
       


    $subscribeBtn.on('click', function (e) {


        $.ajax({


            type: "GET",
            url: "/Users/Subscribe",

            data: {
                userId: userId
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",

            success: successFunc,
            error: errorFunc
        });


    })

    function successFunc(data, status) {
        $subscribeBtn.text(data)

        
    }
    function errorFunc(errorData) {
        alert('Ошибка' + errorData.responseText);
    }

})();

