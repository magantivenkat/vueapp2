const grconfig = {
  baseUrl: "",
  baseProjectUrl: "",
  isProjectAdmin: true
};

const setupGrConfig = function() {
  const pageConfig = document.getElementById("admin-config");

  if(pageConfig) {
    grconfig.baseUrl = pageConfig.dataset.baseUrl;
    grconfig.baseProjectUrl = pageConfig.dataset.baseProjectUrl;
    grconfig.isProjectAdmin = grconfig.baseProjectUrl;
  }

}

export {
  grconfig,
  setupGrConfig
}