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
const leaveTypes =
    ["ANNUAL",
        "OVER_TIME",
        "SICK",
        "PERSONAL",
        "OTHER"]

export default class extends React.Component<{ start: string, end: string }, { leaveType: string }> {
    constructor(props) {
        super(props)

        this.state = {
            leaveType: leaveTypes[0]
        }
    }
    render() {
        let { start, end } = this.props;
        return <Query query={WORKHOURS_DATA_QUERY} variables={{ from: start, to: end }}>
            {leaveRequestAppView(
                start,
                end,
                this.state.leaveType,
                (value) => this.setState({ leaveType: value })
            )}
        </Query>
    }
}

const leaveRequestAppView = (start, end, leaveType, setLeaveType) => ({ data: { workHours }, loading }) => {
    let description = "";
    return <Paper style={{ padding: "20px" }}>
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
        <TextField
            label="Type"
            margin="normal"
            fullWidth
            select
            value={leaveType}
            variant="filled"
            style={{ margin: "8px" }}
            onChange={(e) => setLeaveType(e.target.value)}
        >
            {leaveTypes.map(option => (
                <MenuItem
                    key={option}
                    value={option}>
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
            onChange={e => description = e.target.value}
        />
        <TextField
            label="Calculated Hours"
            margin="normal"
            error={workHours === 0}
            value={loading ? "loading" : workHours}
            fullWidth
            InputProps={{
                disabled: true,
            }}
            variant="filled"
            style={{ margin: "8px" }}
        />
        <Mutation refetchQueries={["GetCalendarData", "GetPagedLeaveRequests"]}
            mutation={CREATE_LEAVEREQUEST_MUTATION}>
            {(createLeaveRequest, { data, error, called }) => <>
                <Button
                    variant="contained"
                    color="primary"
                    onClick={() => workHours > 0 ? createLeaveRequest({
                        variables: {
                            leaveRequest: { start: start, end: end, type: leaveType, description: description }
                        }
                    }) : ""}>Create</Button>
            </>}

        </Mutation>
    </Paper>
}