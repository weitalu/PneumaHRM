import React from 'react';
import { render } from "react-dom";
import { BrowserRouter, Route, Link } from 'react-router-dom';

import Dashboard from './dashboard/Dashboard'


render(<BrowserRouter>
    <div>
        <ul>
            <li>
                <Link to="/">Home</Link>
            </li>
            <li>
                <Link to="/about">About</Link>
            </li>
            <li>
                <Link to="/topics">Topics</Link>
            </li>
        </ul>

        <hr />

        <Route exact path="/" component={Dashboard} />
        <Route path="/about" component={Dashboard} />
        <Route path="/topics" component={Dashboard} />
    </div>
</BrowserRouter>, document.querySelector('#root'));