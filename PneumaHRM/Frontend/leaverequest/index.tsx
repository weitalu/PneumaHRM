import React from 'react';

import Button from '@material-ui/core/Button';
import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableFooter from '@material-ui/core/TableFooter';
import TableHead from '@material-ui/core/TableHead';
import TablePagination from '@material-ui/core/TablePagination';
import TableRow from '@material-ui/core/TableRow';
import Paper from '@material-ui/core/Paper';

import { Link } from 'react-router-dom'

import { Query, Mutation } from 'react-apollo';
import GET_LEAVE_REQUESTS_QUERY from './getLeaveRequests';
import DELETE_LEAVE_REQUEST from './deleteLeaveRequest';

import DetailView from './detail';
import ROWS_PER_PAGE_OPTIONS from '../rowsPerPageOptions';

export default (props) => {
    console.log(props);
    console.log(new URLSearchParams(props.location.search).get("id"));
    return tableView;
};

let temp = { skip: 0, take: 5 };

const tableView = <Paper>
    <Table>
        <TableHead>
            <TableRow>
                <TableCell>ID</TableCell>
                <TableCell>Leave Taker</TableCell>
                <TableCell>From</TableCell>
                <TableCell>To</TableCell>
                <TableCell>Type</TableCell>
                <TableCell>State</TableCell>
                <TableCell></TableCell>
            </TableRow>
        </TableHead>
        <Query query={GET_LEAVE_REQUESTS_QUERY} variables={temp}>
            {({ fetchMore, data, loading }) => loading ? <>Loading</> :
                <>
                    <TableBody>
                        {data.leaveRequests.map(leaveRequestToTableRow)}
                    </TableBody>
                    <TableFooter>
                        <TableRow>
                            <TablePagination
                                rowsPerPageOptions={ROWS_PER_PAGE_OPTIONS}
                                colSpan={1}
                                count={data.page.totalCount}
                                rowsPerPage={5}
                                page={0}
                                onChangePage={(e, p) => console.log(p)}
                                onChangeRowsPerPage={({ target: { value } }) => console.log(value)}
                            />
                        </TableRow>
                    </TableFooter>
                </>}
        </Query>
    </Table>
</Paper>

const leaveRequestToTableRow = row => (
    <TableRow key={row.id}>
        <TableCell>
            {row.id}
        </TableCell>
        <TableCell>
            {row.owner}
        </TableCell>
        <TableCell>{row.from}</TableCell>
        <TableCell>{row.to}</TableCell>
        <TableCell>{row.type}</TableCell>
        <TableCell>{row.state}</TableCell>
        <TableCell>
            <Mutation
                mutation={DELETE_LEAVE_REQUEST}
                variables={{ requestId: row.id }}
                refetchQueries={["GetCalendarData", "GetPagedLeaveRequests"]}>
                {deleteAction}
            </Mutation>
            <ApproveAction />
        </TableCell>
    </TableRow>
)

const deleteAction = (deleteLeaveRequest, { data, called }) => <Button
    variant="contained"
    color="primary"
    size="small"
    onClick={() => deleteLeaveRequest()}>
    Delete</Button>

const ApproveAction = () => <Button
    variant="contained"
    size="small"
    color="primary">Approve</Button>