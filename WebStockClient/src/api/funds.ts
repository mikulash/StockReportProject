import { useQuery } from "@tanstack/react-query";
import api from ".";
import { FundDetail } from "../model/fund";

export const useFunds = () =>
  useQuery({
    queryKey: ["get", "funds"],
    queryFn: async () =>
      (await api().get("/stock-web-api/fund").json()) as FundDetail[],
  });
