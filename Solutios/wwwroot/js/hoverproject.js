﻿$(document).ready(function () {
    $("#test").hover(function () {
        $(this).css("background-color", "yellow");
    }, function () {
        $(this).css("background-color", "pink");
    });
});