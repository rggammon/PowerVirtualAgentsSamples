fetch("api/me")
    .then(function (response) {
        return response.json();
    })
    .then(function (data) {
        store.signInName = data.signInName;
    });

var navbar = new Vue({
    el: '#navbar',
    data: {
        store
    }
});

const routes = [
    { path: '/', component: httpVueLoader('templates/home.vue') },
    { path: '/setup', component: httpVueLoader('templates/setup.vue') }
];

const router = new VueRouter({
    routes
});

const app = new Vue({
    router
}).$mount('#app');

// Todo:
// https://docs.microsoft.com/en-us/azure/active-directory/develop/v2-oauth2-implicit-grant-flow#refreshing-tokens
// https://docs.microsoft.com/en-us/outlook/rest/javascript-tutorial