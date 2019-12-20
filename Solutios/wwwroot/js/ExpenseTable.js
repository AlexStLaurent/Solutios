//var counter = 1;
//var fakecounter = 2;
function addTR(name, id) {

    if (CheckTable(counter, name) != true) {   
        numberofcell = $("#rows" + id).val();
        numberofcell++;

        $('<tr id="tablerow' + numberofcell + '">' +
				'<td>' +
            '<input class="form-control" id="subName' + numberofcell + name.defaultValue + '" name="subName' + fakecounter + name.defaultValue+'" type="text" placeholder="Entrer un nom..." />' +
				'</td>'+
				'<td>' +
            '<input  class="form-control" type="number" id="subCost' + numberofcell + name.defaultValue + '" name="subCost' + fakecounter + name.defaultValue+'" placeholder="Entrer un montant..." />' +
				'</td>'+
				/*'<td>'+
					'<a class="btn btn-success text-white" name="modifybtn">Sauvegarder/Modifier</a>'+
				'</td>'+*/
        '</tr>').appendTo("#Table" + name.defaultValue);
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

function CheckTable(counter, name) {
    for (var i = 1; i <= counter; i++) {
        subName = $("#subName" + i + name.defaultValue).val();
        subCost = $("#subCost" + i + name.defaultValue).val();
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
        $("#Expensedata").append(markup);
    }
}

$(document).ready(function () {
    $('#btnsaveExpense').click(function () {
        sendData();
    });
});
    