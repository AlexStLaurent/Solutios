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
        c = $('#color').val();
        id += 1;

        var t = { Spending: d, amount: p, color:c }
        test.push(t);

        myarray.push(parseInt(p));        
        var markup = "<tr><td>" + d + " <input type='hidden' value=" + d + "> </td><td>" + p + "$ <input type='hidden' value=" + p + "><td>" + c + " <input type='hidden' value=" + c +"></td></td><td><button type='button' onclick='Delete(" + id + ")' name= 'delete-row-" + id + "' class='delete btn btn-secondary'>Supprimer</button></td></tr>";
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
    margee = $('#marge').val();
    var marge = { Spending: "MargeSoumis", amount: margee, color: "#FFFF" }
    var margep = { Spending: "MargeProjeter", amount: margee, color: "#FFFFF" }
    test.push(marge);
    test.push(margep);
    objectArray = JSON.stringify(test);
    var markup = "<tr><td><input type='hidden' name='table' value = " + objectArray +" required></td></tr>";
    $("table tbody").append(markup);
}

$(document).ready(function () {
    $('#btnAjouterProjet').click(function () {
        sendtable();
    });
});


/*function addTR(name) {

    alert(name);
    $('<tr id="tablerow' + counter + '">' +
        '<td>' +
        '<input class="form-control" name="subName' + counter + '" type="text" placeholder="Entrer un nom..." />' +
        '</td>' +
        '<td>' +
        '<input  class="form-control" name="subCost" type="number' + counter + '" placeholder="Entrer un montant..." />' +
        '</td>' +
        /*'<td>'+
            '<a class="btn btn-success text-white" name="modifybtn">Sauvegarder/Modifier</a>'+
        '</td>'+*//*
        '</tr>').appendTo("#Table" + name);
    counter;
    return false;
}*/
