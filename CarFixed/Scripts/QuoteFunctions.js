var displayTimes = false;


function firstPageNext() {
    if ($("#page1 input").valid() == true) {
        $("#page1, #page2").toggleClass("hidden");
    }
}

function firstPageClose() {
    $(".quote-popup").toggleClass("hidden");
    $("html").toggleClass("noscroll");
}

function secondPageBack() {
    $("#page1, #page2").toggleClass("hidden");
}

function secondPageNext() {

    $("#_DivBasicSubCategoryServices").html('');
    $("#_DivBasicSubCategoryTimes").html('');

    displayTimes = false;

    var dropDown = $("#_DrpSubCategory").data("kendoDropDownList");
    var dataItem = dropDown.dataItem(dropDown.select());

    if (dataItem.IsLinkedToRepairTime == true) {
        //Get Repairs
        displayTimes = true;

        $.ajax({
            url: $("#_DivBasicSubCategoryTimes").data('request-url'),
            type: 'post',
            data: '{ subCategoryId:' + dataItem.BasicSubCategoryID + '}',
            contentType: 'application/json',
            success: function (result) {

                $("#_DivBasicSubCategoryTimes").html(result);

                $("#page2, #page3").toggleClass("hidden");

            }
        });
    } else if (dataItem.IsLinkedToService == true) {
        //Get Services
        displayTimes = true;

        $.ajax({
            url: $("#_DivBasicSubCategoryServices").data('request-url'),
            type: 'post',
            data: '{ subCategoryId:' + dataItem.BasicSubCategoryID + '}',
            contentType: 'application/json',
            success: function (result) {

                $("#_DivBasicSubCategoryServices").html(result);

                $("#page2, #page3").toggleClass("hidden");

            }
        });

    }

    //Show Times????? Page 3

    //Or Show Address?? Page 4
    if (!displayTimes) {
        $("#page2, #page4").toggleClass("hidden");
    }
}

function thirdPageBack() {
    $("#page2, #page3").toggleClass("hidden");
}

function thirdPageNext() {
    $("#page3, #page4").toggleClass("hidden");
}

function fourthPageBack() {

    if (displayTimes) {
        $("#page3, #page4").toggleClass("hidden");
    }
    else {
        $("#page2, #page4").toggleClass("hidden");
    }
//Back to page 2 or 3

}

function fourthPageSend(quoteSubmitUrl) {
    var itemsToValidate = $("#page4 .k-textbox:enabled");
    var hold = $('#_FrmSubmit').serialize();
    if (itemsToValidate.valid() == true) {
        $.ajax({
            url: quoteSubmitUrl,
            type: 'post',
            data: hold,
            success: function (result) {
                if (result.success) {
                    //whatever you wanna do after the form is successfully submitted
                    $(".quote-popup").toggleClass("hidden");
                    $("html").toggleClass("noscroll");
                    window.location.reload();
                }
                else {
                    alert(result.msg);
                }
            }
        });

    }
}

$(document).on("click", ".quote-popup", function () {
    $(this).toggleClass("hidden");
    $("html").toggleClass("noscroll");
    $("#popup").toggleClass("active");
});

$(document).on("click", ".close", function () {
    $(".quote-popup").toggleClass("hidden");
    $("html").toggleClass("noscroll");
    $("#popup").toggleClass("active");
});

$(document).on("click", "#popup", function (e) {
    e.stopPropagation();
});

function filterSubCats() {
    return {
        categoryId: $("#_DrpCategory").val()
    };
}

function wrongCar() {
    $(".vehicle-details").addClass("hidden"); 
    $('#quote-vrm').val("");
    $('#homepage-vrm-input').val("");
    //$("").removeClass("hidden"); MANUAL LOOKUP
}