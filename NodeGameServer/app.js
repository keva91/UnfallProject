

var http = require('http');
var express = require('express');
var app = express();
var server = http.createServer(app);

// Chargement de socket.io
var io = require('socket.io')(server);

var port = process.env.PORT || 3000;

// Start server
server.listen(port,function(){
    console.log('server is listenning')
});


//first route
app.get('/', function (req, res) { 
    res.send('send back');
    console.log(" first route") 
}); 



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





