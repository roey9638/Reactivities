import { z } from 'zod';
import { requiredString } from '../util/util';

// In here I'm making sure that the [properties] for an [Activity] for [Example] are [required] And VVV
// Making Sure that what the [user] has writing in the [Input Fields] in the [Forms] are [correct].
export const activitySchema = z.object({
    title: requiredString('Title'),
    description: requiredString('Description'),
    category: requiredString('Category'),
    date: z.coerce.date({
        message: 'Date is required'
    }),
    location: z.object({
        venue: requiredString('Venue'),
        city: z.string().optional(),
        latitude: z.coerce.number(),
        longitude: z.coerce.number()
    })

})

export type ActivitySchema = z.infer<typeof activitySchema>;