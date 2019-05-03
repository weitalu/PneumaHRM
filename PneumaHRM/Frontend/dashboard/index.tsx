import React from 'react';

import { Redirect } from 'react-router-dom'
import moment from 'moment';

import CALENDER_DATA_QUERY from './getCalendarData'
import { Query } from 'react-apollo';

import Grid from '@material-ui/core/Grid';
import CalendarApp from './calendar'
import CreateLeaveRequestApp from './createLeaveRequestApp'

const calenderStart = "2019-01-01"
interface internalState {
  start: moment.Moment
  end: moment.Moment
}
export default class extends React.Component<{}, internalState> {
  constructor(props) {
    super(props)

    this.state = {
      start: moment(),
      end: moment()
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
                  end={this.state.end.format()} />
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