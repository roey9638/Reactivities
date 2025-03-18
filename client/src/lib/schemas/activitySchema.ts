import { z } from 'zod';

export const requiredString = (fieldName: string) =>
    z.string({ required_error: `${fieldName} is required` })
        .min(1, { message: `${fieldName} is required` })


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