

var http = require('http');
var express = require('express');
var app = express();
var server = http.createServer(app);

// Chargement de socket.io
var io = require('socket.io').listen(server); 
var fs = require("fs");

var port = process.env.PORT || 3000;



//first route
app.get('/', function (req, res) { 
    res.send('send back');
    console.log(" first route") 
}); 

app.get('/listScores', function (req, res) {
   fs.readFile( __dirname + "/" + "scores.json", 'utf8', function (err, data) {
       console.log( data );
       res.send( data );
   });
})

io.sockets.on('connection', function (socket, pseudo) {

    //console.log('someone')
    //console.log(socket)


    // Quand un client se connecte, on lui envoie un message
    socket.emit('message', {msg:'Vous êtes bien connecté !'});
    // On signale aux autres clients qu'il y a un nouveau venu
    socket.broadcast.emit('message', 'Un autre client vient de se connecter ! ');

 

    socket.on('connexion', function() {
        console.log("connexion etablie !!")
    });


    /*socket.on('disconnect', function() { 
        console.log('disconnect'); 
    }); */

});

// Start server
server.listen(port,function(){
    console.log('server is listenning')
});

