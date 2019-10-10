// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $(".add-row").click(function () {
        var depense = $('#depense').val();
        var prix = $('#prix').val();
        var comment = $('#comment').val();
        $('#total').text(prix);
        var markup = "<tr><td>" + depense + "</td><td>" + prix + "$</td><td>" + comment +"</td><td><button type='button' class='delete btn btn-secondary'>Delete Row</button></td></tr>";
        $("table tbody").append(markup);
        formClear();
    });

    // Find and remove selected table rows
    $(".delete-row").click(function () {
        $("table tbody").find('input[name="record"]').each(function () {
            if ($(this).is(":checked")) {
                $(this).parents("tr").remove();
            }
        });
    });
});


function formClear() {
    $('#comment').val('');
    $('#prix').val('');
    $('#depense').val('');
}