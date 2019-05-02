import React from 'react';

import { Mutation } from 'react-apollo';
import { Redirect } from 'react-router-dom'
import Button from '@material-ui/core/Button';

import DELETE_LEAVE_REQUEST from './deleteLeaveRequest';

export default ({ match: { params: { id } } }) => {
    console.log("Leave Request ");
    console.log(id);
    return <Mutation
        mutation={DELETE_LEAVE_REQUEST}
        variables={{ requestId: id }}>
        {aa}
    </Mutation>;
}

const aa = (deleteLeaveRequest, { data, called }) => {
    console.log(called);
    console.log(data);
    if (called) return <Redirect to="/dashboard"></Redirect>
    return <Button
        variant="contained"
        color="primary"
        onClick={() => deleteLeaveRequest({ refetchQueries: ["GetCalendarData"])}>
        Delete
</Button>
}