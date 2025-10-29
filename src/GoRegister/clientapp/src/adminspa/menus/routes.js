import MenuList from "./pages/MenuList";
import MenuItemCreateEdit from "./pages/MenuItemCreateEdit";

const routes = [
  {
    path: "/menu",
    component: {
      render: (c) => c("router-view"),
    },
    children: [
      {
        name: "menu-list",
        path: "",
        component: MenuList,
        props: true,
      },
      {
        name: "menu-item-edit",
        path: "edit/:id",
        component: MenuItemCreateEdit,
        props: true,
      },      
      {
        name: "menu-item-create",
        path: "create",
        component: MenuItemCreateEdit,
        props: true,
      },
    ],
  },
];

export default routes;