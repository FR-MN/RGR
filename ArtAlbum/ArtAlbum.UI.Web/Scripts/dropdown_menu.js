
var foo = $("#list_dropdowns");
foo.ready(function () {

    foo.find("ul").hide();

    foo.find("h3 span").click(function () {
        $(this).parent().next().slideToggle();
    });
});