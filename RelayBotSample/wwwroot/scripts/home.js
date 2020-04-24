var Home = {
    data: function () {
        return {
            botServices: []
        };
    },
    created: function() {
        this.fetchData();
    },
    watch: {
        // call again the method if the route changes
        '$route': 'fetchData'
    },
    methods: {
        fetchData: function() {
            var self = this;
            fetch("api/channelapps")
                .then(function (response) {
                    return response.json();
                })
                .then(function (data) {
                    self.botServices = data.value;
                });
        }
    }
};