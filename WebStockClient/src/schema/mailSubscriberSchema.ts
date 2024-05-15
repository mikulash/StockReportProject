import { z } from "zod";

const MailSubscriberPreferenceSchema = z.object({
  fundName: z.string().max(200),
  outputType: z.enum(["Html", "String"]),
});

export const MailSubscriberSchema = z.object({
  email: z.string().email().max(200),
  preferences: z.array(MailSubscriberPreferenceSchema).min(1),
});
