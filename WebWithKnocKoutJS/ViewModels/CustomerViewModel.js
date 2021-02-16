
var Customer = function (customer) {
	this.CustId = ko.observable(customer.CustID);
	this.FirstName = ko.observable(customer.FirstName);
	this.LastName = ko.observable(customer.LastName);
	this.Email = ko.observable(customer.Email);
	this.Address = ko.observable(new Address(customer.Address));
};

var Address = function (address) {
	this.AddressID = ko.observable(address?.AddressID);
	this.Country = ko.observable(new Country(address?.Country));
	this.City = ko.observable(address?.City);
	this.Street = ko.observable(address?.Street);

	this.getAddress = this.Country().CountryName() + ', '
		+ (this.City() != null ? this.City() : ' ') + ', '
		+ (this.Street() != null ? this.Street() : ' ');
};

var Country = function (country) {
	this.CountryID = ko.observable(country?.CountryID);
	this.CountryName = ko.observable(country?.CountryName);
};

var CusViewModel = function () {
	var self = this;
	self.Countries = ko.observableArray([]);
	self.Cities = ko.observableArray([]);
	self.Streets = ko.observableArray([]);

	self.Customers = ko.observableArray([]);

	self.request = new DataManager();

	self.getCountries = function () {
		self.request.sendRequest("GET", "/api/Countries/")
			.done(function (data) {
				self.Countries($.map(data,
					function (country) { return country.CountryName }));
			});
	};

	self.getCities = function () {
		self.request.sendRequest("GET", "/api/Addresses/cities")
			.done(function (data) {
				self.Cities($.map(data,
					function (city) { return city }));
			});
	};

	self.getStreets = function () {
		self.request.sendRequest("GET", "/api/Addresses/streets")
			.done(function (data) {
				self.Streets($.map(data,
					function (street) { return street }));
			});
	};

	self.getCustomers = function () {
		self.request.sendRequest("GET", "/api/Customers/")
			.done(function (data) {
				self.Customers($.map(data,
					function (customer) { return new Customer(customer) }));
			});
	};

	var defaultCustomer = {
		CustID: 0,
		FirstName: "",
		LastName: "",
		Email: "",
		Address: {
			AddressID: 0,
			Country: {
				CountryID: 0,
				CountryName: "",
			},
			City: "",
			Street: "",
		}
	};

	self.chosedCustomer = ko.observable(new Customer(defaultCustomer));
	self.isCustomerChosed = ko.observable(false);
	self.getCustomerDetail = function (customer) {
		self.request.sendRequest("GET", "/api/Customers/" + customer.CustId())
			.done(function (data) {
				self.chosedCustomer(new Customer(data));
				self.isCustomerChosed(true);
			});
	};

	self.clearCustomer = function () {
		self.chosedCustomer(new Customer(defaultCustomer));
		self.isCustomerChosed(false);
	}

	self.cancel = function () {
		self.clearCustomer();
	};

	self.addCustomer = function (customer) {
		self.request.sendRequest("POST", "/api/Customers/", ko.toJSON(customer.chosedCustomer()))
			.done(function () {
				self.clearCustomer();
				updateData();
				alert("Customer is successfully added");
			});
	};

	self.updateCustomer = function (customer) {
		self.request.sendRequest("PUT", "/api/Customers/", ko.toJSON(customer.chosedCustomer()))
			.done(function () {
				updateData();
				alert("Customer is successfully updated");
				self.cancel();
			});
	};

	self.deleteCustomer = function (customer) {
		self.request.sendRequest("DELETE", "/api/Customers/" + customer.CustId())
			.done(function (data) {
				if (data) {
					updateData();
					alert("Customer is successfully deleted");
				}
			});
	};

	self.getChartLine = function () {
		self.request.sendRequest("GET", "/api/Customers/chart")
			.done(function (data) {
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
			});
	};

	function updateData() {
		self.getChartLine();
		self.getCustomers();
		self.getCountries();
		self.getCities();
		self.getStreets();
	};

	updateData();
};

ko.applyBindings(new CusViewModel()); 
