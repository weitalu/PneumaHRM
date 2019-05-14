import React from 'react';

import Button from '@material-ui/core/Button';
import Grid from '@material-ui/core/Grid';
import Typography from '@material-ui/core/Typography';
import TextField from '@material-ui/core/TextField';
import Paper from '@material-ui/core/Paper';
import FormControlLabel from '@material-ui/core/FormControlLabel';
import Checkbox from '@material-ui/core/Checkbox';

import { Query, Mutation } from 'react-apollo';

import DELETE_LEAVE_REQUEST from './deleteLeaveRequest';
import APPROVE_LEAVE_REQUEST from './approveLeaveRequest';
import DEPUTY_LEAVE_REQUEST from './deputyLeaveRequest';
import GET_LEAVE_REQUEST_DETAIL_QUERY from './getLeaveRequestDetail';

export default (id) => <Query query={GET_LEAVE_REQUEST_DETAIL_QUERY} variables={{ id: id }}>
    {({ data: { leaveRequests }, loading }) => loading ? <>Loading</> :
        <main>
            <Paper style={{ padding: "10px" }}>
                <Typography component="h1" variant="h4" align="center">Leave Request Detail</Typography>
                <Form data={leaveRequests[0]}></Form>
            </Paper>
        </main>}
</Query>


const deleteButton = (deleteLeaveRequest, { data }) => <Button
    variant="contained"
    color="primary"
    onClick={() => deleteLeaveRequest({ refetchQueries: ["GetCalendarData", "GetPagedLeaveRequests"] })}>
    Delete
</Button>
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

const Form = (props) =>
    <>
        <Typography variant="h6" gutterBottom>
            {props.data.owner}'s  Leave Request {props.data.id}
        </Typography>
        <Grid container spacing={24}>
            <Grid item xs={12}>
                <Mutation
                    mutation={APPROVE_LEAVE_REQUEST}
                    variables={{ id: props.data.id }}
                    children={ApproveAction()} />
                <Mutation
                    mutation={DEPUTY_LEAVE_REQUEST}
                    variables={{ id: props.data.id }}
                    children={DeputyAction(props.data.canDeputyBy)}
                />
            </Grid>
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
            <Grid item xs={12}>
                <FormControlLabel
                    control={<Checkbox color="secondary" name="saveAddress" value="yes" />}
                    label="Use this address for payment details"
                />
            </Grid>
        </Grid>
    </>


