import React from 'react'
import { Activity } from '../../../app/models/activity';
import { Button, Item, Label, Segment } from 'semantic-ui-react';

interface Props {
    activities: Activity[];
    selecteActivity: (id: string) => void;
    deleteActivity: (id: string) => void;
}


export default function ActivityList({activities, selecteActivity, deleteActivity}: Props) {
    return (
        /* This is [Where] i [represent/Display] my [activities/list]!!! */
        <Segment>
            <Item.Group divided>
                {activities.map(activity => (
                    <Item key={activity.id}>
                        <Item.Content>
                            <Item.Header as='a'>{activity.title}</Item.Header>
                            <Item.Meta>{activity.date}</Item.Meta>
                            <Item.Description>
                                <div>{activity.description}</div>
                                <div>{activity.city}, {activity.venue}</div>
                            </Item.Description>
                            <Item.Extra>
                                <Button onClick={() => selecteActivity(activity.id)} floated='right' content='View' color='blue'/>
                                <Button onClick={() => deleteActivity(activity.id)} floated='right' content='Delete' color='red'/>
                                <Label basic content={activity.category} />
                            </Item.Extra>
                        </Item.Content>
                    </Item>
                ))}
            </Item.Group>
        </Segment>
    )
}