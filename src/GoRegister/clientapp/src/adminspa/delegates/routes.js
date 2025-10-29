// import the project pages
import BulkUpload from "./pages/BulkUpload";

const routes = [
  {
    path: "/delegates",
    component: {
      render: (c) => c("router-view"),
    },
    children: [
      {
        name: "delegates-bulkupload",
        path: "bulkupload",
        component: BulkUpload,
        props: true,
      },
    ],
  },
];

export default routes;
