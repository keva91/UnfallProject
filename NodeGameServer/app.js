

var http = require('http');
var express = require('express');
var app = express();
var server = http.createServer(app);

// Chargement de socket.io
var io = require('socket.io').listen(server); 



//first route
app.get('/', function (req, res) { 
    res.send('send back');
    console.log(" first route") 
}); 



io.sockets.on('connection', function (socket, pseudo) {
    // Quand un client se connecte, on lui envoie un message
    socket.emit('message', 'Vous êtes bien connecté !');
    // On signale aux autres clients qu'il y a un nouveau venu
    socket.broadcast.emit('message', 'Un autre client vient de se connecter ! ');

    // Dès qu'on nous donne un pseudo, on le stocke en variable de session
    socket.on('petit_nouveau', function(pseudo) {
        socket.pseudo = pseudo;
    });

    socket.on('Connexion', function(pseudo) {
        console.log("connexion etablie !!")
    });

    // Dès qu'on reçoit un "message" (clic sur le bouton), on le note dans la console
    socket.on('message', function (message) {
        // On récupère le pseudo de celui qui a cliqué dans les variables de session
        console.log(socket.pseudo + ' me parle ! Il me dit : ' + message);
    }); 

    // socket.on('disconnect', function() { 
    //     console.log('disconnect'); 
    // }); 

});

server.listen(8000); 




