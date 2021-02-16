
var DataManager = function () {
	this.sendRequest = function (type, url, data) {
		return $.ajax({
			type: type,
			url: url,
			contentType: 'application/json',
			dataType: 'json',
			data: data != null ? data : null,
		}).fail(function (error) {
			alert('Error ' + error.status + ' : ' + error.responseJSON.Message);
		});
	}
}
