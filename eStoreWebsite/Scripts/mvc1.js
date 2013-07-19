
    //$(function () {
    //    $("#tabs").tabs();       // look for tabs element 
    //});


$(function () {             // look for tabs
    if ($("#tabs").length != 0) {
        $("#tabs").tabs({
            select: function (e, ui) {
                var validTab = $("#form0").valid(); 
                if (!validTab) {
                    e.preventDefault(); //don't let them go to new tab unless errors fixed
                }
            }
        })
    }
});



