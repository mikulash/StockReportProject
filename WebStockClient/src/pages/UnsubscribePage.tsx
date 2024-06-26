import { FunctionComponent, useEffect, useState } from "react";
import { useMailSubscriberDelete } from "../api/emailManagement";
import { useParams } from "react-router-dom";
import Error404Page from "./Error404Page";

const UnsubscribePage: FunctionComponent = () => {
  const [isInvalidId, setIsInvalidId] = useState(false);
  const queryUnsubscribe = useMailSubscriberDelete();

  const { id } = useParams();

  const subscriberId = id ?? undefined;

  useEffect(() => {
    setIsInvalidId(false);
    if (!subscriberId) {
      setIsInvalidId(true);
      return;
    }
    queryUnsubscribe.mutate(subscriberId);
  }, [subscriberId]);

  if (isInvalidId) return <Error404Page />;

  return (
    <section className="bg-white dark:bg-gray-900 h-full min-h-screen pt-24">
      <div className="py-8 px-4 mx-auto max-w-screen-xl lg:py-16 lg:px-6">
        <div className="mx-auto max-w-screen-md sm:text-center">
          {queryUnsubscribe.isError ? (
            <>
              <h2 className="mb-4 text-3xl tracking-tight font-extrabold text-gray-900 sm:text-4xl dark:text-white">
                Unable to unsubscribe
              </h2>
              <p className="mx-auto mt-8 mb-8 max-w-2xl font-light text-gray-500 md:mt-12 sm:text-xl dark:text-gray-400">
                Try again later.
              </p>
              <a
                href="/"
                className="inline-flex text-white bg-blue-600 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center dark:focus:ring-blue-900 my-4"
              >
                Back to Homepage
              </a>
            </>
          ) : queryUnsubscribe.isPending ? (
            <p className="mx-auto mt-8 text-gray-500">Pending...</p>
          ) : (
            <>
              <h2 className="mb-4 text-3xl tracking-tight font-extrabold text-gray-900 sm:text-4xl dark:text-white">
                Unsubscribe Confirmation
              </h2>
              <p className="mx-auto mt-8 max-w-2xl font-light text-gray-500 md:mt-12 sm:text-xl dark:text-gray-400">
                You have been successfully unsubscribed from our newsletter.
              </p>
              <p className="mx-auto my-4 max-w-2xl font-light text-gray-500  sm:text-xl dark:text-gray-400">
                We're sorry to see you go, but we respect your decision. If you
                ever change your mind, you can resubscribe at any time.
              </p>
              <p className="mx-auto mb-8 max-w-2xl font-light text-gray-500 md:mb-12 sm:text-xl dark:text-gray-400">
                Thank you for your past interest in our newsletter.
              </p>
              <a
                href="/"
                className="inline-flex text-white bg-blue-600 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center dark:focus:ring-blue-900 my-4"
              >
                Back to Homepage
              </a>
            </>
          )}
        </div>
      </div>
    </section>
  );
};

export default UnsubscribePage;
