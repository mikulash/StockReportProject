import { FunctionComponent } from "react";
import { useForm, SubmitHandler } from "react-hook-form";
import { MailSubscriber } from "../model/mailSubscriber";
import { useMailSubscriberCreate } from "../api/emailManagement";
import { zodResolver } from "@hookform/resolvers/zod";
import { MailSubscriberSchema } from "../schema/mailSubscriberSchema";
import Toast from "../components/feedback/Toast";
import FundInput from "../components/form/FundInput";

const SubscribtionPage: FunctionComponent = () => {
  const {
    register,
    handleSubmit,
    formState: { errors },
    resetField,
  } = useForm<MailSubscriber>({ resolver: zodResolver(MailSubscriberSchema) });
  const queryPost = useMailSubscriberCreate();

  const onSubmit: SubmitHandler<MailSubscriber> = (data) => {
    queryPost.mutate(data);
    if (queryPost.isSuccess) {
      resetField("email");
    }
  };

  return (
    <section className="bg-white dark:bg-gray-900 h-screen pt-12">
      <div className="py-8 px-4 mx-auto max-w-screen-xl lg:py-16 lg:px-6">
        <div className="mx-auto max-w-screen-md sm:text-center">
          <h2 className="mb-4 text-3xl tracking-tight font-extrabold text-gray-900 sm:text-4xl dark:text-white">
            Sign up for our stock newsletter
          </h2>
          <p className="mx-auto mb-8 max-w-2xl font-light text-gray-500 md:mb-12 sm:text-xl dark:text-gray-400">
            Stay ahead of market trends and investment opportunities. Sign up
            for our stock newsletter and receive expert insights, analysis, and
            exclusive updates straight to your inbox!
          </p>
          <form onSubmit={handleSubmit(onSubmit)}>
            <div className="items-center mx-auto mb-3 space-y-4 max-w-screen-sm sm:flex sm:space-y-0">
              <div className="relative w-full">
                <label
                  htmlFor="email"
                  className="hidden mb-2 text-sm font-medium text-gray-900 dark:text-gray-300"
                >
                  Email address
                </label>
                <div className="flex absolute inset-y-0 left-0 items-center pl-3 pointer-events-none">
                  <svg
                    className="w-5 h-5 text-gray-500 dark:text-gray-400"
                    fill="currentColor"
                    viewBox="0 0 20 20"
                    xmlns="http://www.w3.org/2000/svg"
                  >
                    <path d="M2.003 5.884L10 9.882l7.997-3.998A2 2 0 0016 4H4a2 2 0 00-1.997 1.884z" />
                    <path d="M18 8.118l-8 4-8-4V14a2 2 0 002 2h12a2 2 0 002-2V8.118z" />
                  </svg>
                </div>
                <input
                  className={`block p-3 pl-10 w-full text-sm ${
                    errors.email
                      ? "bg-red-50 border border-red-500 text-red-900 placeholder-red-700"
                      : "bg-gray-50 text-gray-900 border border-gray-300"
                  }   rounded-lg sm:rounded-none sm:rounded-l-lg dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white outline-none`}
                  placeholder="Enter your email"
                  {...register("email")}
                />
              </div>
              <div>
                <button
                  type="submit"
                  className="py-3 px-5 w-full text-sm font-medium text-center text-white rounded-lg border cursor-pointer bg-blue-700 border-blue-600 sm:rounded-none sm:rounded-r-lg hover:bg-primary-800 dark:bg-primary-600 dark:hover:bg-primary-700"
                >
                  Subscribe
                </button>
              </div>
            </div>

            {errors.email && (
              <div
                className="flex mx-auto max-w-screen-sm items-center p-4 mb-4 text-sm text-red-800 rounded-lg bg-red-50 dark:bg-gray-800 dark:text-red-400"
                role="alert"
              >
                <svg
                  className="flex-shrink-0 inline w-4 h-4 me-3"
                  aria-hidden="true"
                  xmlns="http://www.w3.org/2000/svg"
                  fill="currentColor"
                  viewBox="0 0 20 20"
                >
                  <path d="M10 .5a9.5 9.5 0 1 0 9.5 9.5A9.51 9.51 0 0 0 10 .5ZM9.5 4a1.5 1.5 0 1 1 0 3 1.5 1.5 0 0 1 0-3ZM12 15H8a1 1 0 0 1 0-2h1v-3H8a1 1 0 0 1 0-2h2a1 1 0 0 1 1 1v4h1a1 1 0 0 1 0 2Z" />
                </svg>
                <span className="sr-only">Info</span>
                <div>
                  <span className="font-medium">{errors.email?.message}</span>
                </div>
              </div>
            )}
            <div className="mx-auto max-w-screen-sm text-sm text-left text-gray-500 newsletter-form-footer dark:text-gray-300">
              We care about the protection of your data.
            </div>

            <FundInput />
          </form>

          {queryPost.isError && (
            <div className="py-12 flex justify-center">
              <Toast
                onClose={() => queryPost.reset()}
                type={"danger"}
                text={"Unable to subscribe."}
              />
            </div>
          )}
          {queryPost.isSuccess && (
            <div className="py-12 flex justify-center">
              <Toast
                onClose={() => queryPost.reset()}
                type={"success"}
                text={"Subscribed!"}
              />
            </div>
          )}
        </div>
      </div>
    </section>
  );
};

export default SubscribtionPage;
