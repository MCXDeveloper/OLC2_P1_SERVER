var util = require('util');
var path = require('path');
var cors = require('cors');
var _http = require('http');
var table = require('markdown-table');
var express = require('express');
var bodyparser = require('body-parser');
const session = require('express-session');

const app = express();
var http = new _http.Server(app);

/**
 * Express configuration.
 */
app.use(function(req, res, next) {
    res.header("Access-Control-Allow-Origin", "*");
    res.header("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
    next();
});
app.use(bodyparser.urlencoded());
app.use(session({secret: 'ocl2-p1-client'}));
app.use('/javascripts', express.static(path.join(__dirname, '/public/javascripts')));
app.use('/codemirror', express.static(path.join(__dirname, '/public/codemirror')));
app.use('/stylesheets', express.static(path.join(__dirname, '/public/stylesheets')));
app.use('/jstree', express.static(path.join(__dirname, '/public/jstree')));
app.use('/grammar', express.static(path.join(__dirname, '/public/analizadores')));
app.use('/media', express.static(path.join(__dirname, '/public/media')));
app.set("port", process.env.PORT || 3000);

let sess;

/**
 * Start Express server.
 */
http.listen(app.get("port"), () => {
    console.log(("App is running at http://localhost:%d in %s mode"), app.get("port"), app.get("env"));
    console.log("Press CTRL-C to stop\n");
});

/**
 * Primary app routes.
 */
app.get('/', function(req, res){
    sess = req.session;
    if(sess.user_name) {
        return res.redirect('/Home');
    }
    res.sendFile('index.html', { root: path.join(__dirname, './views') });
    //res.sendFile('ModoAvanzado.html', { root: path.join(__dirname, './views') });
});

app.get('/getActualUser', function(req, res){
    sess = req.session;
    res.end(sess.user_name);
});

app.post('/login',(req,res) => {

    sess = req.session;
    sess.user_name = req.body.userx;

    if(sess.user_name) {
        res.end('done');
    }else{
        res.end('nah');
    }

});

app.post('/buildTable', function(req, res){
    let content = req.body.querydata;
    res.end(table(content));
});

app.get('/logout',(req,res) => {
    sess = req.session;
    sess.user_name = undefined;
    res.end('done');
});

app.get('/Home', function(req, res){
    sess = req.session;
    if(sess.user_name) {
        res.sendFile('Principal.html', { root: path.join(__dirname, './views') });
    }
    else {
        return res.redirect('/');
    }
});

app.get('/ModoPrincipiante', function(req, res){
    sess = req.session;
    if(sess.user_name) {
        res.sendFile('ModoPrincipiante.html', { root: path.join(__dirname, './views') });
    }
    else {
        return res.redirect('/');
    }
});

app.get('/ModoIntermedio', function(req, res){
    sess = req.session;
    if(sess.user_name) {
        res.sendFile('ModoIntermedio.html', { root: path.join(__dirname, './views') });
    }
    else {
        return res.redirect('/');
    }
});

app.get('/ModoAvanzado', function(req, res){
    sess = req.session;
    if(sess.user_name) {
        res.sendFile('ModoAvanzado.html', { root: path.join(__dirname, './views') });
    }
    else {
        return res.redirect('/');
    }
});

module.exports = app;
