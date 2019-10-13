// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
myarray = [,];

$(document).ready(function () {
    $(".add-row").click(function () {
        var depense = $('#depense').val();
        var prix = $('#prix').val();
        var comment = $('#comment').val();
        total = 0;
        myarray.push(parseInt(prix));
        total = myarray.reduce((a, b) => a + b, 0);
        $('#total').text(total.toString());
        var markup = "<tr><td>" + depense + "</td><td>" + prix + "$</td><td>" + comment + "</td><td><button type='button' onclick='Delete()' name= 'delete-row' class='delete btn btn-secondary'>Delete Row</button></td></tr>";
        $("table tbody").append(markup);
        formClear();
    });
});

    // Find and remove selected table rows
    function Delete() {
        $("table tbody").find('button[name="delete-row"]').each(function () {
            $(this).parents("tr").remove();
        });
    }

function formClear() {
    $('#comment').val('');
    $('#prix').val('');
    $('#depense').val('');
}