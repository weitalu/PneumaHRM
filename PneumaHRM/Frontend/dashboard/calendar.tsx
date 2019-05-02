import React from 'react';

import { Redirect } from 'react-router-dom'

import FullCalendar from '@fullcalendar/react'
import { EventInput } from '@fullcalendar/core'
import dayGridPlugin from '@fullcalendar/daygrid'
import timeGridPlugin from '@fullcalendar/timegrid'
import listPlugin from '@fullcalendar/list';
import interactionPlugin from '@fullcalendar/interaction' // needed for dayClick

import './main.scss'

interface InternalState {
    redirect?: Redirect
}
interface CalendarProps {
    holidays: EventInput[],
    leaves: RedirectEventInput[],
    onDateSelected?: (start: Date, end: Date) => void
}
type RedirectEventInput = EventInput & { redirect: Redirect };
export default class extends React.Component<CalendarProps, InternalState> {

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
                        plugins={[dayGridPlugin, timeGridPlugin, interactionPlugin, listPlugin]}
                        ref={this.calendarComponentRef}
                        weekends={true}
                        events={this.props.holidays.concat(this.props.leaves)}
                        selectable
                        select={(info) => this.props.onDateSelected(info.start, info.end)}
                        eventClick={({ event: { extendedProps: { redirect } } }) => redirect == null ? console.log("nothing") : this.setState({ redirect: redirect })}
                    />
                </div>
            </div>
        )
    }
}