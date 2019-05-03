import React from 'react';

import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableHead from '@material-ui/core/TableHead';
import TableRow from '@material-ui/core/TableRow';
import Paper from '@material-ui/core/Paper';

import { Link } from 'react-router-dom'

import { Query } from 'react-apollo';
import GET_LEAVE_REQUESTS_QUERY from './getLeaveRequests';
import DetailView from './detail';

export default ({ match: { url } }) => {
    console.log(url);
    return listView;
};



const listView = <Query query={GET_LEAVE_REQUESTS_QUERY}>
    {({ data: { leaveRequests }, loading }) => loading ? <>Loading</> : <Paper>
        <Table>
            <TableHead>
                <TableRow>
                    <TableCell>Leave Taker</TableCell>
                    <TableCell align="right">Type</TableCell>
                    <TableCell align="right">From</TableCell>
                    <TableCell align="right">To</TableCell>
                </TableRow>
            </TableHead>
            <TableBody>
                {leaveRequests.map(row => (
                    <TableRow key={row.id}>
                        <TableCell component="th" scope="row">
                            {row.owner}
                        </TableCell>
                        <TableCell align="right">{row.type}</TableCell>
                        <TableCell align="right">{row.from}</TableCell>
                        <TableCell align="right">{row.to}</TableCell>
                    </TableRow>
                ))}
            </TableBody>
        </Table>
    </Paper>}
</Query>