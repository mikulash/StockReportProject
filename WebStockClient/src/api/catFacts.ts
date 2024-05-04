import { useQuery } from "@tanstack/react-query";
import api from ".";

export const useCatFact = () =>
  useQuery({
    queryKey: ["get", "fact"],
    queryFn: async () => (await api().get("/api/fact").json()) as { fact: string },
  });
