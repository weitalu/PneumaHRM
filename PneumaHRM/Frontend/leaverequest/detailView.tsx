import React from 'react';

import { Mutation } from 'react-apollo';
import { Redirect } from 'react-router-dom'
import Button from '@material-ui/core/Button';

import DELETE_LEAVE_REQUEST from './deleteLeaveRequest';

export default (id, toListView) => {
    console.log("Leave Request ");
    console.log(id);
    return <>
        <Button
            variant="contained"
            color="primary"
            onClick={() => toListView()}>To List</Button>
        <Mutation
            mutation={DELETE_LEAVE_REQUEST}
            variables={{ requestId: id }}
            onCompleted={() => toListView()}>
            {deleteButton}
        </Mutation></>;
}

const deleteButton = (deleteLeaveRequest, { data }) => <Button
    variant="contained"
    color="primary"
    onClick={() => deleteLeaveRequest({ refetchQueries: ["GetCalendarData", "GetPagedLeaveRequests"] })}>
    Delete
</Button>

