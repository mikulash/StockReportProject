import { useRoutes } from "react-router-dom";
import Error404Page from "./pages/Error404Page";
import SubscribtionPage from "./pages/SubscriptionPage";
import UnsubscribePage from "./pages/UnsubscribePage";

const Router = () => {
  return useRoutes([
    {
      path: `/`,
      element: <SubscribtionPage />,
      errorElement: <Error404Page />,
    },
    {
      path: `/unsubscribe/:id`,
      element: <UnsubscribePage />,
      errorElement: <Error404Page />,
    },
    { path: "*", element: <Error404Page /> },
  ]);
};

export default Router;
