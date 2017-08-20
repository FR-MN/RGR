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

(function () {
    var $confirmBtn = $('#confirm-btn');

    
   

    $confirmBtn.on('click', function (e) {
 
        var color = $(".colorpicker_new_color").css("background-color"),
    nav = $("#nav").prop('checked'),
    background = $("#background").prop('checked'),
    main_elements = $("#main-elements").prop('checked'),
    font_color = $("#font-color").prop('checked'),
        ref_color = $("#ref-color").prop('checked');


        if (nav) {
            
            
            $(".gn-scroller").css("background-color", color);
            $(".gn-menu").css("background-color", color);
            
            $(".gn-menu-main").css("background-color",color);
        }

         if (background) {
            document.body.style.backgroundColor = color;
        }
         if (main_elements) {
             $(".dropdown").css("background-color", color);
            $("#first-user-block").css("background-color",color);
            $("#favorite-images").css("background-color",color);
            $("#main-images").css("background-color", color);
            $("#dd").css("background-color",color);
            $("#second-user-block").css("background-color", color);
            $(".button7").css("background", color);
            $(".modal-content").css("background-color", color);
           
        }
         if (font_color) {
            document.body.style.color = color;
        }
         if (ref_color) {
             
             $("footer").css("background-color", color);
             $(".button7").css("background-color",$("#first-user-block").css("background-color"));
             $("#user-name").css("color",color);
            
             $("a").css("color", color);
        }
    })
    })();