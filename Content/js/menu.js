function setNavigation() {
    var baseUrl = window.location.protocol + "//" + window.location.host;
    var path = window.location.href;
    path = path.replace(baseUrl, "");
    //path = decodeURIComponent(path);
    console.log(path);

            $(".menu a").each(function () {
                var href = $(this).attr('href');
                
                if (path.substring(0, href.length) === href) {
                    $(this).closest('li').addClass('active');
                    $(this).closest('ul').closest('li').find(".menu-toggle").addClass('toggled');
                    $(this).closest('ul').addClass('open');
                    $(this).closest('li').addClass('active');
                    $(this).addClass('toggled');
                }
            });
        }