import { FunctionComponent } from "react";
import { useFormContext } from "react-hook-form";
import { useFunds } from "../../api/funds";
import { FundDetail } from "../../model/fund";

interface FundInputProps {
  name: string;
}

const FundInput: FunctionComponent<FundInputProps> = ({
  name,
}: FundInputProps) => {
  const { register } = useFormContext();

  const queryGetFunds = useFunds();

  return (
    <div>
      <label
        htmlFor="countries"
        className="hidden block mb-2 text-sm font-medium text-gray-900 dark:text-white"
      >
        Select a fund
      </label>
      <select
        id="countries"
        className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white"
        {...register(`${name}.fundName`)}
      >
        <option defaultValue="">Select a fund</option>
        {queryGetFunds.data &&
          queryGetFunds.data.map((fund: FundDetail) => (
            <option key={fund.id} value={fund.fundName}>
              {fund.fundName}
            </option>
          ))}
      </select>
      <ul className="items-center w-full mt-4 text-sm font-medium text-gray-900 bg-white border border-gray-200 rounded-lg sm:flex dark:bg-gray-700 dark:border-gray-600 dark:text-white">
        <li className="w-full border-b border-gray-200 sm:border-b-0 sm:border-r dark:border-gray-600">
          <div className="flex items-center ps-3">
            <input
              id={`horizontal-list-radio-${name}-html`}
              type="radio"
              defaultValue="html"
              defaultChecked
              className="w-4 h-4 text-blue-600 bg-gray-100 border-gray-300 dark:ring-offset-gray-700 dark:bg-gray-600 dark:border-gray-500"
              {...register(`${name}.outputType`)}
            />
            <label
              htmlFor={`horizontal-list-radio-${name}-html`}
              className="w-full py-3 ms-2 text-sm font-medium text-gray-900 dark:text-gray-300"
            >
              HTML
            </label>
          </div>
        </li>
        <li className="w-full border-b border-gray-200 sm:border-b-0 sm:border-r dark:border-gray-600">
          <div className="flex items-center ps-3">
            <input
              id={`horizontal-list-radio-${name}-string`}
              type="radio"
              defaultValue="string"
              className="w-4 h-4 text-blue-600 bg-gray-100 border-gray-300  dark:ring-offset-gray-700 dark:bg-gray-600 dark:border-gray-500"
              {...register(`${name}.outputType`)}
            />
            <label
              htmlFor={`horizontal-list-radio-${name}-string`}
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
