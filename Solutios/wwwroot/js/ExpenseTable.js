var counter = 1;
$(function () {
	//ajout d'une ligne
	$('#add').click(function () {
		$('<tr id="tablerow' + counter + '">' +
				'<td>'+
					'un test d\'une dépense'+
					'</td>'+
			'<td>' +
			'<input class="form-control" name="subName ' + counter + '" type="text" value="10,000" />' +
					'</td>'+
				'<td>'+
					'<a class="btn btn-success text-white" name="modifybtn0">Sauvegarder/Modifier</a>'+
				'</td>'+
			'</tr>').appendTo('#tableSubExpenses');
		counter++;
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
