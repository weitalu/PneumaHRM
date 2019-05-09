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

import moment from 'moment';

import { Query, Mutation } from 'react-apollo';
import GET_LEAVE_REQUESTS_QUERY from './getPagedLeaveRequests';
import DELETE_LEAVE_REQUEST from './deleteLeaveRequest';
import ROWS_PER_PAGE_OPTIONS from '../rowsPerPageOptions';
import createRedirect from '../util/createRedirect';

export default (pageNum = 0, pageSize = ROWS_PER_PAGE_OPTIONS[0], setPageNum, setPageSize, toDetail) => <Paper>
    <Table>
        <TableHead>
            <TableRow>
                <TableCell>Create On</TableCell>
                <TableCell>ID</TableCell>
                <TableCell>Leave Taker</TableCell>
                <TableCell>From</TableCell>
                <TableCell>To</TableCell>
                <TableCell>WorkHour</TableCell>
                <TableCell>Type</TableCell>
                <TableCell>State</TableCell>
                <TableCell></TableCell>
            </TableRow>
        </TableHead>
        <Query query={GET_LEAVE_REQUESTS_QUERY}
            variables={{ skip: pageNum * pageSize, take: pageSize }}
            fetchPolicy="cache-and-network">
            {({ data, loading }) => loading ? <>Loading</> :
                <>
                    <TableBody>
                        {data.leaveRequests.map(leaveRequestToTableRow(toDetail))}
                    </TableBody>
                    <TableFooter>
                        <TableRow>
                            <TablePagination
                                rowsPerPageOptions={ROWS_PER_PAGE_OPTIONS}
                                colSpan={3}
                                count={data.page.totalCount}
                                rowsPerPage={pageSize}
                                page={pageNum}
                                onChangePage={(e, p) => setPageNum(p)}
                                onChangeRowsPerPage={({ target: { value } }) => setPageSize(value)}
                            />
                        </TableRow>
                    </TableFooter>
                </>}
        </Query>
    </Table>
</Paper>
const leaveRequestToTableRow = (toDetail) => (row, index) => (
    <TableRow key={index}>
        <TableCell onClick={(e) => toDetail(row.id)}>
            {moment(row.createdOn).fromNow()}
        </TableCell>
        <TableCell onClick={(e) => toDetail(row.id)}>
            {row.id}
        </TableCell>
        <TableCell>
            {row.owner}
        </TableCell>
        <TableCell>{moment(row.from).format('llll')}</TableCell>
        <TableCell>{moment(row.to).format('llll')}</TableCell>
        <TableCell>{row.workHour}</TableCell>
        <TableCell>{row.type}</TableCell>
        <TableCell>{row.state}</TableCell>
        <TableCell>
            <Mutation
                mutation={DELETE_LEAVE_REQUEST}
                variables={{ requestId: row.id }}
                refetchQueries={["GetCalendarData", "GetPagedLeaveRequests"]}>
                {deleteAction(row.canDelete)}
            </Mutation>
            {DeputyAction(row.canDeputyBy)()}
            <ApproveAction />
            <CompleteAction />
        </TableCell>
    </TableRow >
)

const deleteAction = (canDelete) => (deleteLeaveRequest) => <Button
    variant="contained"
    color="primary"
    size="small"
    disabled={!canDelete}
    style={{ marginLeft: "1px" }}
    onClick={() => deleteLeaveRequest()}>Delete</Button>

const DeputyAction = (canDeputyBy) => (deputyLeaveRequest) => <Button
    variant="contained"
    size="small"
    disabled={!canDeputyBy}
    style={{ marginLeft: "1px" }}
    color="primary"
    onClick={() => deputyLeaveRequest()}>Deputy</Button>

const ApproveAction = () => <Button
    variant="contained"
    size="small"
    style={{ marginLeft: "1px" }}
    color="primary">Approve</Button>

const CompleteAction = () => <Button
    variant="contained"
    size="small"
    style={{ marginLeft: "1px" }}
    color="primary">Complete</Button>
const ViewAction = () => <Button
    variant="contained"
    size="small"
    style={{ marginLeft: "1px" }}
    color="primary"> View</Button>