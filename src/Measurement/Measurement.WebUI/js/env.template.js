const environment = {
    getRequestUrl(from, to) {
        return `${window.location.protocol}//${window.location.hostname}:${API_PORT}/${API_URI}/` + encodeURIComponent(from) + '/' + encodeURIComponent(to);
    }
};