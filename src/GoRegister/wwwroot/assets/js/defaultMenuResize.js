// Creates Burger Menu when standard menu no longer fits on screen

function navMenuResize() {
    var element = document.getElementsByTagName("nav");
    var burgerMenu = document.getElementsByClassName("burger-menu-bars");
    var headerContent = document.getElementsByClassName("header-content");

    // Set values to default to work out dims
    //extract in class - .collapsed ?
    element[0].style.display = "flex";
    headerContent[0].style.flexWrap = "nowrap";
    element[0].style.flexDirection = "row";

    if (element[0].scrollWidth > element[0].clientWidth) {
        element[0].style.display = "none";
        burgerMenu[0].style.display = "inline-block";
        headerContent[0].style.flexWrap = "wrap";
        element[0].style.flexDirection = "column";
        element[0].style.width = "100%";
    }
    else {
        element[0].style.display = "flex";
        burgerMenu[0].style.display = "none";
        headerContent[0].style.flexWrap = "nowrap";
        element[0].style.flexDirection = "row";
        element[0].style.width = "initial";
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

    console.log(nav[0].style.display);

    if (nav[0].style.display == "none") {
        nav[0].style.display = "flex";
    }
    else {
        nav[0].style.display = "none";
    }
}