﻿var counter = 1;
$(function () {
	//ajout d'une ligne
    (function AddTR  (name) {
		$('<tr id="tablerow' + counter + '">' +
				'<td>' +
					'<input class="form-control" name="subName' + counter + '" type="text" placeholder="Entrer un nom..." />' +
				'</td>'+
				'<td>' +
					'<input  class="form-control" name="subCost" type="number' + counter + '" placeholder="Entrer un montant..." />' +
				'</td>'+
				/*'<td>'+
					'<a class="btn btn-success text-white" name="modifybtn">Sauvegarder/Modifier</a>'+
				'</td>'+*/
            '</tr>').appendTo('#tableSubExpenses' + name);
		counter;
		return false;
	});
});
function removeTr(index) {
	if (counter > 1) {
		$('#tablerow' + index).remove();
		counter--;
	}
	return false;
}
