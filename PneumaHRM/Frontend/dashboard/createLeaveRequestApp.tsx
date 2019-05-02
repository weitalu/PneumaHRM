import React from 'react';

import Button from '@material-ui/core/Button';
import MenuItem from '@material-ui/core/MenuItem';
import Paper from '@material-ui/core/Paper';
import TextField from '@material-ui/core/TextField';
import Typography from '@material-ui/core/Typography';

import { Query, Mutation } from 'react-apollo';

import CALENDER_DATA_QUERY from './getCalendarData'
import WORKHOURS_DATA_QUERY from './getWorkHours'
import CREATE_LEAVEREQUEST_MUTATION from './createLeaveRequest'


export default ({ start, end, leaveType, setLeaveType }) =>
    <Query query={WORKHOURS_DATA_QUERY} variables={{ from: start, to: end }}>
        {leaveRequestAppView(start, end, leaveType, setLeaveType)}
    </Query>
const leaveTypes = ["ANNUAL",
    "OVER_TIME",
    "SICK",
    "PERSONAL",
    "OTHER"]
const leaveRequestAppView = (start, end, leaveType, setLeaveType) => ({ data: { workHours }, loading }) => <Paper style={{ padding: "20px" }}>
    <Typography variant="h5" component="h3">
        Create Leave Request
  </Typography>
    <TextField
        label="Start"
        margin="normal"
        fullWidth
        variant="filled"
        value={start}
        style={{ margin: "8px" }} />
    <TextField
        label="End"
        margin="normal"
        fullWidth
        variant="filled"
        value={end}
        style={{ margin: "8px" }} />
    <Mutation refetchQueries={[{ query: CALENDER_DATA_QUERY, variables: { start: "2019-01-01" } }]}
        mutation={CREATE_LEAVEREQUEST_MUTATION}>
        {(createLeaveRequest, { data }) => <>
            <TextField
                label="Type"
                margin="normal"
                fullWidth
                select
                variant="filled"
                value={leaveType}
                style={{ margin: "8px" }}
                onChange={(e) => setLeaveType(e.target.value)}
            >
                {leaveTypes.map(option => (
                    <MenuItem key={option} value={option}>
                        {option}
                    </MenuItem>
                ))}
            </TextField>
            <TextField
                label="Description"
                fullWidth
                multiline
                variant="filled"
                style={{ margin: "8px" }}
                onChange={e => console.log(e.target.value)}
            />
            <TextField
                label="Calculated Hours"
                margin="normal"
                value={loading ? "loading" : workHours}
                fullWidth
                InputProps={{
                    disabled: true,
                }}
                variant="filled"
                style={{ margin: "8px" }}
            />
            <Button
                variant="contained"
                color="primary"
                onClick={() => createLeaveRequest({
                    variables: {
                        leaveRequest: { name: "testing", start: start, end: end, type: leaveType }
                    }
                })}>Create</Button>
        </>}

    </Mutation>
</Paper>