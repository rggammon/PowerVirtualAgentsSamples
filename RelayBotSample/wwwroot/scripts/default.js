fetch("api/configuration", { method: "GET", headers: { 'Accept': 'application/json' }, mode: "no-cors" })
    .then(function (response) {
        return response.json();
    })
    .then(function (data) {
        data.setupScript = window.location.href + "setup.sh";

        var app = new Vue({
            el: '#app',
            data: data
        });
    });