(function () {
    var $ref = $('.show-image'),
    
    img = new Image();
    

    $ref.on('click', function (e) {
        
        

        var imageid = $(this).attr("data-target").slice(1),
            temp = "#" + imageid,
            md = $(temp).find($(".top_image"));
            $(temp).modal()
        e.preventDefault();
        
        
        img.src = '/Images/GetImage/' + imageid;
        md.append(img);

    })
       
     

    })();