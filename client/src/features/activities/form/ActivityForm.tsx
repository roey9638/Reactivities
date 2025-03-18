import { Box, Button, Paper, Typography } from "@mui/material";
import { useActivities } from "../../../lib/hooks/useActivities";
import { useNavigate, useParams } from "react-router";
import { useForm } from 'react-hook-form';
import { useEffect } from "react";
import { activitySchema, ActivitySchema } from "../../../lib/schemas/activitySchema";
import { zodResolver } from '@hookform/resolvers/zod';
import TextInput from "../../../app/shared/components/TextInput";
import SelectInput from "../../../app/shared/components/SelectInput";
import { categoryOptions } from "./categoryOptions";
import DateTimeInput from "../../../app/shared/components/DateTimeInput";
import LocationInput from "../../../app/shared/components/LocationInput";


export default function ActivityForm() {

    // The [ActivitySchema] in the [useForm<ActivitySchema>] is to [Validate] the [Fields] From the [Input Fields].
    const { control, reset, handleSubmit } = useForm<ActivitySchema>({
        mode: 'onTouched',
        resolver: zodResolver(activitySchema)
    });

    const navigate = useNavigate();

    const { id } = useParams();

    const { updateActivity, createActivity, activity, isLoadingActivity } = useActivities(id);

    useEffect(() => {
        // What the [reset] does is if i have an [activity] VVV
        // Than it will [register] all the [Input Fields] with that [activity].
        if (activity) reset({
            ...activity,
            location: { /* Ive took out the [location] from the [activity] 
                        And [changed] it into an [Object]. Because in the [ActivitySchema] we made it into an [Object]  */
                city: activity.city,
                venue: activity.venue,
                latitude: activity.latitude,
                longitude: activity.longitude
            }
        });
    }, [activity, reset]);


    const onSubmit = async (data: ActivitySchema) => {
        { /* Ive took out the [location] from the [data] 
            And [changed] it into a [Normal Properties]. 
            Because in the [CreateActivityDto] in the [API] is just a [Class] with [Properties]  */}
        const { location, ...rest } = data;
        const flattenedData = {...rest, ...location} as Activity;
        
        try {
            if (activity) {
                updateActivity.mutate({ ...activity, ...flattenedData }, {
                    onSuccess: () => navigate(`/activities/${activity.id}`)
                })
            } else {
                createActivity.mutate(flattenedData, {
                    onSuccess: (id) => navigate(`/activities/${id}`)
                })
            }
        } catch (error) {
            console.log(error);
        }
    }

    if (isLoadingActivity) return <Typography>Loading Activity...</Typography>

    return (
        <Paper sx={{ borderRadius: 3, padding: 3 }}>
            <Typography variant="h5" gutterBottom color="primary">
                {activity ? 'Edit Activity' : 'Create Activity'}
            </Typography>
            <Box component='form' onSubmit={handleSubmit(onSubmit)} display='flex' flexDirection='column' gap={3}>
                {/* The [name] must be [Exactly] how it was writing in the [ActivitySchema] */}
                
                <TextInput label='Title' control={control} name='title' />
                <TextInput label='Description' control={control} name='description' multiline rows={3} />

                <Box display='flex' gap={3}>
                    <SelectInput items={categoryOptions} label='Category' control={control} name='category' />
                    <DateTimeInput label='Date' control={control} name='date' />
                </Box>

                <LocationInput control={control} label='Enter the location' name="location" />

                <Box display='flex' justifyContent='end' gap={4}>
                    <Button color="inherit">Cancel</Button>
                    <Button
                        type="submit"
                        color="success"
                        variant="contained"
                        disabled={updateActivity.isPending || createActivity.isPending}
                    >
                        Submit
                    </Button>
                </Box>
            </Box>
        </Paper>
    )
}