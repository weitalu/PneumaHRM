import React from 'react';

import DASHBOARD_DATA_QUERY from './query';
import { Query } from 'react-apollo';

import FullCalendar from '@fullcalendar/react'
import { EventInput } from '@fullcalendar/core'
import dayGridPlugin from '@fullcalendar/daygrid'
import timeGridPlugin from '@fullcalendar/timegrid'
import interactionPlugin from '@fullcalendar/interaction' // needed for dayClick


export default () => <Query query={DASHBOARD_DATA_QUERY} children>
 {({ data: { holidays }, loading }) => {
      if (loading || !holidays) {
        return <div>Loading ...</div>;
      }

      return (
        <DemoApp holidays={holidays.map(day=>({title: day.name , start:day.value}))}/>
      );
    }}
</Query>

import './main.scss'

interface DemoAppState {
  calendarWeekends: boolean
  calendarEvents: EventInput[]
}

class DemoApp extends React.Component<{holidays: EventInput[]}, DemoAppState> {

  calendarComponentRef = React.createRef<FullCalendar>()

  constructor(props: {}) {
    super(props)

    this.state = {
      calendarWeekends: true,
      calendarEvents: [ ]
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
            plugins={[ dayGridPlugin, timeGridPlugin, interactionPlugin ]}
            ref={ this.calendarComponentRef }
            weekends={ this.state.calendarWeekends }
            events={ this.props.holidays }
            businessHours={this.props.holidays}
            dateClick={ (arg)=>this.handleDateClick(arg) }
            />
        </div>
      </div>
    )
  }

  handleDateClick = (arg) => {
  }

}