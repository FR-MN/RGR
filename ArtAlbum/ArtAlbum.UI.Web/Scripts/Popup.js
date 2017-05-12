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