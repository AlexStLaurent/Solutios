var counter = 1;
var fakecounter = 2;
function addTR(name, id) {

    if (CheckTable(counter) != true) {   
        numberofcell = $("#rows" + id).val();
        numberofcell++;

        $('<tr id="tablerow' + numberofcell + '">' +
				'<td>' +
            '<input class="form-control" id="subName' + numberofcell + name + '" name="subName' + fakecounter + name+'" type="text" placeholder="Entrer un nom..." />' +
				'</td>'+
				'<td>' +
            '<input  class="form-control" type="number" id="subCost' + numberofcell + name + '" name="subCost' + fakecounter + name+'" placeholder="Entrer un montant..." />' +
				'</td>'+
				/*'<td>'+
					'<a class="btn btn-success text-white" name="modifybtn">Sauvegarder/Modifier</a>'+
				'</td>'+*/
        '</tr>').appendTo("#Table" + name);
        $("#rows" + id).val(numberofcell);
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
        subName = $("#subName" + i).val().replace(/ /g,"_");
        subCost = $("#subCost" + i).val();
        if (subCost == "" || subName == "" ) {
            return true            
        }
    }
}

function sendData() {
    
    numberofrow = $("#numberofrow").val();    
    for (let i = 1; i < numberofrow; i++) {
        var data = [];
        numberofcell = $("#rows" + i).val();
        cellname = $("#item" + i).val();

        for (var j = 1; j <= numberofcell; j++) {
            subName = $("#subName" + j + cellname).val().replace(/ /g, "_");
            subCost = $("#subCost" + j + cellname).val();
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