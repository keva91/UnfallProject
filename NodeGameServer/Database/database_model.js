

function database_model(){

	this.version = '0.0.1';

	var db = null;
	var mysql = require('mysql');
	var config = {
		host:'127.0.0.1',
		user:'root',
		password:'',
		database:'unfalldb'
	}

	this.connect = function(callback){
		db = mysql.createConnection(config);
		db.connect(function(err){
			if(err){
				console.error('error connecting mysql: '+err);
				return;
			}
			console.log('Connected to the database '+ config.database);
			callback(err);
		})
	}


}

module.exports = new database_model;


