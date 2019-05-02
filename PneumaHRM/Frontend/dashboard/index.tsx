import React from 'react';

import { Redirect } from 'react-router-dom'
import moment from 'moment';

import CALENDER_DATA_QUERY from './getCalendarData'
import WORKHOURS_DATA_QUERY from './getWorkHours'
import CREATE_LEAVEREQUEST_MUTATION from './createLeaveRequest'
import { Query, Mutation } from 'react-apollo';

import Button from '@material-ui/core/Button';
import Grid from '@material-ui/core/Grid';
import MenuItem from '@material-ui/core/MenuItem';
import Paper from '@material-ui/core/Paper';
import TextField from '@material-ui/core/TextField';
import Typography from '@material-ui/core/Typography';

import CalendarApp from './calendar'
import { throwServerError } from 'apollo-link-http-common';

const calenderStart = "2019-01-01"
const leaveTypes = ["ANNUAL",
  "OVER_TIME",
  "SICK",
  "PERSONAL",
  "OTHER"]
interface internalState {
  start: moment.Moment
  end: moment.Moment
  leaveType: string
}
export default class extends React.Component<{}, internalState> {
  constructor(props) {
    super(props)

    this.state = {
      start: moment(),
      end: moment(),
      leaveType: leaveTypes[0]
    }
  }
  render() {
    return <Query query={CALENDER_DATA_QUERY} variables={{ start: calenderStart }}>
      {({ data: { holidays, leaveRequests }, loading }) => {
        if (loading || !holidays || !leaveRequests) {
          return <div>Loading ...</div>;
        }
        return (
          <div>

            <Grid container spacing={24}>
              <Grid item xs={3}>
                <CreateLeaveRequestApp
                  start={this.state.start.format()}
                  end={this.state.end.format()}
                  leaveType={this.state.leaveType}
                  setLeaveType={(value) => this.setState({ leaveType: value })} />
              </Grid>
              <Grid item xs={6}>
                <CalendarApp
                  holidays={holidays.map(holidayDataToInput)}
                  leaves={leaveRequests.map(leaveRequestToInput)}
                  onDateSelected={(start, end) => {
                    this.setState({
                      start: moment(start),
                      end: moment(end)
                    });
                  }} />
              </Grid>
              <Grid item xs={3}>
              </Grid>
            </Grid>
          </div>
        );
      }}
    </Query>
  }
}



const CreateLeaveRequestApp = ({ start, end, leaveType, setLeaveType }) =>
  <Query query={WORKHOURS_DATA_QUERY} variables={{ from: start, to: end }}>
    {leaveRequestAppView(start, end, leaveType, setLeaveType)}
  </Query>
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
  <Mutation refetchQueries={[{ query: CALENDER_DATA_QUERY, variables: { start: calenderStart } }]}
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

const holidayDataToInput = day => ({
  title: day.name,
  start: day.value,
  allday: true,
  rendering: 'background'
  color: '#ff9f89'
})

const leaveRequestToInput = leave => ({
  title: `Taker: ${leave.owner}, Type: ${leave.type}`,
  start: leave.from,
  end: leave.to,
  redirect: leave.id != null ? <Redirect push to={"/leaverequest/" + leave.id} /> : null
})