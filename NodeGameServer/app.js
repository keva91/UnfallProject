

var http = require('http');
var express = require('express');
var app = express();
var server = http.createServer(app);
var bodyParser = require('body-parser');

// Chargement de socket.io
var io = require('socket.io').listen(server); 
var fs = require("fs");

var port = process.env.PORT || 3000;

var database_model = require('./Database/database_model');


database_model.connect(function(err_connect){
	if(err_connect){
		console.log(err_connect);
	}
	
})




app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: true })); 

//first route
app.get('/', function (req, res) { 
    res.send('send back');
    console.log(" first route") 
}); 

app.post('/user/pseudo', function (req, res) { 
    res.send('thank you');
    console.log(req.body)
    console.log("mon premier post") 
}); 

app.get('/listScores', function (req, res) {
   fs.readFile( __dirname + "/" + "scores.json", 'utf8', function (err, data) {
       console.log(data);
       console.log(data.length);
       res.send( data );
   });
})

io.sockets.on('connection', function (socket, pseudo) {

 


   console.log('socket connected: ' + socket.id);

    socket.on('disconnect', function(){
        console.log('socket disconnected: ' + socket.id);
    });

    socket.on('test-event1', function(){
        console.log('got test-event1');
    });

    socket.on('test-event2', function(data){
        console.log('got test-event2');
        console.log(data);

        socket.emit('test-event', {
            test:12345,
            test2: 'test emit event'
        });
    });

    socket.on('test-event3', function(data, callback){
        console.log('got test-event3');
        console.log(data);

        callback({
            test: 123456,
            test2: "test3"
        });
    });

});

// Start server
server.listen(port,function(){
    console.log('------- server is running -----')
});

