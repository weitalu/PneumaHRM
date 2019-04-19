import React from 'react';
import AppBar from '@material-ui/core/AppBar'
import Badge from '@material-ui/core/Badge'
import CssBaseline from '@material-ui/core/CssBaseline'
import Divider from '@material-ui/core/Divider'
import Drawer from '@material-ui/core/Drawer';
import IconButton from '@material-ui/core/IconButton'
import List from '@material-ui/core/List'
import ListItem from '@material-ui/core/ListItem';
import ListItemIcon from '@material-ui/core/ListItemIcon';
import ListItemText from '@material-ui/core/ListItemText';
import Toolbar from '@material-ui/core/Toolbar'
import Typography from '@material-ui/core/Typography'

import ChevronLeftIcon from '@material-ui/icons/ChevronLeft'
import MenuIcon from '@material-ui/icons/Menu'
import NotificationsIcon from '@material-ui/icons/Notifications'
import DashboardIcon from '@material-ui/icons/Dashboard';
import NoteAddIcon from '@material-ui/icons/NoteAdd';
//import {AppBar} from '@material-ui/core' all the package will be boundled 

import { Link, Route } from 'react-router-dom'

import Dashboard from './dashboard/index'
import LeaveRequest from './leaverequest/index'
const drawerWidth = 240;
const routes = [
    {
        path: "/dashboard",
        exact: true,
        header: () => "Dashboard",
        content: Dashboard,
        icon: DashboardIcon
    },
    {
        path: "/leaverequest",
        header: () => "Leave Request",
        content: LeaveRequest,
        icon: NoteAddIcon
    }
];
const mainListItems = (
    <div>
        {routes.map((route) =>
            (<ListItem button component={Link} to={route.path}>
                <ListItemIcon>
                    <route.icon></route.icon>
                </ListItemIcon>
                <ListItemText primary={route.header()} />
            </ListItem>)}
    </div>
);

export default class App extends React.Component {
    render() {
        return <div>
            <CssBaseline />
            <AppBar position="absolute"
                style={{ width: `calc(100% - ${drawerWidth}px)`, marginLeft: drawerWidth }}>
                <Toolbar>
                    <IconButton
                        color="inherit"
                        aria-label="Open drawer">
                        <MenuIcon />
                    </IconButton>
                    {routes.map((route, index) => (
                        <Route
                            key={index}
                            path={route.path}
                            exact={route.exact}
                            render={() => <Typography
                                component="h1"
                                variant="h6"
                                color="inherit"
                                noWrap>
                                {route.header()}
                            </Typography>}
                        />
                    ))}

                </Toolbar>
            </AppBar>
            <Drawer
                variant="permanent"
                style={{ position: 'relative', whiteSpace: 'nowrap', width: drawerWidth }}
                open
            >
                <div>
                    <IconButton>
                        <ChevronLeftIcon />
                    </IconButton>
                </div>
                <Divider />
                <List>{mainListItems}</List>
            </Drawer>

            <main style={{ marginLeft: `${drawerWidth}px`, marginTop: "100px" }}>
                {routes.map((route, index) =>
                    (<Route
                        path={route.path}
                        exact={route.exact}
                        component={route.content} />))}
            </main>
        </div>
    }
}
