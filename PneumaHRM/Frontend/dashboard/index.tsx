import React from 'react';

import DASHBOARD_DATA_QUERY from './query';
import { Query } from 'react-apollo';

import FullCalendar from '@fullcalendar/react'
import { EventInput } from '@fullcalendar/core'
import dayGridPlugin from '@fullcalendar/daygrid'
import timeGridPlugin from '@fullcalendar/timegrid'
import interactionPlugin from '@fullcalendar/interaction' // needed for dayClick


var a = () => <Query query={DASHBOARD_DATA_QUERY} children={({ data, loading }) => <>{JSON.stringify(data)}</>}>
</Query>



import './main.scss'

interface DemoAppState {
  calendarWeekends: boolean
  calendarEvents: EventInput[]
}

export default class DemoApp extends React.Component<{}, DemoAppState> {

  calendarComponentRef = React.createRef<FullCalendar>()

  constructor(props: {}) {
    super(props)

    this.state = {
      calendarWeekends: true,
      calendarEvents: [ // initial event data
        { title: 'Event Now', start: new Date() }
      ]
    }
  }

  render() {
    return (
      <div className='demo-app'>
        <div className='demo-app-top'>
          <button onClick={ this.toggleWeekends }>toggle weekends</button>&nbsp;
          <button onClick={ this.gotoPast }>go to a date in the past</button>&nbsp;
          (also, click a date/time to add an event)
        </div>
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
            events={ this.state.calendarEvents }
            dateClick={ this.handleDateClick }
            />
        </div>
      </div>
    )
  }

  toggleWeekends = () => {
    this.setState({ // update a property
      calendarWeekends: !this.state.calendarWeekends
    })
  }

  gotoPast = () => {
    let calendarApi = this.calendarComponentRef.current!.getApi()
    calendarApi.gotoDate('2000-01-01') // call a method on the Calendar object
  }

  handleDateClick = (arg: any) => {
    if (confirm('Would you like to add an event to ' + arg.dateStr + ' ?')) {
      this.setState({  // add new event data
        calendarEvents: this.state.calendarEvents.concat({ // creates a new array
          title: 'New Event',
          start: arg.date,
          allDay: arg.allDay
        })
      })
    }
  }

}