import React from 'react';

import { Redirect } from 'react-router-dom'

import FullCalendar from '@fullcalendar/react'
import { EventInput } from '@fullcalendar/core'
import dayGridPlugin from '@fullcalendar/daygrid'
import timeGridPlugin from '@fullcalendar/timegrid'
import listPlugin from '@fullcalendar/list';
import interactionPlugin from '@fullcalendar/interaction' // needed for dayClick

import './main.scss'

interface CalendarProps {
    holidays: EventInput[],
    leaves: RedirectEventInput[],
    onDateSelected?: (start: Date, end: Date) => void
}
type RedirectEventInput = EventInput & { redirect: Redirect };
export default class extends React.Component<CalendarProps> {

    calendarComponentRef = React.createRef<FullCalendar>()
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
                        plugins={[dayGridPlugin, timeGridPlugin, interactionPlugin, listPlugin]}
                        ref={this.calendarComponentRef}
                        weekends={true}
                        events={this.props.holidays.concat(this.props.leaves)}
                        selectable
                        select={(info) => this.props.onDateSelected(info.start, info.end)}
                        eventClick={({ event: { extendedProps: { handleClick } } }) => handleClick != null ? handleClick() : console.log("no handle")}
                    />
                </div>
            </div>
        )
    }
}