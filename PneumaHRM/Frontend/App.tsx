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

import MenuIcon from '@material-ui/icons/Menu'
import NotificationsIcon from '@material-ui/icons/Notifications'
import DashboardIcon from '@material-ui/icons/Dashboard';
import NoteAddIcon from '@material-ui/icons/NoteAdd';
import AccountBalance from '@material-ui/icons/AccountBalance'
//import {AppBar} from '@material-ui/core' all the package will be boundled 

import { NavLink, Route } from 'react-router-dom'

import { Query } from 'react-apollo';

import APP_QUERY from './query'
import DashboardComponent from './dashboardComponent/index'
import LeaveRequestComponent from './leaverequestComponent/index'
import LeaveBalanceComponent from './leavebalanceComponent/index'
const drawerWidth = 240;
const routes = [
    {
        path: "/dashboard",
        exact: true,
        header: () => "Dashboard",
        content: DashboardComponent,
        icon: DashboardIcon
    },
    {
        path: "/balance",
        header: () => "Leave Balance",
        content: LeaveBalanceComponent,
        icon: AccountBalance
    },
    {
        path: "/leaverequest",
        header: () => "Leave Request",
        content: LeaveRequestComponent,
        icon: NoteAddIcon
    }
];
const myLink = path => props => <NavLink to={path} {...props} />
const toItem = route =>
    <ListItem button component={myLink(route.path)}>
        <ListItemIcon>
            <route.icon></route.icon>
        </ListItemIcon>
        <ListItemText primary={route.header()} />
    </ListItem>
export default () => <Query query={APP_QUERY}>
    {({ data: { self }, loading }) => {
        if (loading || !self) {
            return <div>Loading ...</div>;
        }

        return (
            <App myName={self.userName} />
        );
    }}
</Query>
class App extends React.Component<{ myName: string }> {
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
                            render={() => <Typography
                                component="h1"
                                variant="h6"
                                color="inherit"
                                noWrap>
                                {route.header() + ", Hello " + this.props.myName}
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
                    </IconButton>
                </div>
                <Divider />
                <List>
                    <div>
                        {routes.map(toItem)}
                    </div>
                </List>
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
