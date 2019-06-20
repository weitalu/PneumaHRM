import React from 'react';

import { Route } from 'react-router-dom'
import moment from 'moment';

import CALENDER_DATA_QUERY from './getCalendarData'
import { Query } from 'react-apollo';

import Grid from '@material-ui/core/Grid';
import CalendarApp from './calendarComp'
import CreateLeaveRequestApp from './createLeaveRequestComp'


const calenderStart = "2019-01-01"
interface internalState {
  start: moment.Moment
  end: moment.Moment
}
export default class extends React.Component<any, internalState> {
  constructor(props) {
    super(props)

    this.state = {
      start: moment(),
      end: moment()
    }
  }
  render() {
    console.log(this.props.history)
    return <Query query={CALENDER_DATA_QUERY} variables={{ start: calenderStart }}>
      {({ data: { holidays, leaveRequests }, loading }) => {
        if (loading || !holidays || !leaveRequests) {
          return <div>Loading ...</div>;
        }
        return (
          <div id="Dashboard">

            <Grid container spacing={24}>
              <Grid item xs={3}>
                <CreateLeaveRequestApp
                  start={this.state.start}
                  end={this.state.end} />
              </Grid>
              <Grid item xs={6}>
                <CalendarApp
                  holidays={holidays.map(holidayDataToInput)}
                  leaves={leaveRequests.map(leaveRequestToInput(this.props.history))}
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
  rendering: 'background',
  color: '#ff9f89'
})

const leaveRequestToInput = (history) => (leave) => ({
  title: `${leave.owner}, ${leave.type}`,
  start: leave.from,
  end: leave.to,
  handleClick: () => history.push({
    pathname: '/leaverequest',
    search: 'id=' + leave.id
  })
})