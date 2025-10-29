// import the project pages
import Index from "./pages/Index";

const routes = [
  {
    path: "/themes",
    component: {
      render: (c) => c("router-view"),
    },
    children: [
      { path: "", component: Index },
    ],
  },
];

export default routes;
