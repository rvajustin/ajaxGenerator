(function() {
    const self = { };
    const u = 'undefined';
    const w = window;

    function _ajax(method, url, id, area, controller, action, data, ignoreErrors, options) {
        const defaultOptions = {
            method: method,
            url: url,
            area: area,
            controller: controller,
            action: action,
            ignoreErrors: ignoreErrors || false
        };

        options = _merge(defaultOptions, options || { });
        return _invoke(options, data);
    }

    function _onError(xhr, message, options) {

    }

    _onError = $ERROR_CALLBACK;

    function _invoke(options, data) {
        const settings = {
            url: options.url,
            type: options.method,
            data: data,
            dataType: options.dataType || 'json',
            headers: {'X-Requested-With': 'XMLHttpRequest', 'X-Ignore-Errors': options.ignoreErrors.toString()},
            error: function (xhr, message) {
                if (options.ignoreErrors || (xhr && xhr.getResponseHeader('X-Ignore-Errors') === 'true')) {
                    return;
                }
                _onError(xhr, message, options);
            }
        };

        const xhr = self.__last_xhr = options.xhr = jQuery.ajax(settings);
        self.__last_xhr.options = options;
        return _merge(options, xhr);
    }

    function _merge(obj1, obj2) {
        let merged = obj1 || {};
        obj2 = obj2 || { };

        for (let attr in obj2) {
            merged[attr] = obj2[attr];
        }

        return merged;
    }

    function _window() {
        return w;
    }
    
    //**DECLARATIONS**//
    //**FUNCTIONS**//

    return self;
})();