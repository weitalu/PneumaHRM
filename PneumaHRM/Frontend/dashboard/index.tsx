import React from 'react';

import { Redirect } from 'react-router-dom'
import moment from 'moment';

import CALENDER_DATA_QUERY from './getCalendarData'
import WORKHOURS_DATA_QUERY from './getWorkHours'
import CREATE_LEAVEREQUEST_MUTATION from './createLeaveRequest'
import { Query, Mutation } from 'react-apollo';

import Button from '@material-ui/core/Button';
import Grid from '@material-ui/core/Grid';
import Paper from '@material-ui/core/Paper';
import TextField from '@material-ui/core/TextField';
import Typography from '@material-ui/core/Typography';

import CalendarApp from './calendar'

interface internalState {
  inputFocusOn: "start" | "end" | "none"
  start: moment.Moment
  end: moment.Moment
}
export default class extends React.Component<{}, internalState> {
  constructor(props) {
    super(props)

    this.state = {
      inputFocusOn: "none",
      start: moment("2019-01-01"),
      end: moment("2019-01-05")
    }
  }
  render() {
    return <Query query={CALENDER_DATA_QUERY} variables={{ start: "2019-01-01" }}>
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
                  focusOn={(value) => this.setState({ inputFocusOn: value })} />
              </Grid>
              <Grid item xs={6}>
                <CalendarApp
                  holidays={holidays.map(holidayDataToInput)}
                  leaves={leaveRequests.map(leaveRequestToInput)}
                  onDateSelected={v => {
                    if (this.state.inputFocusOn === "start") {
                      this.setState({ start: moment(v) });
                    } else {
                      this.setState({ end: moment(v) });
                    }
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



const CreateLeaveRequestApp = ({ start, end, focusOn }) =>
  <Query query={WORKHOURS_DATA_QUERY} variables={{ from: start, to: end }}>
    {leaveRequestAppView(start, end, focusOn)}
  </Query>
const leaveRequestAppView = (start, end, focusOn) => ({ data: { workHours }, loading }) => loading ? <></> : <Paper style={{ padding: "20px" }}>
  <Typography variant="h5" component="h3">
    Create Leave Request
  </Typography>
  <TextField
    label="Leave Request Start"
    margin="normal"
    fullWidth
    variant="filled"
    value={start}
    style={{ margin: "8px" }}
    autoFocus 
    onFocus={() => focusOn("start")} />
  <TextField
    label="Leave Request End"
    margin="normal"
    fullWidth
    variant="filled"
    value={end}
    style={{ margin: "8px" }}
    autoFocus 
    onFocus={() => focusOn("end")} />
  <TextField
    id="filled-read-only-input"
    label="Calculated Work Hours"
    margin="normal"
    value={workHours}
    fullWidth
    InputProps={{
      disabled: true,
    }}
    variant="filled"
    style={{ margin: "8px" }}
  />
  <Button variant="contained" color="primary">Create</Button>
</Paper>

const holidayDataToInput = day => ({
  title: day.name,
  start: day.value,
  allday: true
})

const leaveRequestToInput = leave => ({
  title: `${leave.type} ${leave.owner} workHour:${leave.workHour}`,
  start: leave.from,
  end: leave.to,
  redirect: <Redirect push to={"/leaverequest/" + leave.id} />
})