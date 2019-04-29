import React from 'react';

import DASHBOARD_DATA_QUERY from './query';
import { Query } from 'react-apollo';

import FullCalendar from '@fullcalendar/react'
import { EventInput } from '@fullcalendar/core'
import dayGridPlugin from '@fullcalendar/daygrid'
import timeGridPlugin from '@fullcalendar/timegrid'
import interactionPlugin from '@fullcalendar/interaction' // needed for dayClick


export default () => <Query query={DASHBOARD_DATA_QUERY}>
  {({ data: { holidays, leaveRequests }, loading }) => {
    if (loading || !holidays || !leaveRequests) {
      return <div>Loading ...</div>;
    }

    return (
      <DemoApp holidays={holidays.map(day => ({ title: day.name, start: day.value, allday: true }))}
        leaves={leaveRequests.map(leave => ({ title: `${leave.type} ${leave.owner} workHour:${leave.workHour}`, start: leave.from, end: leave.to }))} />
    );
  }}
</Query>

import './main.scss'
import leaverequest from '../leaverequest';
import { SSL_OP_ALLOW_UNSAFE_LEGACY_RENEGOTIATION } from 'constants';

interface DemoAppState {
  calendarWeekends: boolean
  calendarEvents: EventInput[]
}

class DemoApp extends React.Component<{ holidays: EventInput[], leaves: EventInput[] }, DemoAppState> {

  calendarComponentRef = React.createRef<FullCalendar>()

  constructor(props: {}) {
    super(props)

    this.state = {
      calendarWeekends: true,
      calendarEvents: []
    }
  }

  render() {
    return (
      <div className='demo-app'>
        <div className='demo-app-calendar'>
          <FullCalendar
            defaultView="dayGridMonth"
            header={{
              left: 'prev,next today',
              center: 'title',
              right: 'dayGridMonth,timeGridWeek,timeGridDay,listWeek'
            }}
            plugins={[dayGridPlugin, timeGridPlugin, interactionPlugin]}
            ref={this.calendarComponentRef}
            weekends={this.state.calendarWeekends}
            events={this.props.holidays.concat(this.props.leaves)}
            dateClick={(arg) => this.handleDateClick(arg)}
          />
        </div>
      </div>
    )
  }

  handleDateClick = (arg) => {
    console.log(arg);
  }

}