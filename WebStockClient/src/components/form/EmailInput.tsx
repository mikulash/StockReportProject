import { FunctionComponent } from "react";
import { useFormContext } from "react-hook-form";

interface EmailInputProps {
  name: string;
}

const EmailInput: FunctionComponent<EmailInputProps> = ({
  name,
}: EmailInputProps) => {
  const {
    register,
    formState: { errors },
  } = useFormContext();

  return (
    <>
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
            {...register(name)}
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
            <span className="font-medium">
              {errors.email.message?.toString()}
            </span>
          </div>
        </div>
      )}
    </>
  );
};

export default EmailInput;
