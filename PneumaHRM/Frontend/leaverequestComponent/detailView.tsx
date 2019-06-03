import React from 'react';

import Button from '@material-ui/core/Button';
import Grid from '@material-ui/core/Grid';
import List from '@material-ui/core/List';
import ListItem from '@material-ui/core/ListItem';
import ListItemText from '@material-ui/core/ListItemText';
import Typography from '@material-ui/core/Typography';
import TextField from '@material-ui/core/TextField';
import Paper from '@material-ui/core/Paper';


import { Query, Mutation } from 'react-apollo';

import APPROVE_LEAVE_REQUEST from './approveLeaveRequest';
import DEPUTY_LEAVE_REQUEST from './deputyLeaveRequest';
import GET_LEAVE_REQUEST_DETAIL_QUERY from './getLeaveRequestDetail';

export default (id) => <Query query={GET_LEAVE_REQUEST_DETAIL_QUERY} variables={{ id: id }}>
    {({ data, loading, client }) => loading ? <>Loading</> :
        data.leaveRequests.length > 0 ?
            <main>
                <Paper style={{ padding: "10px" }}>
                    <Typography component="h1" variant="h4" align="center">Leave Request Detail</Typography>
                    <Form data={data.leaveRequests[0]} />
                    <Typography variant="h6" gutterBottom>History / Comment</Typography>
                    <Grid container spacing={24}>
                        <Grid item xs={12}>
                            <List>
                                {data.leaveRequests[0].comments.map(toListItem)}
                            </List>
                            <TextField
                                label="Comment"
                                fullWidth
                                value={data.currentComment}
                                onChange={e => client.writeData({ data: { currentComment: e.target.value } })}
                            />
                            <Mutation
                                mutation={APPROVE_LEAVE_REQUEST}
                                refetchQueries={["GetPagedLeaveRequests", "GetLeaveRequestDetail"]}
                                variables={{ input: { requestId: id, content: data.currentComment } }}
                                onCompleted={e => client.writeData({ data: { currentComment: "" } })}
                                children={ApproveAction()} />
                            <Mutation
                                mutation={DEPUTY_LEAVE_REQUEST}
                                refetchQueries={["GetPagedLeaveRequests", "GetLeaveRequestDetail"]}
                                variables={{ input: { requestId: id, content: data.currentComment } }}
                                onCompleted={e => client.writeData({ data: { currentComment: "" } })}
                                children={DeputyAction(data.leaveRequests[0].canDeputyBy)}
                            />
                            <CompleteAction />
                        </Grid>
                    </Grid>
                </Paper>
            </main> : <></>}
</Query>

const DeputyAction = (canDeputyBy) => (deputyLeaveRequest) => <Button
    variant="contained"
    size="small"
    disabled={!canDeputyBy}
    style={{ marginLeft: "1px" }}
    color="primary"
    onClick={() => deputyLeaveRequest()}>Deputy</Button>

const ApproveAction = () => (approve) => <Button
    variant="contained"
    size="small"
    style={{ marginLeft: "1px" }}
    color="primary"
    onClick={() => approve()}>Approve</Button>

const CompleteAction = () => <Button
    variant="contained"
    size="small"
    style={{ marginLeft: "1px" }}
    color="primary">Complete</Button>
const Form = (props) => <>
    <Typography variant="h6" gutterBottom>
        {props.data.owner}'s  Leave Request {props.data.id}
    </Typography>
    <Grid container spacing={24}>
        <Grid item xs={12} sm={6}>
            <TextField
                label="Start"
                fullWidth
                value={props.data.from}
            />
        </Grid>
        <Grid item xs={12} sm={6}>
            <TextField
                label="End"
                value={props.data.to}
                fullWidth
            />
        </Grid>
        <Grid item xs={12} sm={6}>
            <TextField
                label="type"
                fullWidth
                value={props.data.type}
            />
        </Grid>
        <Grid item xs={12} sm={6}>
            <TextField
                label="Calculated hours"
                fullWidth
                value={props.data.workHour}
            />
        </Grid>
        <Grid item xs={12}>
            <TextField
                label="Description"
                fullWidth
                value={props.data.description}
            />
        </Grid>
    </Grid>
</>

const toListItem = (row, index) => <ListItem alignItems="flex-start">
    <ListItemText
        key={index}
        primary={row.type}
        secondary={
            <>
                <Typography component="span" color="textPrimary" >
                    {row.createdBy}
                </Typography>
                {row.content ? row.content : "No Comment input"}
            </>
        }
    />
</ListItem>


