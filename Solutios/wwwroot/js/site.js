// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
myarray = [,];
id = 0;

$(document).ready(function () {
    $(".add-row").click(function () {
        var depense = $('#depense').val();
        var prix = $('#prix').val();
        var comment = $('#comment').val();
        id += 1;
        myarray.push(parseInt(prix));        
        var markup = "<tr><td>" + depense + "</td><td>" + prix + "$</td><td>" + comment + "</td><td><button type='button' onclick='Delete(" + id + ")' name= 'delete-row-" + id + "' class='delete btn btn-secondary'>Delete Row</button></td></tr>";
        $("table tbody").append(markup);
        showtotal();
        formClear();
    });
});

    // Find and remove selected table rows
function Delete(id) {
    $("table tbody").find("button[name='delete-row-"+ id + "']").each(function () {
        $(this).parents("tr").remove();
        delete myarray[id];
        showtotal();
        });
    }

function formClear() {
    $('#comment').val('');
    $('#prix').val('');
    $('#depense').val('');
}

function showtotal() {
    total = 0;
    total = myarray.reduce((a, b) => a + b, 0);
    $('#total').text(total.toString());
}