import emails from "./emails/routes";
import delegates from "./delegates/routes";
import menus from "./menus/routes";
import themes from "./themes/routes";

const buildRoutes = function() {
  const r = [];
  // add routes
  r.push(...emails);
  r.push(...delegates);
  r.push(...menus);
  r.push(...themes);
  return r;
}

export default buildRoutes();