import { useMutation, useQuery } from "@tanstack/react-query";
import api from ".";
import { MailSubscriber, MailSubscriberDetail } from "../model/mailSubscriber";
import { queryClient } from "../main";

export const useMailSubscribers = () =>
  useQuery({
    queryKey: ["get", "subscribers"],
    queryFn: async () =>
      (await api()
        .get("/mail-web-api/mailsubscriber")
        .json()) as MailSubscriberDetail[],
  });

export const useMailSubscriberCreate = () =>
  useMutation({
    mutationFn: async (data: MailSubscriber) =>
      (await api()
        .post("/mail-web-api/mailsubscriber", {
          json: data,
        })
        .json()) as MailSubscriberDetail,
    onSuccess: () => {
      queryClient.invalidateQueries();
    },
  });

export const useMailSubscriberUpdate = () =>
  useMutation({
    mutationFn: async (data: MailSubscriberDetail) =>
      (await api()
        .put(`/mail-web-api/mailsubscriber/${data.id}`, {
          json: { email: data.email },
        })
        .json()) as MailSubscriberDetail,
    onSuccess: () => {
      queryClient.invalidateQueries();
    },
  });

export const useMailSubscriberDelete = () =>
  useMutation({
    mutationFn: async (id: number) =>
      await api().delete(`/mail-web-api/mailsubscriber/${id}`).json(),
    onSuccess: () => {
      queryClient.invalidateQueries();
    },
  });

export const useUnsubscribe = () =>
  useMutation({
    mutationFn: (id: number) =>
      api().delete(`/mail-web-api/mailsubscriber/${id}`).json(),
  });
