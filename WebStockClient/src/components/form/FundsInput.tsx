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

  const { fields, append } = useFieldArray({
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
    <>
      {fields.map((field, index) => (
        <FundInput key={field.id} name={`${name}.${index}`} />
      ))}
      <button type="button" onClick={handleAddFund}>
        Add
      </button>
    </>
  );
};

export default FundsInput;
