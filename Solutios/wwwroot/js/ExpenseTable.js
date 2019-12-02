var counter = 1;
var fakecounter = 2;
function addTR(name) {

    if (CheckTable(counter) != true) {   

    $('<tr id="tablerow' + fakecounter + '">' +
				'<td>' +
        '<input class="form-control" id="subName' + fakecounter + '" name="subName' + fakecounter + '" type="text" placeholder="Entrer un nom..." />' +
				'</td>'+
				'<td>' +
        '<input  class="form-control" type="number" id="subCost' + fakecounter + '" name="subCost' + fakecounter + '" placeholder="Entrer un montant..." />' +
				'</td>'+
				/*'<td>'+
					'<a class="btn btn-success text-white" name="modifybtn">Sauvegarder/Modifier</a>'+
				'</td>'+*/
        '</tr>').appendTo("#Table" + name);
        $("#numberofrow" + name).val(counter);
        fakecounter++;
        counter++;
    }
        return false;
    }
	



function removeTr(index) {
	if (counter > 1) {
		$('#tablerow' + index).remove();
		counter--;
	}
	return false;
}

function CheckTable(counter) {
    

    for (var i = 1; i <= counter; i++) {
        subName = $("#subName" + i).val();
        subCost = $("#subCost" + i).val();
        if (subCost == "" || subName == "" ) {
            return true            
        }
    }
}

function sendData() {
    var data = [];
    numberofrow = $("#numberofrow").val();
    numberofcell = $("#numberofrow" + name).val();
    for (var i = 1; i <= numberofrow; i++) {
        for (var j = 1; j <= numberofcell; j++) {
            subName = $("#subName" + i).val();
            subCost = $("#subCost" + i).val();
            if (subCost != "" || subName != "") {
                var tablestr = { Spending: subName, amount: subCost, color: "#FFFF" }
                data.push(tablestr);
            }
        }
        objectArray = JSON.stringify(data);
        var markup = "<tr><td><input type='hidden' name='"+i+"' value = " + objectArray + " required></td></tr>";
        $("#TEST").append(markup);
    }
}

    //$('#btnAjouterProjet').click(function () {
    //    senddepense();
    //});