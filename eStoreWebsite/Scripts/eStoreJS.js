function setCurrentPageIndicator() {

    try {
        var item = $('#main-content').data('mnuitem');
        $("#" + item).attr("class", "active");

        var item = $('#main-content-orders').data('mnuitem');
        $("#" + item).attr("class", "active");
    }
    catch (err) { }

    try {
        var item2 = $('#main-content2').data('mnuitem');
        $("#" + item2).attr("class", "active");
    }

    catch (err) { }
}

function copyProdInfoToPopup(pcd) {
    $("#qty").val("0");
    $("#messg2").text("");
    $("#detailsGraphic").attr("src", $("#Graphic" + pcd).attr("src"));
    $("#detailsProdName").text($("#Name" + pcd).text());
    $("#detailsDescr").text($("#Descr" + pcd).data('description'));
    $("#detailsProdcd").val(pcd);
    $("#detailsPrice").text($("#Price" + pcd).text());
}

function init_map(map_canvas_id, lat, lng, myPositions) {
    $('#popupResults').find("tr").remove();

    tr = $("<tr>");
    td = $("<td>");
    td.text("Icon");
    tr.append(td);
    td = $("<td>");
    td.text("Branch #");
    tr.append(td);
    td = $("<td>");
    td.text("Distance");
    tr.append(td);
    td = $("<td>");
    td.text("Address");
    tr.append(td);
    tr.appendTo($("#popupResults"));

    var myLatLng = new google.maps.LatLng(lat, lng);
    var map_canvas = $('#' + map_canvas_id)[0];

    var options = {
        zoom: 10,
        center: myLatLng,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };

    var map = new google.maps.Map(map_canvas, options);
    var i2 = 0;
    var td, tr;

    for (i = 0; i < myPositions.length; i++) {
        i2 = i + 1;
        // Place a marker
        new google.maps.Marker({
            position: new google.maps.LatLng(myPositions[i].Latitude, myPositions[i].Longitude),
            map: map,
            icon: '/Content/eStoreImages/marker' + i2 + '.png',
            title: 'Store# ' + myPositions[i].BranchID
        });

        tr = $("<tr>");
        td = $("<td>");
        var img = $('<img />').attr({ 'src': "/Content/eStoreImages/marker" + i2 + ".png", 'alt': 'MyAlt' }).appendTo(td);
        td.css({ 'text-align': 'center;' });
        td.appendTo(tr)
        td = $("<td>");
        td.css({ 'padding-left': '10px;' });
        td.text(myPositions[i].BranchID);
        td.appendTo(tr)
        td = $("<td>");
        td.text(parseFloat(myPositions[i].Distance).toFixed(2) + " kms");
        td.appendTo(tr)
        td = $("<td>");
        td.text(myPositions[i].Street + ", " + myPositions[i].City + ", " + myPositions[i].Region);
        td.appendTo(tr);
        tr.appendTo($("#popupResults"));
    }

    $(location).attr('href', '#maps_popup');                                              // jump to the popup
}


var oldElem;
var tbl = null;
var orderId;
var orderDate;
var oldRow;
var oldClass;
var i2;
var xml;
var myWindow;
var ext = 0.0;
var data;
var popupTbl;

$(document).ready(function () {  // check to see if page has a listview
    if ($("#selectedRow").length != 0) {
        $("#selectedRow")[0].scrollIntoView(true);
    }

    $('#findthem').click(function () {                                                // will fire when client's button is clicked
        var address = $("#address")[0].value; // address textbox
        var locations;
        geocoder = new google.maps.Geocoder();                                        // A service for converting between an address to LatLng
        geocoder.geocode({ 'address': address }, function (results, status) {
            if (status == google.maps.GeocoderStatus.OK) {
                var lat = results[0].geometry.location.lat();
                var lng = results[0].geometry.location.lng();

                $.getJSON("/Branches/getJsonCloseBranches?&lat=" + lat + "&lng=" + lng, null, function (data) {
                    locations = data;
                    init_map('map_canvas', lat, lng, locations);
                });
            }
            //            $(location).attr('href', '/Branches/FindClosest?&lat=' + lat + '&lng=' + lng); // Pass the lat and lng to the server side code
        });
    });

    $(function () {             // load the tabs 
        if ($("#tabs").length != 0) {
            $("#tabs").tabs({
                select: function (e, ui) {
                    var validTab = $("#form1").valid();

                    if (!validTab) {
                        e.preventDefault(); //don't let them go to new tab unless errors fixed
                    }

                }

            })
        }
    });


    $('a').click(function (e) { // click on X to close
        if ($(this).parent().attr('id') == 'ordPopup') {
            //Cancel the link behavior
            e.preventDefault();
            $('#dialog, .popupWindow').hide();
        }
    }); // X - click

    $('td').click(function () { // click on a table cell in notes
        if ($(this).attr('id') == 'OrderNo') {  // if we have more than 1 table, we want the right one
            var id = '#ordPopup'; // empty table
            //Get the window height and width
            var winH = $(window).height();
            var winW = $(window).width();
            //Set the popup window to center
            $(id).css('top', winH / 2 - $(id).height() / 2);
            $(id).css('left', winW / 2 - $(id).width() / 2);
            $(id).show();

            if (oldRow != null) { // reset old row to previous state
                oldRow.removeAttr("style");
                oldRow.removeAttr("class");
                $(oldRow).toggleClass("itemSelected");
                $(oldRow).toggleClass(oldClass);
            }

            oldRow = $(this).parents("tr");
            oldClass = oldRow.attr("class");
            oldRow.removeAttr("style");
            oldRow.removeAttr("class");
            oldRow.toggleClass("itemSelected");
            orderId = $(this).text();
            orderDate = oldRow[0].cells[1].innerHTML;

            // build popup
            getOrders($.trim(orderId));
        }
    });
}); //(document).ready


function getOrders(oid) {
    $.getJSON("/Orders/getJsonOrders", null, function (data) {
        buildOrder(oid, orderDate, data);
    });
} // findOrder

function buildOrder(oid, odate, data) {

    var tbl = $('<table>');

    // delete rows from old table (if it exists)
    $(tbl).find("tr").remove();

    var tot = 0.0;

    // Row-->
    var tr = $("<tr>");
    var td = $("<td>");
    tr.css({ 'padding': '5px' });
    td.css({ 'width': '50%', 'text-align': 'center', 'font-weight': 'bold', 'font-size': 'small', 'border': 'none' });
    td.addClass("tblItemBigHead");
    td.text("Order: " + oid);
    td.appendTo(tr);       // add cell to row
    td = $("<td colspan='5'>");
    td.css({ 'width': '50%', 'text-align': 'right', 'font-weight': 'bold', 'border': 'none' });
    td.addClass("tblItemBigHead");
    td.text("Date: " + odate);
    td.appendTo(tr);       // add cell to table
    tr.appendTo(tbl);      // add row to table
    // Row-->
    tr = $("<tr>");
    td = $("<td>");
    td.css({ 'width': '43%', 'text-align': 'center', 'font-weight': 'bold' });
    tr.css({ 'padding': 'none' });
    td.addClass("tblItemHead");
    td.text("Product Name");
    td.appendTo(tr);       // add cell to row
    td = $("<td>");
    td.css({ 'width': '19%', 'text-align': 'center', 'font-weight': 'bold' });
    td.addClass("tblItemHead");
    td.text("Sell Price");
    td.appendTo(tr);       // add cell to row
    td = $("<td>");
    td.css({ 'width': '10%', 'text-align': 'center', 'font-weight': 'bold' });
    td.addClass("tblItemHead");
    td.text("QtyS");
    td.appendTo(tr);       // add cell to row
    td = $("<td>");
    td.css({ 'width': '10%', 'text-align': 'center', 'font-weight': 'bold' });
    td.addClass("tblItemHead");
    td.text("QtyO");
    td.appendTo(tr);       // add cell to row
    td = $("<td>");
    td.css({ 'width': '10%', 'text-align': 'center', 'font-weight': 'bold' });
    td.addClass("tblItemHead");
    td.text("QtyB");
    td.appendTo(tr);       // add cell to row
    td = $("<td>");
    td.css({ 'width': '15%', 'text-align': 'center', 'font-weight': 'bold' });
    td.addClass("tblItemHead");
    td.text("Extended");
    td.appendTo(tr);       // add cell to row
    tr.appendTo(tbl);      // add row to table
    // Row-->
    tr = $("<tr>");
    td = $("<td>");
    td.css({ 'width': '43%', 'text-align': 'center', 'font-weight': 'bold' });
    td.innerHTML = "&nbsp;";
    td.appendTo(tr);       // add cell to row
    tr.appendTo(tbl);      // add row to table

    $.each(data, function (index, order) {
        if (order.OrderID == oid) {   // make sure we have an order
            tr = $("<tr>");
            td = $("<td>");
            td.addClass("disNameItem");
            td.text(order.ProductName);
            td.appendTo(tr);       // add cell to row
            td = $("<td>");
            ext = 0.0;
            ext = parseFloat(order.SellingPrice);
            td.addClass("disItem");
            td.text(cur(order.SellingPrice));
            td.appendTo(tr);       // add cell to row
            td = $("<td>");
            td.css({ 'width': '10%', 'text-align': 'center', 'font-weight': 'bold' });
            td.addClass("disItem");
            td.text(order.QtyOrdered);
            td.appendTo(tr);       // add cell to row
            td = $("<td>");
            td.css({ 'width': '10%', 'text-align': 'center', 'font-weight': 'bold' });
            td.addClass("disItem");
            td.text(order.QtySold);
            td.appendTo(tr);       // add cell to row
            td = $("<td>");
            td.css({ 'width': '10%', 'text-align': 'center', 'font-weight': 'bold' });
            td.addClass("disItem");
            td.text(order.QtyBackOrdered);
            td.appendTo(tr);       // add cell to row
            td = $("<td>");
            ext = ext * parseInt(order.QtySold);
            td.css({ 'text-align': 'center', 'font-weight': 'bold' });
            td.addClass("disItem");
            td.text(cur(ext));
            td.appendTo(tr);       // add cell to row
            tr.appendTo(tbl);      // add row to table
            tot = tot + ext;
        } // if
    }); // for

    //Row-->
    tr = $("<tr>");
    td = $("<td colspan='6'>");
    td.text(" ");
    td.appendTo(tr);       // add cell to row        
    tr.appendTo(tbl);      // add row to table

    // Row-->
    tr = $("<tr>");
    td = $("<td colspan='5'>");
    td.text(" ");
    td.appendTo(tr);       // add cell to row        
    td = $("<td>");
    td.css({ 'text-align': 'center', 'font-weight': 'bold' });
    td.addClass("disItem");
    td.text("----------");
    td.appendTo(tr);       // add cell to row        
    tr.appendTo(tbl);      // add row to table

    //Row-->
    tr = $("<tr>");
    td = $("<td colspan='5'>");
    td.css({ 'text-align': 'right', 'font-weight': 'bold' });
    tr.css({ 'border': 'none', 'padding': 'none' });
    td.text("Total:");
    td.appendTo(tr);       // add cell to row        
    td = $("<td>");
    td.css({ 'text-align': 'right', 'font-weight': 'bold' });
    td.addClass("disItem");
    td.text(cur(tot));
    td.appendTo(tr);       // add cell to row        
    tr.appendTo(tbl);      // add row to table

    //Row-->
    tr = $("<tr>");
    td = $("<td colspan='5'>");
    td.css({ 'text-align': 'right', 'font-weight': 'bold' });
    td.text("Tax:");
    td.appendTo(tr);       // add cell to row        
    td = $("<td>");
    td.css({ 'text-align': 'right', 'font-weight': 'bold' });
    td.addClass("disItem");
    td.text(cur(tot * .13));
    td.appendTo(tr);       // add cell to row        
    tr.appendTo(tbl);      // add row to table

    //Row-->
    tr = $("<tr>");
    td = $("<td colspan='3'>");
    td.css({ 'text-align': 'right', 'font-weight': 'bold' });
    td.appendTo(tr);       // add cell to row        
    td = $("<td>");
    td = $("<td colspan='2'>");
    td.addClass("tblItemBigFoot");
    td.text("Order Tot:");
    td.appendTo(tr);       // add cell to row        
    td = $("<td>");
    td.addClass("tblItemBigFoot");
    td.text(cur(tot * 1.13));
    td.appendTo(tr);       // add cell to row        
    tr.appendTo(tbl);      // add row to table

    //Row-->
    tr = $("<tr>");
    td = $("<td colspan='6'>");
    td.text(" ");
    td.appendTo(tr);       // add cell to row        
    tr.appendTo(tbl);      // add row to table
    $('#popupTbl').find("tr").remove();      // clean out old stuff
    $('#popupTbl').append($(tbl).html());    // put in new stuff
//    var x = $('#popupTbl').html();

} //buildOrder


// obtained from the internet unknown source
function cur(num) {
    num = num.toString().replace(/\$|\,/g, '');
    if (isNaN(num))
        num = "0";
    sign = (num == (num = Math.abs(num)));
    num = Math.floor(num * 100 + 0.50000000001);
    cents = num % 100;
    num = Math.floor(num / 100).toString();
    if (cents < 10)
        cents = "0" + cents;
    for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3) ; i++)
        num = num.substring(0, num.length - (4 * i + 3)) + ',' +
        num.substring(num.length - (4 * i + 3));
    return (((sign) ? '' : '-') + '$' + num + '.' + cents);
}