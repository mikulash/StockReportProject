import { FunctionComponent } from "react";
import { useForm, SubmitHandler, FormProvider } from "react-hook-form";
import { MailSubscriber } from "../model/mailSubscriber";
import { useMailSubscriberCreate } from "../api/emailManagement";
import { zodResolver } from "@hookform/resolvers/zod";
import { MailSubscriberSchema } from "../schema/mailSubscriberSchema";
import Toast from "../components/feedback/Toast";
import EmailInput from "../components/form/EmailInput";
import FundsInput from "../components/form/FundsInput";

const SubscribtionPage: FunctionComponent = () => {
  const methods = useForm<MailSubscriber>({
    resolver: zodResolver(MailSubscriberSchema),
  });

  const queryPost = useMailSubscriberCreate();

  const onSubmit: SubmitHandler<MailSubscriber> = (data) => {
    console.log(data);
    queryPost.mutate(data);
    if (queryPost.isSuccess) {
      methods.resetField("email");
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
          <FormProvider {...methods}>
            <form onSubmit={methods.handleSubmit(onSubmit)}>
              <EmailInput name="email" />
              <div className="mx-auto max-w-screen-sm text-sm text-left text-gray-500 newsletter-form-footer dark:text-gray-300">
                We care about the protection of your data.
              </div>

              <FundsInput name={`preferences`} />
            </form>
          </FormProvider>

          {queryPost.isError && (
            <Toast
              onClose={() => queryPost.reset()}
              type={"danger"}
              text={"Unable to subscribe."}
            />
          )}
          {queryPost.isSuccess && (
            <Toast
              onClose={() => queryPost.reset()}
              type={"success"}
              text={"Subscribed!"}
            />
          )}
        </div>
      </div>
    </section>
  );
};

export default SubscribtionPage;
