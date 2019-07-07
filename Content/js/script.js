function success(message) {
    iziToast.show({
        title: 'Success',
        message: message,
        color: 'green', // blue, red, green, yellow
        position: 'topCenter',
    });
}
function error(message) {
    iziToast.show({
        title: 'Error',
        message: message,
        color: 'red', // blue, red, green, yellow
        position: 'topCenter',
    });
}

$(function () {

    //$('.dataTable').each(function () {
    //    var tfoot = $("<tfoot></tfoot>").append($(this).find("thead tr").clone());
    //    tfoot.appendTo($(this));
    //});

    //});
    $('.color').colorpicker({});

    //var t = $('.dataTable').DataTable({
    //    pageLength: 50,
    //    dom: 'Bfrtip',
    //    //bStateSave: true,
    //    buttons: [
    //        'pageLength',/*'copy','csv',*/ 'excel', 'pdf', 'print'
    //    ],

    //    initComplete: function () {
    //        var a = $(this).find("thead tr:eq(0)").clone();
    //        a.insertBefore($(this).find("thead tr:eq(0)"));

    //        this.api().columns().every(function () {
    //            var column = this;
    //            if ($.trim($(column.header()).html()) == "" || $.trim($(column.header()).html()) == "SN") return;
    //            var select = $('<select style="width:100%" ><option value=""></option></select>')
    //                .appendTo($(column.header()).empty())
    //                .on('change', function () {
    //                    var val = $.fn.dataTable.util.escapeRegex(
    //                        $(this).val()
    //                    );

    //                    column
    //                        .search(val ? '^' + val + '$' : '', true, false)
    //                        .draw();
    //                });

    //            column.data().unique().sort().each(function (d, j) {
    //                if (column.search() === '^' + d + '$') {
    //                    select.append('<option value="' + d + '" selected="selected">' + d + '</option>')
    //                } else {
    //                    select.append('<option value="' + d + '">' + d + '</option>')
    //                }
    //            });

    //        });

    //    }
    //});
    //t.on('order.dt search.dt print', function () {
    //    if ($.trim($(this).find("thead th:first").html()) == "SN" || $.trim($(this).find("thead th:first").html()) == "SN.") {

    //        t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
    //            cell.innerHTML = i + 1;
    //        });
    //    }
    //}).draw();

    $(".dataTable").dataTable();

    $("a.ajax-link").click(function (e) {
        e.preventDefault();
        var btn = $(this);

        if (btn.closest("ul.nav-tabs")) {
            btn.closest("ul.nav-tabs").find("li.active").removeClass("active");
            btn.parent().addClass("active");

        }
        $(btn.data("target")).html("<div class='text-center'><img src='/Content/imgs/loading-bar.gif'/></div>");

        $(btn.data("target")).load(btn.attr("href"), function (data, status, xhr) {

            if (status == "error") {
                $(btn.data("target")).html("<div class='alert alert-danger text-center'><strong>Sorry; </strong>" + xhr.status + " " + xhr.statusText + "</div>");
                return;
            }

            $(btn.data("target") + " .back").hide();

            $(btn.data("target") + " form").ajaxForm({
                dataType: "json",
                success: function (data) {


                    if (data.type == 'success') {
                        iziToast.show({
                            title: 'Success',
                            message: data.message,
                            color: 'green', // blue, red, green, yellow
                            position: 'topCenter',
                        });

                        eval(data.success);
                    } else {
                        iziToast.show({
                            title: 'Error',
                            message: data.message,
                            color: 'red', // blue, red, green, yellow
                            position: 'topCenter',
                        });
                    }



                },
                error: function (data, status, xhr) {
                    // var obj = JSON.parse(data.responseText);
                    iziToast.show({
                        title: 'Error',
                        message: data.status + " " + xhr,
                        color: 'red', // blue, red, green, yellow
                        position: 'topCenter',
                    });
                }
            });

            //additional plugins
            $(btn.data("target") + " .dataTable").dataTable();
            $(btn.data("target") + " .nav-tabs .active a").first().click();


            //$(btn.data("target") + ' .dataTable').each(function () {
            //    var tfoot = $("<tfoot></tfoot>").append($(this).find("thead tr").clone());
            //    tfoot.appendTo($(this));

            //});
            //var t = $(btn.data("target") + ' .dataTable').DataTable({
            //    pageLength: 50,
            //    dom: 'Bfrtip',
            //    bStateSave: true,
            //    buttons: [
            //        'pageLength',/*'copy','csv',*/ 'excel', 'pdf', 'print'
            //    ],

            //    initComplete: function () {
            //        this.api().columns().every(function () {
            //            var column = this;
            //            if ($.trim($(column.header()).html()) == "" || $.trim($(column.header()).html()) == "SN") return;
            //            var select = $('<select style="width:100%"><option value=""></option></select>')
            //                .appendTo($(column.footer()).empty())
            //                .on('change', function () {
            //                    var val = $.fn.dataTable.util.escapeRegex(
            //                        $(this).val()
            //                    );

            //                    column
            //                        .search(val ? '^' + val + '$' : '', true, false)
            //                        .draw();
            //                });

            //            column.data().unique().sort().each(function (d, j) {
            //                if (column.search() === '^' + d + '$') {
            //                    select.append('<option value="' + d + '" selected="selected">' + d + '</option>')
            //                } else {
            //                    select.append('<option value="' + d + '">' + d + '</option>')
            //                }
            //            });

            //        });
            //    }
            //});
            //t.on('order.dt search.dt print', function () {
            //    if ($.trim($(this).find("thead th:first").html()) == "SN" || $.trim($(this).find("thead th:first").html()) == "SN.") {

            //        t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            //            cell.innerHTML = i + 1;
            //        });
            //    }
            //}).draw();

            $('.date').datetimepicker({
                timepicker: false,
                ampm: true, // FOR AM/PM FORMAT
                format: 'Y-m-d',
            });
            $('.datetime').datetimepicker({
                timepicker: true,
                ampm: true, // FOR AM/PM FORMAT
                format: 'Y-m-d g:i A',
                step: 15
            });
            $('.time').datetimepicker({
                timepicker: true,
                ampm: true, // FOR AM/PM FORMAT
                format: 'g:i A',
                step: 5,
                minDate: new Date(),
                maxDate: new Date(),
                datepicker: false

            });
            $('.color').colorpicker({});
            $('.multiselect').multiselect({
                includeSelectAllOption: true,
                maxHeight: 200
                /*allSelectedText: 'All'*/
            });
        }, function (data, status, xhr) {
            $(btn.data("target")).html("<div class='alert alert-danger text-center'><strong>Sorry; </strong>" + xhr.status + " " + xhr.statusText + "</div>");

        });
    });

    $(".projects ul li.active > a").click();

    $("body").on("click", ".edit, .delete, .details, .modal-link", function (e) {
        var btn = $(this);

        e.preventDefault();

        if (btn.data("type") == "popup") {
            window.open(btn.attr("href"), "popupwindow","'toolbar=no,location = no,status = no,menubar = no,scrollbars = yes,resizable = yes,width = 1000,height =600'", true);
            return;
        }

        $('#myModal').modal('show');
        $("#myModal .modal-body").html("<div class='text-center'><img src='/Content/imgs/loading-bar.gif'/></div>");

        if (btn.hasClass("edit")) {
            $("#myModal .modal-title").html(document.viewTitle + " / " + "Edit");
        } else if (btn.hasClass("delete")) {
            $("#myModal .modal-title").html(document.viewTitle + " / " + "Delete");
        } else if (btn.hasClass("details")) {
            $("#myModal .modal-title").html(document.viewTitle + " / " + "Details");
        } else {
            $("#myModal .modal-title").html(btn.attr("title"));
        }


        if (btn.data("type") == "image") {
            $("#myModal .modal-body").html("<div class='text-center'><img src='" + btn.attr("href") + "' style='max-height:500px;max-width:100%'/></div>");
            return;
        }
        if (btn.data("type") == "iframe") {
            $("#myModal .modal-body").html("<div class='text-center'><iframe src='" + btn.attr("href") + "' style='max-height:500px;max-width:100%'/></iframe>");
            return;
        }
       

        $("#myModal .modal-body").load(btn.attr("href"), function (data, status, xhr) {



            if (status == "error") {
                $("#myModal .modal-body").html("<div class='alert alert-danger text-center'><strong>Sorry; </strong>" + xhr.status + " " + xhr.statusText + "</div>");
                return;
            }

            $("#myModal .modal-body  .back").hide();

            $("#myModal .modal-body form").ajaxForm({
                dataType: "json",
                success: function (data) {

                    if (data.type == 'success') {
                        iziToast.show({
                            title: 'Success',
                            message: data.message,
                            color: 'green', // blue, red, green, yellow
                            position: 'topCenter',
                        });

                        $('#myModal').modal('toggle');
                        if (window.location.href.indexOf("/Projects/Wizard/") > -1) {
                            $(".projects ul li.active > a").click();
                        } else {
                            window.location = window.location;
                        }
                    } else {
                        iziToast.show({
                            title: 'Error',
                            message: data.message,
                            color: 'red', // blue, red, green, yellow
                            position: 'topCenter',
                        });
                    }


                },
                error: function (data, status, xhr) {
                    // var obj = JSON.parse(data.responseText);
                    iziToast.show({
                        title: 'Error',
                        message: data.status + " " + xhr,
                        color: 'red', // blue, red, green, yellow
                        position: 'topCenter',
                    });
                }
            });

            $('.date').datetimepicker({
                timepicker: false,
                ampm: true, // FOR AM/PM FORMAT
                format: 'Y-m-d',
            });
            $('.datetime').datetimepicker({
                timepicker: true,
                ampm: true, // FOR AM/PM FORMAT
                format: 'Y-m-d g:i A',
                step: 15
            });
            $('.time').datetimepicker({
                timepicker: true,
                ampm: true, // FOR AM/PM FORMAT
                format: 'g:i A',
                step: 5,
                minDate: new Date(),
                maxDate: new Date(),
                datepicker: false

            });
            $('.color').colorpicker({});
            $('.multiselect').multiselect({
                includeSelectAllOption: true,
                maxHeight: 200
                /*allSelectedText: 'All'*/
            });
        }, function (data, status, xhr) {
            $("#myModal .modal-body").html("<div class='alert alert-danger text-center'><strong>Sorry; </strong>" + xhr.status + " " + xhr.statusText + "</div>");
        });

    });
    $("body").on("click", ".addnew", function (e) {
        e.preventDefault();
        var btn = $(this);
        $('#myModal').modal('show');

        $("#myModal .modal-body").html("<div class='text-center'><img src='/Content/imgs/loading-bar.gif'/></div>");
        $("#myModal .modal-body").load(btn.attr("href"), function (data) {

            $("#myModal .modal-title").html(document.viewTitle + " / " + "Create New");

            if (status == "error") {
                $("#myModal .modal-body").html("<div class='alert alert-danger text-center'><strong>Sorry; </strong>" + xhr.status + " " + xhr.statusText + "</div>");
                return;
            }

            $("#myModal .modal-body .back").hide();
            $("#myModal .modal-body form").ajaxForm({
                dataType: "json",
                success: function (data) {
                    if (data.type == 'success') {
                        iziToast.show({
                            title: 'Success',
                            message: data.message,
                            color: 'green', // blue, red, green, yellow
                            position: 'topCenter',
                        });

                        $('#myModal').modal('toggle');
                        if (window.location.href.indexOf("/Projects/Wizard/") > -1) {
                            $(".projects ul li.active > a").click();
                        } else {
                            window.location = window.location;
                        }
                    } else {
                        iziToast.show({
                            title: 'Error',
                            message: data.message,
                            color: 'red', // blue, red, green, yellow
                            position: 'topCenter',
                        });
                    }




                },
                error: function (data, status, xhr) {
                    // var obj = JSON.parse(data.responseText);

                    iziToast.show({
                        title: 'Error',
                        message: data.status + " " + xhr,
                        color: 'red', // blue, red, green, yellow
                        position: 'topCenter',
                    });
                }
            });

            $('.date').datetimepicker({
                timepicker: false,
                ampm: true, // FOR AM/PM FORMAT
                format: 'Y-m-d',
            });
            $('.datetime').datetimepicker({
                timepicker: true,
                ampm: true, // FOR AM/PM FORMAT
                format: 'Y-m-d g:i A',
                step: 15
            });
            $('.time').datetimepicker({
                timepicker: true,
                ampm: true, // FOR AM/PM FORMAT
                format: 'g:i A',
                step: 5,
                minDate: new Date(),
                maxDate: new Date(),
                datepicker: false

            });
            $('.color').colorpicker({});
            $('.multiselect').multiselect({
                includeSelectAllOption: true,
                maxHeight: 200
                /*allSelectedText: 'All'*/
            });
        }, function (data, status, xhr) {
            $("#myModal .modal-body").html("<div class='alert alert-danger text-center'><strong>Sorry; </strong>" + xhr.status + " " + xhr.statusText + "</div>");

        });

    });

    $("img.img-profile").on("error", function () { $(this).attr('src', '/Content/img/no-image-profile') })

    $("body").on("click", ".select-account", function () {
        $(".select-account").each(function () { $(this).removeClass("active") });
        $(this).addClass("active");
        $(this).closest(".account-select").find("input").val($(this).data("id"));
    })
    $("body").on("click", ".select-vehicle", function () {
        $(".select-vehicle").each(function () { $(this).removeClass("active") });
        $(this).addClass("active");
        $(this).closest(".vehicle-select").find("input").val($(this).data("id"));
    });

    $(".nav-tabs .active a").first().click();
});