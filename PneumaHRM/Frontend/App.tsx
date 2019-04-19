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

import ChevronLeftIcon from '@material-ui/icons/ChevronLeft';
import MenuIcon from '@material-ui/icons/Menu'
import NotificationsIcon from '@material-ui/icons/Notifications'
import DashboardIcon from '@material-ui/icons/Dashboard';
import NoteAddIcon from '@material-ui/icons/NoteAdd';
//import {AppBar} from '@material-ui/core' all the package will be boundled 

import { Link, Route } from 'react-router-dom'

import Dashboard from './dashboard/index'
import LeaveRequest from './leaverequest/index'

const mainListItems = (
    <div>
        <ListItem button component={Link} to="/dashboard">
            <ListItemIcon>
                <DashboardIcon />
            </ListItemIcon>
            <ListItemText primary="Dashboard" />
        </ListItem>
        <ListItem button component={Link} to="/leaverequest">
            <ListItemIcon>
                <NoteAddIcon />
            </ListItemIcon>
            <ListItemText primary="Leave Request" />
        </ListItem>
    </div>
);

export default class App extends React.Component {
    render() {
        return <div>
            <CssBaseline />
            <AppBar position="absolute">
                <Toolbar>
                    <IconButton
                        color="inherit"
                        aria-label="Open drawer">
                        <MenuIcon />
                    </IconButton>
                    <Typography
                        component="h1"
                        variant="h6"
                        color="inherit"
                        noWrap>
                        Haha
                    </Typography>
                    <IconButton color="inherit">
                        <Badge badgeContent={4} color="secondary">
                            <NotificationsIcon />
                        </Badge>
                    </IconButton>
                </Toolbar>
            </AppBar>
            <Drawer
                variant="permanent"
            >
                <div>
                    <IconButton>
                        <ChevronLeftIcon />
                    </IconButton>
                </div>
                <Divider />
                <List>{mainListItems}</List>
            </Drawer>

            <main>
                <Route path="/dashboard" exact component={Dashboard} />
                <Route path="/leaverequest" component={LeaveRequest} />
            </main>
        </div>
    }
}
