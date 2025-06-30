import { Grid2, Typography } from "@mui/material";
import ProfileHeader from "./ProfileHeader";
import ProfileContent from "./ProfileContent";
import { useParams } from "react-router";
import { useProfile } from "../../lib/hooks/useProfile";

export default function ProfilePage() {

  // This will get the [id] from the [URL]
  const {id} = useParams();
  const {profile, loadingProfile} = useProfile(id);

  if (loadingProfile) return <Typography>Loading Profile...</Typography>

  if (!profile) return <Typography>Profile Not Found</Typography>

  return (
    <Grid2 container>
        <Grid2 size={12}>
            <ProfileHeader profile={profile} />
            <ProfileContent />
        </Grid2>
    </Grid2>
  )
}