import { useRoutes } from "react-router-dom";
import CatFactPage from "./pages/CatFactPage";
import Error404Page from "./pages/Error404Page";
import DashboardPage from "./pages/DashboardPage";

const Router = () => {
  return useRoutes([
    {
      path: `/`,
      element: <CatFactPage />,
      errorElement: <Error404Page />,
    },
    {
      path: `/dashboard`,
      element: <DashboardPage />,
      errorElement: <Error404Page />,
    },
    {
      path: `/subscribe`,
      element: <div>Subscribe</div>,
      errorElement: <Error404Page />,
    },
    { path: "*", element: <Error404Page /> },
  ]);
};

export default Router;
