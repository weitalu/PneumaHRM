import React, { useState } from 'react';

import ExpansionPanel from '@material-ui/core/ExpansionPanel';
import ExpansionPanelSummary from '@material-ui/core/ExpansionPanelSummary';
import ExpansionPanelDetails from '@material-ui/core/ExpansionPanelDetails';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';

import { Query, Mutation } from 'react-apollo';

import GET_EMPLOYEE from './getEmployee';
import GET_LEAVE_BALANCE from './getLeaveBalances';

export default () => {
    let [currentDetail, updateCurrentDetail] = useState("");
    let displayDetail = currentDetail === employee.userName;
    return <Query query={GET_EMPLOYEE}>
        {({ data, loading }) => loading ? <>Loading</> : data.employees.map((employee) =>
            <ExpansionPanel expanded={displayDetail}>
                <ExpansionPanelSummary
                    expandIcon={<ExpandMoreIcon onClick={() => updateCurrentDetail(employee.userName)} />}>
                    {employee.userName}, now have {employee.currentBalance} leave balance</ExpansionPanelSummary>
                {displayDetail ? <ExpansionPanelDetails>This is balance Detail</ExpansionPanelDetails> : <></>}

            </ExpansionPanel>)}
    </Query>
}
