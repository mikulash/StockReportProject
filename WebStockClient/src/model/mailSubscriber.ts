import { z } from "zod";
import { MailSubscriberSchema } from "../schema/mailSubscriberSchema";

export type MailSubscriber = z.infer<typeof MailSubscriberSchema>;

export type MailSubscriberDetail = MailSubscriber & {
  id: number;
};
