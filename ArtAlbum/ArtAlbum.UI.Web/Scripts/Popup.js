

    //var $ref = $('[aria-labelledby=myModalLabel]');

    


    //$.each($ref, function (e) {
    //    $("html,body").css("overflow", "auto");
    //});






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
    var $ref = $('[name = previmage ]');
    img = new Image();
   


    $ref.on('click', function (e) {
        //$('body').css('overflow', 'hidden');

        var imageid = $(this).attr("value");

        $(imageid).modal('hide')
        var imageid = $(this).attr("data-target").slice(1),
            temp = "#" + imageid,
            md = $(temp).find($(".top_image"));
        $(temp).appendTo("body");
        e.preventDefault();
        $(temp).modal("show");
        $("html,body").addClass("modal-open");
        img.src = '/Images/GetImage/' + imageid;


        md.append(img);


      

    })



})();
(function () {
    var $ref = $('[name = nextimage]');
    img = new Image();



    $ref.on('click', function (e) {
        //$('body').css('overflow', 'hidden');

        var imageid = $(this).attr("value");

        $(imageid).modal('hide')
        var imageid = $(this).attr("data-target").slice(1),
            temp = "#" + imageid,
            md = $(temp).find($(".top_image"));
        $(temp).appendTo("body");
        e.preventDefault();
        $(temp).modal("show");
        $("html,body").addClass("modal-open");
        img.src = '/Images/GetImage/' + imageid;
       
       
        md.append(img);
        
        



    })



})();
(function () {
    var $ref = $('[name = _previmage]');
    img = new Image();



    $ref.on('click', function (e) {


        var imageid = $(this).attr("value");

        $(imageid).modal('hide')
        var imageid = $(this).attr("data-target").slice(2),
            temp = "#_" + imageid,
            md = $(temp).find($(".top_image"));
        $(temp).appendTo("body");
        e.preventDefault();
        $(temp).modal("show");
        $("html,body").addClass("modal-open");
        img.src = '/Images/GetImage/' + imageid;


        md.append(img);




    })



})();
(function () {
    var $ref = $('[name = _nextimage]');
    img = new Image();



    $ref.on('click', function (e) {

        var imageid = $(this).attr("value");

        $(imageid).modal('hide')
        var imageid = $(this).attr("data-target").slice(2),
            temp = "#_" + imageid,
            md = $(temp).find($(".top_image"));
        $(temp).appendTo("body");
        e.preventDefault();
        $(temp).modal("show");
        $("html,body").addClass("modal-open");
        img.src = '/Images/GetImage/' + imageid;


        md.append(img);




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



(function () {
    var $ref = $('.submit'),
        textarea,
    correctimageid;

    $ref.on('click', function (e) {

       var imageId = $(this).attr("id");

       correctimageid = imageId.replace('submit', ''),
           temp = "#" + correctimageid,
           md = $(temp).find($(".commentText")),
           textarea = md.val(),
           authorId = md.attr("id");
      
       $(temp).appendTo("body");
        e.preventDefault();


        $.ajax({


            type: "GET",
            url: "/Images/AddComment",

            data: {
                commentText: textarea,
                imageId: correctimageid
               
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",

            success: successFunc,
            error: errorFunc
        });
       

    })
    function successFunc(data, status) {
        if (data[0] === true)
        {
            var temp = data[3];
            $('#' + correctimageid).find($("ul", ".comments")).append(
                ' <li> <div class="comment"><div class="reply_image"><a href="/id337666144"><img src="/Users/GetAvatar?userId=' + data[1] + '"  class="reply_img" ></a></div><div class="content_commetn"><div class="reply_author"><a class="author" href="/id337666144" data-from-id="337666144">' + data[2] + '</a></div><div class="reply_text"><div class="wall_reply_text">' + textarea + '</div></div></div><div class="date-of-creating">'+data[3]+'</div></div><hr/></li>'
                );
        }
        else {

        }
     
        
       


    }
    function errorFunc(errorData) {
        alert('Ошибка' + errorData.responseText);
    }



})();