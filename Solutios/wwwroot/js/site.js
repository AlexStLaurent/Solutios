// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(document).ready(function () {
    $("#add-row").click(function () {
        //if ($('#depense').val() != "" && $('#prix').val() != "") {
        //    if (!isNaN($('#prix').val())) {
        //        d = $('#depense').val().replace(/ /g, "_");
        //        d = d.replace(/'/g, "_");
        //        p = $('#prix').val();
        //        c = $('#color').val();
        //        id += 1;

        //        var t = { Spending: d, amount: p, color: c }
        //        test.push(t);

        //        myarray.push(parseInt(p));
        var numberofitems = parseInt($("#numberofrows").val());
        var markup = "<tr>" +
            "<td><input class='form-control' type='text' id='depense" + numberofitems+"'></td>"+
            "<td> <input class='form-control' type='number' id='prix" + numberofitems +"' onchange='calculTotal()' ></td>"+
            "<td><input class='form-control' type='color' id='color" + numberofitems+"' /></td>" +
            "<td>" +
            "<button type='button' onclick='Delete(" + numberofitems + ")' name= 'delete-row-" + numberofitems + "' class='delete btn btn-secondary'><i class='fas fa-trash - alt'></i></button></td>" +
                    "</tr>";
        $("table tbody").append(markup);
        numberofitems += 1;
        $("#numberofrows").val(numberofitems);
        //        showtotal();
        //        formClear();
        //    }
        ////}
        //else {
        //    alert("Veuillez remplir les champs");
        //}
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

function calculTotal() {

    var total = 0;
    var numberofrow = $("#numberofrows").val();    
    for (let i = 0; i < numberofrow; i++) {
        prix = $("#prix" + i).val();

        if (prix != "") {
            prix = parseFloat(prix);
            total += prix;
        }
    }
    $('#total').text(total.toString());
}

function sendtable() {
    
    var data = [];
    numberofrow = $("#numberofrows").val();

    for (let i = 0; i < numberofrow; i++) {
        depence = $("#depense" + i).val().replace(/ /g, "_");
        depence = depence.replace(/'/g, "_");
        prix = $("#prix" + i).val();
        color = $("#color" + i).val();

        if (depence != "" || prix != "") {
            var tablestr = { Spending: depence, amount: prix, color: color }
            data.push(tablestr);
        }
    }

    margee = $('#margeprice').val();
    var marge = { Spending: "MargeSoumis", amount: margee, color: "#FFFF" }
    var margep = { Spending: "MargeProjeter", amount: margee, color: "#FFFFF" }
    data.push(marge);
    data.push(margep);
    objectArray = JSON.stringify(data);
    var markup = "<input type='hidden' name='table' value = " + objectArray + " required>";
    $("table tbody").append(markup);
}

$(document).ready(function () {
    $('#btnAjouterProjet').click(function () {
        sendtable();
    });

    $("#margeprice").change(function () {
        var marge = parseFloat($("#margeprice").val());
        total = parseFloat($('#total').text());
        pourcentage = (marge * 100) / total;
        totalMarger = total + marge;
        $('#totalmarge').text(totalMarger.toString());
        $('#margepourcent').val(pourcentage);
    });

    $("#margepourcent").change(function () {
        var pourcentage = parseFloat($("#margepourcent").val());
        total = parseFloat($('#total').text());
        marge = (pourcentage * total) / 100;
        totalMarger = total + marge;
        $('#totalmarge').text(totalMarger.toString());
        $('#margeprice').val(marge);
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
