(function () {
    var $ref = $('.show-image'),
    
    img = new Image();
    

    $ref.on('click', function (e) {
        
        

        var imageid = $(this).attr("data-target").slice(1),
            temp = "#" + imageid,
            md = $(temp).find($(".top_image"));
        $(temp).appendTo("body");
        e.preventDefault();
        
        
        img.src = '/Images/GetImage/' + imageid;
        md.append(img);

    })
       
     

})();
(function () {
    var $ref = $('.next');
    img = new Image();
   


    $ref.on('click', function (e) {



        var imageid = $(this).attr("data-target").slice(1),
            temp = "#" + imageid,
            md = $(temp).find($(".top_image"));
        $(temp).appendTo("body");
        e.preventDefault();


        img.src = '/Images/GetImage/' + imageid;
        md.append(img);
        var imageid = $(this).attr("value");
       
        $(imageid).modal('hide')


      

    })



})();
(function () {
    var $ref = $('.prev');
    img = new Image();



    $ref.on('click', function (e) {



        var imageid = $(this).attr("data-target").slice(1),
            temp = "#" + imageid,
            md = $(temp).find($(".top_image"));
        $(temp).appendTo("body");
        e.preventDefault();


        img.src = '/Images/GetImage/' + imageid;
        md.append(img);
        var imageid = $(this).attr("value");

        $(imageid).modal('hide')




    })



})();
(function () {
    var $ref = $('._next');
    img = new Image();



    $ref.on('click', function (e) {



        var imageid = $(this).attr("data-target").slice(1),
            temp = "_" + imageid,
            md = $(temp).find($(".top_image"));
        $(temp).appendTo("body");
        e.preventDefault();


        img.src = '/Images/GetImage/' + imageid;
        md.append(img);
        var imageid = $(this).attr("value");

        $(imageid).modal('hide')




    })



})();
(function () {
    var $ref = $('._prev');
    img = new Image();



    $ref.on('click', function (e) {



        var imageid = $(this).attr("data-target").slice(1),
            temp = "#" + imageid,
            md = $(temp).find($(".top_image"));
        $(temp).appendTo("body");
        e.preventDefault();


        img.src = '/Images/GetImage/' + imageid;
        md.append(img);
        var imageid = $(this).attr("value");

        $(imageid).modal('hide')




    })



})();
(function () {
    var $ref = $('.show-small-image'),

    img = new Image();


    $ref.on('click', function (e) {



        var imageid = $(this).attr("data-target").slice(2),
            temp = "#_" + imageid,
            md = $(temp).find($(".top_image"));
        $(temp).appendTo("body");
        e.preventDefault();


        img.src = '/Images/GetImage/' + imageid;
        md.append(img);

    })



})();
(function () {
    var $likeBtn = $('.like'),
     imageId;


    $likeBtn.on('click', function (e) {
       
        
         
        var islikeme = $(this).children(".like-status").html();
        imageId = $(this).attr("value");
        $.ajax({


            type: "GET",
            url: "/Images/UpdateLike",

            data: {
                imageId: imageId,
                isitLikeMe: islikeme
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",

            success: successFunc,
            error: errorFunc
        });


    })

    function successFunc(data, status) {
        $('#' + imageId).find($(".like").children(".like-status")).text(data)
        $('#_' + imageId).find($(".like").children(".like-status")).text(data);


    }
    function errorFunc(errorData) {
        alert('Ошибка' + errorData.responseText);
    }

})();