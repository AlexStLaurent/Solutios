// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
myarray = [,];
var test = [];
id = 0;

$(document).ready(function () {
    $(".add-row").click(function () {
        d = $('#depense').val();
        p = $('#prix').val();
        id += 1;

        var t = { Spending: d, amount: p }
        test.push(t);

        myarray.push(parseInt(p));        
        var markup = "<tr><td>" + d + " <input type='hidden' value=" + d + "> </td><td>" + p + "$ <input type='hidden' value=" + p +"></td><td><button type='button' onclick='Delete(" + id + ")' name= 'delete-row-" + id + "' class='delete btn btn-secondary'>Delete Row</button></td></tr>";
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
        delete test[id];
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

function sendtable() {
    objectArray = JSON.stringify(test);
    var markup = "<tr><td><input type='hidden' name='table' value = " + objectArray +"></td></tr>";
    $("table tbody").append(markup);
}

$(document).ready(function () {
    $('#btnAjouterProjet').click(function () {
        sendtable();
    });
});