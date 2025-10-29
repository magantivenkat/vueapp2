/*MRF Changes: add new function for MRF requests
Modified Date: 22nd September 2022
Modified By: Mandar.Khade@amexgbt.com
Team member: Harish.Rame@amexgbt.com
JIRA Ticket No: GoRegister / GOR - 221
*/


//features
import regPathEdit from "./features/regpath-edit";
import regtypeCreate from "./features/regtype-create";
import customPageEdit from "./features/custompages/custompages-edit";
import themeEdit from "./features/theme-edit";
import delegates from "./features/delegates";
import formAdmin from "./features/form-admin";
import sessions from "./features/sessions-create";
import clientCreateEdit from "./features/client-create-edit";
import pagesDetails from "./features/pages-details";
import pagesIndex from "./features/pages-index";
import requestsIndex1 from "./features/requests-index1";
import projectsIndex from "./features/projects-index";
import projectsCreate from "./features/projects-create";
import projectsSettings from "./features/projects-settings";
import settingsPassword from "./features/settings-password";
import usersCreateEdit from "./features/users-create-edit";
import homeIndex from "./features/home-index";
import previewTheme from "./features/preview-theme";
import previewVersion from "./features/preview-version";
import tpnclientCreateEdit from "./features/tpnclient-create-edit";
import tpnclientemailCreateEdit from "./features/tpnclientemail-create-edit";
import tpnreportcreate from "./features/tpnreport-create";
import tpnclientMapGACreateEdit from "./features/tpnclientGAMap-create-edit";
import projectsManageMRF from "./features/projects-manageMRF";



export default function() {
  registerFeature("regpath-edit", regPathEdit);
  registerFeature("regtype-create", regtypeCreate);
  registerFeature("custompages-edit", customPageEdit);
  registerFeature("theme-edit",themeEdit);    
  registerFeature("delegates",delegates);    
  registerFeature("form-admin",formAdmin);    
  registerFeature("sessions",sessions);    
  registerFeature("client-create-edit",clientCreateEdit);    
  registerFeature("pages-details",pagesDetails);    
    registerFeature("pages-index", pagesIndex);
    registerFeature("requests-index1", requestsIndex1);
  registerFeature("projects-index", projectsIndex); 
  registerFeature("projects-create",projectsCreate);    
  registerFeature("projects-settings",projectsSettings);    
  registerFeature("settings-password",settingsPassword);    
  registerFeature("users-create-edit",usersCreateEdit);    
  registerFeature("home-index",homeIndex);      
    registerFeature("preview-version", previewVersion);
    registerFeature("tpnclient-create-edit", tpnclientCreateEdit);
    registerFeature("tpnclientemail-create-edit", tpnclientemailCreateEdit);
    registerFeature("tpnreport-create", tpnreportcreate);
    registerFeature("tpnclientGAMap-create-edit", tpnclientMapGACreateEdit);
    registerFeature("projects-manageMRF", projectsManageMRF);
    

  previewTheme();//This needs to be executed on every page so it is not registered

}

let pageConfig;
const registerFeature = function(pageTag, cb) {
  if(!pageConfig) {
    pageConfig = document.getElementById("page-config");
    if(!pageConfig) return;
  }

  if(pageConfig.dataset.page.toUpperCase() === pageTag.toUpperCase()) {
    cb(pageConfig.dataset);
  }
}