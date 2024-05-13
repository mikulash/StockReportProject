import { FunctionComponent, useState } from "react";
import Dashboard from "../../components/layout/Dashboard";
import {
  useMailSubscriberDelete,
  useMailSubscribers,
} from "../../api/emailManagement";
import { MailSubscriberDetail } from "../../model/mailSubscriber";
import Modal from "../../components/Modal";

const EmailManagementPage: FunctionComponent = () => {
  const queryGet = useMailSubscribers();
  const queryDelete = useMailSubscriberDelete();

  const [openModal, setOpenModal] = useState(false);

  const handleDelete = (id: number) => {
    queryDelete.mutate(id);
  };

  if (queryGet.isPending)
    return (
      <Dashboard>
        <p>Loading...</p>
      </Dashboard>
    );

  if (queryGet.isError)
    return (
      <Dashboard>
        <p>Error fetching data</p>
      </Dashboard>
    );

  return (
    <>
      {openModal && (
        <Modal
          title={"New item"}
          onClose={() => setOpenModal(false)}
          actions={
            <>
              <button
                data-modal-hide="static-modal"
                type="button"
                className="text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
              >
                Add
              </button>
              <button
                data-modal-hide="static-modal"
                type="button"
                className="py-2.5 px-5 ms-3 text-sm font-medium text-gray-900 focus:outline-none bg-white rounded-lg border border-gray-200 hover:bg-gray-100 hover:text-blue-700 focus:z-10 focus:ring-4 focus:ring-gray-100 dark:focus:ring-gray-700 dark:bg-gray-800 dark:text-gray-400 dark:border-gray-600 dark:hover:text-white dark:hover:bg-gray-700"
                onClick={() => setOpenModal(false)}
              >
                Cancel
              </button>
            </>
          }
        />
      )}

      <Dashboard>
        <h1 className="mb-4 text-2xl font-extrabold leading-none tracking-tight text-gray-900 md:text-3xl lg:text-4xl dark:text-white">
          Mail Subscribers
        </h1>
        <div className="relative overflow-x-auto">
          <table className="w-full text-sm text-left rtl:text-right text-gray-500 dark:text-gray-400">
            <thead className="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
              <tr>
                <th scope="col" className="px-6 py-3">
                  Id
                </th>
                <th scope="col" className="px-6 py-3">
                  Address
                </th>
                <th scope="col" className="px-6 py-3 text-end">
                  Actions
                </th>
              </tr>
            </thead>
            <tbody>
              {queryGet.data.map((mailSubscriber: MailSubscriberDetail) => (
                <tr
                  key={mailSubscriber.id}
                  className="bg-white border-b dark:bg-gray-800 dark:border-gray-700"
                >
                  <th
                    scope="row"
                    className="px-6 py-4 font-medium text-gray-900 whitespace-nowrap dark:text-white"
                  >
                    {mailSubscriber.id}
                  </th>
                  <td className="px-6 py-4">{mailSubscriber.email}</td>
                  <td className="px-6 py-4">
                    <div className="flex justify-end">
                      <svg
                        className="w-6 h-6 text-gray-800 dark:text-white"
                        aria-hidden="true"
                        xmlns="http://www.w3.org/2000/svg"
                        width="24"
                        height="24"
                        fill="none"
                        viewBox="0 0 24 24"
                      >
                        <path
                          stroke="currentColor"
                          strokeLinecap="round"
                          strokeLinejoin="round"
                          strokeWidth="2"
                          d="m14.304 4.844 2.852 2.852M7 7H4a1 1 0 0 0-1 1v10a1 1 0 0 0 1 1h11a1 1 0 0 0 1-1v-4.5m2.409-9.91a2.017 2.017 0 0 1 0 2.853l-6.844 6.844L8 14l.713-3.565 6.844-6.844a2.015 2.015 0 0 1 2.852 0Z"
                        />
                      </svg>
                      <button onClick={() => handleDelete(mailSubscriber.id)}>
                        <svg
                          className="w-6 h-6 text-gray-800 dark:text-white"
                          aria-hidden="true"
                          xmlns="http://www.w3.org/2000/svg"
                          width="24"
                          height="24"
                          fill="none"
                          viewBox="0 0 24 24"
                        >
                          <path
                            stroke="currentColor"
                            strokeLinecap="round"
                            strokeLinejoin="round"
                            strokeWidth="2"
                            d="M5 7h14m-9 3v8m4-8v8M10 3h4a1 1 0 0 1 1 1v3H9V4a1 1 0 0 1 1-1ZM6 7h12v13a1 1 0 0 1-1 1H7a1 1 0 0 1-1-1V7Z"
                          />
                        </svg>
                      </button>
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
        <button
          type="button"
          className="mt-5 text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 me-2 mb-2 dark:bg-blue-600 dark:hover:bg-blue-700 focus:outline-none dark:focus:ring-blue-800"
          onClick={() => setOpenModal(true)}
        >
          Add
        </button>
      </Dashboard>
    </>
  );
};

export default EmailManagementPage;
