import { useRoutes } from "react-router-dom";
import CatFactPage from "./pages/CatFactPage";
import Error404Page from "./pages/Error404Page";
import EmailManagementPage from "./pages/Dashboard/EmailManagementPage";
import SubscribtionPage from "./pages/SubscriptionPage";

const Router = () => {
  return useRoutes([
    {
      path: `/`,
      element: <CatFactPage />,
      errorElement: <Error404Page />,
    },
    {
      path: `/dashboard`,
      element: <EmailManagementPage />,
      errorElement: <Error404Page />,
    },
    {
      path: `/subscribe`,
      element: <SubscribtionPage />,
      errorElement: <Error404Page />,
    },
    { path: "*", element: <Error404Page /> },
  ]);
};

export default Router;
