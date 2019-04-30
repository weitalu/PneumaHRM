import React from 'react';

import { Redirect } from 'react-router-dom'

import FullCalendar from '@fullcalendar/react'
import { EventInput } from '@fullcalendar/core'
import dayGridPlugin from '@fullcalendar/daygrid'
import timeGridPlugin from '@fullcalendar/timegrid'
import interactionPlugin from '@fullcalendar/interaction' // needed for dayClick

import './main.scss'

interface InternalState {
    redirect?: Redirect
}
type RedirectEventInput = EventInput & { redirect: Redirect };
export default class extends React.Component<{ holidays: EventInput[], leaves: RedirectEventInput[], onDateSelected?: (value: Date) => void }, InternalState> {

    calendarComponentRef = React.createRef<FullCalendar>()

    constructor(props) {
        super(props)

        this.state = {
        }
    }

    render() {
        if (this.state.redirect) {
            return this.state.redirect;
        }
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
                        weekends={true}
                        events={this.props.holidays.concat(this.props.leaves)}
                        dateClick={({ date }) => this.props.onDateSelected(date)}
                        eventClick={({ event: { extendedProps: { redirect } } }) => redirect == null ? console.log("nothing") : this.setState({ redirect: redirect })}
                    />
                </div>
            </div>
        )
    }
}