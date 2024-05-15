import { FunctionComponent, useEffect } from "react";
import { useFieldArray, useFormContext } from "react-hook-form";
import FundInput from "./FundInput";

interface FundsInputProps {
  name: string;
}

const FundsInput: FunctionComponent<FundsInputProps> = ({
  name,
}: FundsInputProps) => {
  const { control } = useFormContext();

  const { fields, append, remove } = useFieldArray({
    control,
    name: "preferences",
  });

  useEffect(() => {
    if (fields.length === 0) append({});
  }, []);

  const handleAddFund = () => {
    append({});
  };

  return (
    <div className="my-12">
      <h4 className="text-2xl font-bold dark:text-white mb-8 text-gray-600">
        Select funds you want to follow and their format
      </h4>
      {fields.map((field, index) => (
        <div
          key={field.id}
          className={`items-center mx-auto ${
            index === 0 ? "mb-12" : "mb-3"
          } space-y-4 max-w-screen-sm px-8 `}
        >
          <FundInput name={`${name}.${index}`} />
          {index > 0 && (
            <div className="flex justify-end">
              <button
                type="button"
                className="-mr-4 -mt-6 text-gray-900 bg-white border border-gray-300 outline-none hover:bg-gray-100 font-medium rounded-full text-sm px-3 py-2.5 me-2 mb-2 dark:bg-gray-800 dark:text-white dark:border-gray-600 dark:hover:bg-gray-700 dark:hover:border-gray-600 "
                onClick={() => remove(index)}
              >
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
          )}
        </div>
      ))}
      <button
        type="button"
        className="mt-6 text-gray-900 bg-white border border-gray-300 outline-none hover:bg-gray-100 font-medium rounded-full text-sm px-3 py-2.5 me-2 mb-2 dark:bg-gray-800 dark:text-white dark:border-gray-600 dark:hover:bg-gray-700 dark:hover:border-gray-600 "
        onClick={handleAddFund}
      >
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
            d="M5 12h14m-7 7V5"
          />
        </svg>
      </button>
    </div>
  );
};

export default FundsInput;
