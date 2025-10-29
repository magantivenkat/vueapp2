$(document).ready(function () {
    
    const cssRoot = document.documentElement;
    window.addEventListener("storage", (ev) => {    

        const k = `theme-preview`;

        if (ev.key.indexOf(k) !== -1) {
            var themeData = JSON.parse(ev.newValue);            

            const $themeCss = document.getElementById("gr-theme-css");
            if ($themeCss) {
                $themeCss.remove();
            }

            if (themeData.variables.length > 0) {
                themeData.variables.forEach(v => cssRoot.style.setProperty(v.name, v.value));
            }
            if (themeData.css) {                
                document.getElementById("preview-css").innerHTML = themeData.css;
            }
            if (themeData.headerHTML) {                
                document.getElementById("gr-header-container").innerHTML = themeData.headerHTML;
            }
            if (themeData.footerHTML) {                
                document.getElementById("gr-footer-container").innerHTML = themeData.footerHTML;
            }
            if (themeData.logoUrl) {                
                if (document.getElementById("gr-logo") != null)
                    document.getElementById("gr-logo").src = themeData.logoUrl
            }
            if (themeData.fonts.length > 0) {
                const previewFontLinks = document.getElementsByClassName("preview-font");
                themeData.fonts.forEach(f => {
                    for (var i = 0; i < previewFontLinks.length; i++) {
                        if (previewFontLinks[i].getAttribute("href") === f.link) return;
                    }

                    var link = document.createElement('link');
                    link.classList.add("preview-font");
                    link.setAttribute('rel', 'stylesheet');
                    link.setAttribute('type', 'text/css');
                    link.setAttribute('href', f.link);
                    document.head.appendChild(link);
                })
            }

            localStorage.removeItem(k);
        }
    });
});