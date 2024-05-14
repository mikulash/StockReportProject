import { FunctionComponent } from "react";

interface FundInputProps {}

const FundInput: FunctionComponent<FundInputProps> = () => {
  return (
    <div className="items-center mx-auto mb-3 space-y-4 max-w-screen-sm ">
      <label
        htmlFor="countries"
        className="hidden block mb-2 text-sm font-medium text-gray-900 dark:text-white"
      >
        Select a fund
      </label>
      <select
        id="countries"
        className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white"
      >
        <option defaultValue="">Select a fund</option>
        <option value="f1">fund 1</option>
        <option value="f2">fund 2</option>
      </select>
      <ul className="items-center w-full mt-4 text-sm font-medium text-gray-900 bg-white border border-gray-200 rounded-lg sm:flex dark:bg-gray-700 dark:border-gray-600 dark:text-white">
        <li className="w-full border-b border-gray-200 sm:border-b-0 sm:border-r dark:border-gray-600">
          <div className="flex items-center ps-3">
            <input
              id="horizontal-list-radio-license"
              type="radio"
              name="list-radio"
              defaultValue="html"
              defaultChecked
              className="w-4 h-4 text-blue-600 bg-gray-100 border-gray-300 dark:ring-offset-gray-700 dark:bg-gray-600 dark:border-gray-500"
            />
            <label
              htmlFor="horizontal-list-radio-license"
              className="w-full py-3 ms-2 text-sm font-medium text-gray-900 dark:text-gray-300"
            >
              HTML
            </label>
          </div>
        </li>
        <li className="w-full border-b border-gray-200 sm:border-b-0 sm:border-r dark:border-gray-600">
          <div className="flex items-center ps-3">
            <input
              id="horizontal-list-radio-id"
              type="radio"
              name="list-radio"
              defaultValue="string"
              className="w-4 h-4 text-blue-600 bg-gray-100 border-gray-300  dark:ring-offset-gray-700 dark:bg-gray-600 dark:border-gray-500"
            />
            <label
              htmlFor="horizontal-list-radio-id"
              className="w-full py-3 ms-2 text-sm font-medium text-gray-900 dark:text-gray-300"
            >
              Plain text
            </label>
          </div>
        </li>
      </ul>
    </div>
  );
};

export default FundInput;
