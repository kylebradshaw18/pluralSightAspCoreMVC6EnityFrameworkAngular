// site.js
(function () {
  let $sidebarAndWrapper = $("#sidebar,#wrapper");
  let $icon = $("#sidebarToggle i.fa");

  $("#sidebarToggle").on("click", function () {
    $sidebarAndWrapper.toggleClass("hide-sidebar");
    if ($sidebarAndWrapper.hasClass("hide-sidebar")) {
      $icon.removeClass("fa-angle-left");
      $icon.addClass("fa-angle-right");
    } else {
      $icon.removeClass("fa-angle-right");
      $icon.addClass("fa-angle-left");
    }
  });
})();