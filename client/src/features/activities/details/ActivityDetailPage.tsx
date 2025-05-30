import { Grid2, Typography } from "@mui/material";
import { useParams } from "react-router";
import { useActivities } from "../../../lib/hooks/useActivities";
import ActivityDetailsHeader from "./ActivityDetailsHeader";
import ActivityDetailsInfo from "./ActivityDetailsInfo";
import ActivityDetailsChat from "./ActivityDetailsChat";
import ActivityDetailsSidebar from "./ActivityDetailsSidebar";


export default function ActivityDetailPage() {


  // 1) To [Get] the [id] it [must] be [named] [exactly the same] as [named] it in the [Routes Component]
  // 2) In order to [Get] the [id] from the [URL] we [use] the [useParams()].
  // This [id] will come [from] When i [Press] the [(View) Button] in the [ActivityCard] Component.
  const { id } = useParams();
  const { activity, isLoadingActivity } = useActivities(id);

  if (isLoadingActivity) return <Typography>Loading...</Typography>

  if (!activity) return <Typography>Activity Not Found</Typography>

  return (
    <Grid2 container spacing={3}>
      <Grid2 size={8}>
        <ActivityDetailsHeader activity={activity} />
        <ActivityDetailsInfo activity={activity} />
        <ActivityDetailsChat />
      </Grid2>

      <Grid2 size={4}>
        <ActivityDetailsSidebar activity={activity} />
      </Grid2>

    </Grid2>
  )
}