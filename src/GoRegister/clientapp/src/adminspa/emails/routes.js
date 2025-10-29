// import the project pages
import Index from "./pages/Index";
import CreateEdit from "./pages/CreateEdit";
import CreateEditLayout from "./pages/CreateEditLayout";
import Send from "./pages/Send";
import Preview from "./pages/Preview";
import Sent from "./pages/Sent";

const routes = [
  {
    path: "/emails",
    component: {
      render: (c) => c("router-view"),
    },
    children: [
      { path: "", component: Index },
      {
        name: "email-create",
        path: "create/:emailTypeId",
        component: CreateEdit,
        props: true,
      },
      {
        name: "email-edit",
        path: "edit/:emailId",
        component: CreateEdit,
        props: true,
      },
      {
        name: "email-create-layout",
        path: "create-layout",
        component: CreateEditLayout,
        props: true,
      },
      {
        name: "email-edit-layout",
        path: "edit-layout/:emailId",
        component: CreateEditLayout,
        props: true,
      },
      { name: "email-send", path: "/send/:id", component: Send, props: true },
      {
        name: "email-preview",
        path: "preview/:id",
        component: Preview,
        props: true,
      },
      { name: "email-sent", path: "sent/:id", component: Sent, props: true },
    ],
  },
];

export default routes;
