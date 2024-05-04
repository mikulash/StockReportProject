import { useCatFact } from "../api/catFacts";

const CatFactPage = () => {
  const query = useCatFact();

  if (query.isPending) return <p>Loading...</p>;

  if (query.isError) return <p>Error fetching data</p>;

  return (
    <>
      <h1 className="text-3xl font-bold text-red-400">Cat Fact!</h1>
      <p>{query.data.fact}</p>
    </>
  );
};

export default CatFactPage;
