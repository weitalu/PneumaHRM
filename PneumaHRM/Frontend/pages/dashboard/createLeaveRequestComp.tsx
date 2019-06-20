import React from 'react';

import moment from 'moment';

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
    [
        "ANNUAL",
        "OVER_TIME",
        "SICK",
        "PERSONAL",
        "OTHER"
    ]

export default class extends React.Component<{ start: moment.Moment, end: moment.Moment }, { type: string, description: string }> {
    constructor(props) {
        super(props)

        this.state = {
            type: leaveTypes[0],
            description: ""
        }
    }
    render() {
        let { start, end } = this.props;
        return <Query query={WORKHOURS_DATA_QUERY} variables={{ from: start.format(), to: end.format() }}>
            {({ data: { workHours }, loading }) => <Paper style={{ padding: "20px" }}>
                <Typography variant="h5" component="h3">Create Leave Request </Typography>
                <TextField
                    label="Start"
                    margin="normal"
                    fullWidth
                    variant="filled"
                    value={start.format('lll')}
                    style={{ margin: "8px" }} />
                <TextField
                    label="End"
                    margin="normal"
                    fullWidth
                    variant="filled"
                    value={end.format('lll')}
                    style={{ margin: "8px" }} />
                <TextField
                    label="Type"
                    margin="normal"
                    fullWidth
                    select
                    variant="filled"
                    style={{ margin: "8px" }}
                    onChange={e => this.setState({ type: e.target.value })}
                    value={this.state.type}
                >
                    {leaveTypes.map((option, index) => (
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
                    onChange={e => this.setState({ description: e.target.value })}
                    value={this.state.description}
                    variant="filled"
                    style={{ margin: "8px" }}
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
                    mutation={CREATE_LEAVEREQUEST_MUTATION}
                    onCompleted={() => this.setState({ type: leaveTypes[0], description: "" })}
                    variables={{
                        leaveRequest: {
                            start: start.format(),
                            end: end.format(),
                            type: this.state.type,
                            description: this.state.description
                        }
                    }}>
                    {(createLeaveRequest, { data, error, called }) => <>
                        <Button
                            variant="contained"
                            color="primary"
                            onClick={() => workHours > 0 ? createLeaveRequest() : ""}>Create</Button>
                    </>}

                </Mutation>
            </Paper >}
        </Query>
    }
}
