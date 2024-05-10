import { FunctionComponent } from "react";
import Dashboard from "../../components/layout/Dashboard";
import { useMailSubscribers } from "../../api/emailManagement";
import { MailSubscriberDetail } from "../../model/mailSubscriber";

const EmailManagementPage: FunctionComponent = () => {
  const mailSubscribersQuery = useMailSubscribers();

  if (mailSubscribersQuery.isPending) return <p>Loading...</p>;

  if (mailSubscribersQuery.isError) return <p>Error fetching data</p>;

  return (
    <Dashboard>
      {mailSubscribersQuery.data.map((mailSubscriber: MailSubscriberDetail) => (
        <p>{mailSubscriber.email}</p>
      ))}
    </Dashboard>
  );
};

export default EmailManagementPage;
