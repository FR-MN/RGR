(function check2() {

    var $ref = $('[value=image]');
    
    

    $('[value=image]').on('click', function (e) {
        $('[name=userData]').attr('disabled', true);
        $('[name=searchQuery]').attr('disabled', false);
        $('[name=tagsData]').attr('disabled', false);

    })
    $('[value=user]').on('click', function (e) {
        $('[name=userData]').attr('disabled', false);
        $('[name=searchQuery]').attr('disabled', true);
        $('[name=tagsData]').attr('disabled', true);

    })
    $('input[name=r]:checked').val();
   
})();