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

$(document).ready(function () {
    $("a.details").on("click", function (e) {
        pcd = $(this).attr("data-prodcd");
        $("#qty").val("0");
        $("#messg2").text("");
        $("#detailsGraphic").attr("src", $("#Graphic" + pcd).attr("src"));
        $("#detailsProdName").text($("#Name" + pcd).text());
        $("#detailsDescr").text($("#Descr" + pcd).data('description'));
        $("#detailsProdcd").val(pcd);
        $("#detailsPrice").text($("#Price" + pcd).text());
    });
});