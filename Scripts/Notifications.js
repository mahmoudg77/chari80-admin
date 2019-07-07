
var Delay_Notify = 10;
var Delay_MSGCount = 10;
var timeout_handles = [];

function set_time_out(id, code, time) /// wrapper
{
    if (id in timeout_handles) {
        clearTimeout(timeout_handles[id]);
    }

    timeout_handles[id] = setTimeout(code, time * 1000);
}


function getLastNotys() {
    $.ajax({
        type: "POST",
        data: "",
        url:"/notifies/UnNotified",
        dataType: "json",
        success: function (rsp) {
            if (rsp.type == "success") {
                if (rsp.data.length == 0 || rsp.data == null) {
                    $("#smscount").html("");
                } else {
                    $("#smscount").html(rsp.data.length);
                }
                
                var listnoty = '';
                rsp.data.forEach(function (m) {
                    listnoty += '  <li class="">\
                                            <a target="_blank" href="' + (m.link == '' ? 'javascript:void(0)' : m.link) + '" data-id=" ' + m.id + '" class="notify-link">\
                                                      <div class="row">\
                                                        <div class="col-xs-2">\
                                                                <img src="/Content/imgs/photo.jpg"/>\
                                                        </div>\
                                                        <div class="col-xs-10">\
                                                            <b>' + m.from + ':</b> \
                                                            <p>'  + m.subject + '</p>\
                                                        </div>\
                                                    </div>\
                                             </a>\
                                 </li>';
                });


                $("#notifylist").html(listnoty);

               
            } else {

                $("#notifylist").html("error");

            }
            set_time_out('getLastNotys', "getLastNotys()", Delay_Notify);

        },
        error: function (xhr, ajaxOptions, thrownError) {
            set_time_out('getLastNotys', "getLastNotys()", Delay_Notify);
            $("#notifylist").html(xhr.responseText);
        }
    });

}


$(function () {

    getLastNotys();
    $("body").on("click", ".notify-link", function () {
        var btn =$(this);
        $.ajax({
            type: "POST",
            data: { id: btn.data('id') },
            url: "/notifies/Notify",
            dataType: "json",
            success: function (rsp) { },
            error: function () { },
        });
    });
});

function error(msg) {
    iziToast.show({
        title: 'Error',
        message: msg,
        color: 'red', // blue, red, green, yellow
        position: 'topCenter',
    });
}

function success(msg) {
    iziToast.show({
        title: 'Success',
        message: msg,
        color: 'green', // blue, red, green, yellow
        position: 'topCenter',
    });
}

function Alert(msg) {
    iziToast.show({
        title: 'Alert',
        message: msg,
        color: 'yellow', // blue, red, green, yellow
        position: 'topCenter',
    });
}