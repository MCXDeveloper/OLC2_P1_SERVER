var util = require('util');
var path = require('path');
var _http = require('http');
var express = require('express');
var bodyparser = require('body-parser');

const app = express();
var http = new _http.Server(app);

/**
 * Express configuration.
 */
app.use(bodyparser.json());
app.use('/javascripts', express.static(path.join(__dirname, '/public/javascripts')));
app.set("port", process.env.PORT || 3000);

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
    res.sendFile('index.html', { root: path.join(__dirname, './views') });
});

module.exports = app;
