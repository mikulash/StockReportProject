import { FunctionComponent } from "react";
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

  const handleAddFund = () => {
    append({});
  };
  return (
    <>
      {fields.map((field, index) => (
        <FundInput key={field.id} name={`${name}.${index}`} />
      ))}
      <button onClick={handleAddFund}>Add</button>
    </>
  );
};

export default FundsInput;
