import { Grid2, Typography } from "@mui/material";
import ActivityList from "./ActivityList";
import { useActivities } from "../../../lib/hooks/useActivities";
import ActivityFilters from "./ActivityFilters";



export default function ActivityDasboard() {

    // The [isPending] needs to be [Changed] to [isLoading]!!!
    const { activities, isLoading } = useActivities();

    if (!activities || isLoading) return <Typography>Loading...</Typography>

    // const { activities, isLoading } = useActivities();

    // if (isLoading) return <Typography>Loading...</Typography>

    // if (!activities) return <Typography>No activities found</Typography>

    return (
        <Grid2 container spacing={3}>
            <Grid2 size={8}>
                <ActivityList />
            </Grid2>
            <Grid2 size={4}>
                <ActivityFilters />
            </Grid2>
        </Grid2>
    )
}