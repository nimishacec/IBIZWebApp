window.blazorNavMenu = {
    registerResizeCallback: function (dotNetHelper) {
        window.addEventListener('resize', function () {
            dotNetHelper.invokeMethodAsync('OnWindowResize', window.innerWidth);
        });
    },
    getWindowWidth: function () {
        return window.innerWidth;
    }
};