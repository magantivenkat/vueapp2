// Creates Burger Menu when standard menu no longer fits on screen

function navMenuResize() {
    var main = document.getElementsByTagName("main");
    var element = document.getElementsByTagName("nav");
    var burgerMenu = document.getElementsByClassName("burger-menu-bars");

    // Set values to default to work out dims
    //extract in class - .collapsed ?
    element[0].style.display = "flex";
    element[0].style.flexDirection = "column";
    
    console.log("main" ,main[0].clientWidth);

    if (main[0].clientWidth <= 900) {
        element[0].style.display = "none";
        burgerMenu[0].style.display = "inline-block";
        element[0].style.flexDirection = "column";
        element[0].style.width = "100%";
        main[0].style.flexDirection = "column";
    }
    else {
        element[0].style.display = "flex";
        burgerMenu[0].style.display = "none";
        main[0].style.flexDirection = "row";
        element[0].style.removeProperty("width");
    }
}

window.addEventListener("resize", navMenuResize);

function burgerMenuChange(x) {
    if (x.classList) {
        x.classList.toggle("change");
    } else {
        // For IE9
        var classes = x.className.split(" ");
        var i = classes.indexOf("change");

        if (i >= 0)
            classes.splice(i, 1);
        else
            classes.push("change");
        x.className = classes.join(" ");
    }

    var nav = document.getElementsByTagName("nav");

    if (nav[0].style.display == "none") {
        nav[0].style.display = "flex";
    }
    else {
        nav[0].style.display = "none";
    }
}