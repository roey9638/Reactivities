import React from 'react'
import { Grid } from 'semantic-ui-react'
import { Activity } from '../../../app/models/activity'
import ActivityList from './ActivityList';
import ActivityDetails from '../details/ActivityDetails';
import ActivityForm from '../form/ActivityForm';

interface Props {
    activities: Activity[];
    selectedActivity: Activity | undefined;
    selecteActivity: (id: string) => void;
    cancelSelectActivity: () => void;
    editMode: boolean;
    openForm: (id: string) => void;
    closeForm: () => void;
    createOrEdit: (activity: Activity) => void;
    deleteActivity: (id: string) => void;
}

export default function ActivityDashBoard({activities, selectedActivity, 
        selecteActivity, cancelSelectActivity, editMode, openForm, closeForm, createOrEdit, deleteActivity}: Props) {
    return (
        <Grid>
            <Grid.Column width='10'>
                <ActivityList 
                    activities={activities} 
                    selecteActivity={selecteActivity}
                    deleteActivity={deleteActivity}
                />
            </Grid.Column>
            
            <Grid.Column width='6'>
                {selectedActivity && !editMode && /* Here I'm making sure that the [ActivityDetails] will be [Loaded] [Only] if [i have] a [selectedActivity] */
                                                  /* The [selectedActivity] will be [set] in the [<ActivityList />] [Form] */
                    <ActivityDetails 
                        activity={selectedActivity} 
                        cancelSelectActivity={cancelSelectActivity} 
                        openForm={openForm}
                    />
                }
            {editMode && /* I will [Display] the [ActivityForm] only if the [editMode = true] */
                         /* The [selectedActivity] will be [set] in the [<ActivityList />] [Form] */
                <ActivityForm 
                    closeForm={closeForm} 
                    activity={selectedActivity} 
                    createOrEdit={createOrEdit} 
                />
            }
            </Grid.Column>
        </Grid>
    )
}