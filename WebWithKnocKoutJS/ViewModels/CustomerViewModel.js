var Customer = function(customer){
	this.CustId = ko.observable(customer.CustID);
	this.FirstName = ko.observable(customer.FirstName);
	this.LastName = ko.observable(customer.LastName);
	this.Email = ko.observable(customer.Email);
	this.Country = ko.observable(customer.Country);
};

var CusViewModel = function () {
	var self = this;
	self.Countries = ko.observableArray(["Ukraine", "Poland", "Germany", "England"]);

	self.Customers = ko.observableArray([]);

	self.getCustomers = function () {
		$.ajax({
			type: "GET",
			url: "/api/Customers/",
			contentType: 'application/json',
			dataType: 'json',
			success: function (data) {
				self.Customers($.map(data,
					function (customer) { return new Customer(customer) }));
			},
			error: function (error) {
				alert('Error: ' + error.responseJSON.ExceptionMessage);
			}
		});
	};

	var defaultCustomer = {
		CustID: 0,
		FirstName: "",
		LastName: "",
		Email: "",
		Country: ""
	};

	self.chosedCustomer = ko.observable(new Customer(defaultCustomer));
	self.getCustomerDetail = function (customer) {
		$.ajax({
			type: "GET",
			url: "/api/Customers/" + customer.CustId(),
			contentType: 'application/json',
			dataType: 'json',
			success: function (data) {
				self.chosedCustomer(new Customer(data));
				$('#Update').show();
				$('#Cancel').show();

				$('#Save').hide();
				$('#Clear').hide();
			},
			error: function (error) {
				alert('Error: ' + error.responseJSON.ExceptionMessage);
			}
		});
	};

	self.clearCustomer = function () {
		self.chosedCustomer(new Customer(defaultCustomer));
	}

	self.cancel = function () {
		self.clearCustomer();
		$('#Update').hide();
		$('#Cancel').hide();

		$('#Save').show();
		$('#Clear').show();
	};

	self.addCustomer = function (customer) {
		$.ajax({
			type: "POST",
			url: "/api/Customers/",
			contentType: 'application/json',
			dataType: 'json',
			data: ko.toJSON(customer.chosedCustomer()),
			success: function () {
				self.clearCustomer();
				alert("Customer is successfully added");
				updateData();
			},
			error: function (error) {
				alert('Error: ' + error.responseJSON.ExceptionMessage);
			}
		});
	};

	self.updateCustomer = function (customer) {
		$.ajax({
			type: "PUT",
			url: "/api/Customers/",
			contentType: 'application/json',
			dataType: 'json',
			data: ko.toJSON(customer.chosedCustomer()),
			success: function () {
				alert("Customer is successfully updated");
				updateData();
				self.cancel();
			},
			error: function (error) {
				alert('Error: ' + error.responseJSON.ExceptionMessage);
			}
		});
	};

	self.deleteCustomer = function (customer) {
		$.ajax({
			type: "DELETE",
			url: "/api/Customers/" + customer.CustId(),
			contentType: 'application/json',
			dataType: 'json',
			success: function (data) {
				if (data) {
					alert("Customer is successfully deleted");
					updateData();
				}
			},
			error: function (error) {
				alert('Error: ' + error.responseJSON.ExceptionMessage);
			}
		});
	};

	self.getChartLine = function () {
		$.ajax({
			type: "GET",
			url: "/api/Customers/chart",
			contentType: 'application/json',
			dataType: 'json',
			success: function (data) {
				var customersPerCountries = $.map(data,
					function (array) { return array.CustomersCount; });
				var countries = $.map(data,
					function (array) {
						return array.Country;
					});

				new Chart(document.getElementById("line-chart").getContext("2d"), {
					type: 'line',
					data: {
						labels: countries,
						datasets: [{
							label: 'Count Of Customers Per Country',
							data: customersPerCountries,
							borderColor: "#5bc0de",
							lineTension: 0,
						}]
					}
				});		
			},
			error: function (error) {
				alert('Error: ' + error.responseJSON.ExceptionMessage);
			}
		});
	};

	function updateData() {
		self.getChartLine();
		self.getCustomers();
	};

	updateData();
};

ko.applyBindings(new CusViewModel()); 
