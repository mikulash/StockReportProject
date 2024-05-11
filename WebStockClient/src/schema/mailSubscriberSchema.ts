import { z } from "zod";

export const MailSubscriberSchema = z.object({
  email: z.string().email().max(200),
});
