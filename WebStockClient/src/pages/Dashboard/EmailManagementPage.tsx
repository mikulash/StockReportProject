import { FunctionComponent } from "react";
import Dashboard from "../../components/layout/Dashboard";
import { useMailSubscribers } from "../../api/emailManagement";
import { MailSubscriberDetail } from "../../model/mailSubscriber";

const EmailManagementPage: FunctionComponent = () => {
  const { data, isPending, isError } = useMailSubscribers();

  if (isPending)
    return (
      <Dashboard>
        <p>Loading...</p>
      </Dashboard>
    );

  if (isError)
    return (
      <Dashboard>
        <p>Error fetching data</p>
      </Dashboard>
    );

  return (
    <Dashboard>
      {data.map((mailSubscriber: MailSubscriberDetail) => (
        <p>{mailSubscriber.email}</p>
      ))}
    </Dashboard>
  );
};

export default EmailManagementPage;
