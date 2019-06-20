import React, { useState } from 'react';

import Button from '@material-ui/core/Button'
import ExpansionPanel from '@material-ui/core/ExpansionPanel';
import ExpansionPanelSummary from '@material-ui/core/ExpansionPanelSummary';
import ExpansionPanelDetails from '@material-ui/core/ExpansionPanelDetails';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';
import Typography from '@material-ui/core/Typography';

import { Query, Mutation } from 'react-apollo';

import LoadingFallback from '../../util/loadingFallback';
import GET_EMPLOYEE from './getEmployee';
import GET_LEAVE_BALANCE from './getLeaveBalances';
import BalanceTableView from './balancesTableView';
import CreateLeaveBalanceDialog from './createLeaveBalance'

export default () => {
    const [creating, setCreating] = useState(false);
    return <div id="LeaveBalance">
        <Button
            color="primary"
            variant="contained"
            onClick={() => setCreating(true)}
            children={'Create'} />
        <CreateLeaveBalanceDialog open={creating} onClose={() => setCreating(false)} />
        <Query query={GET_EMPLOYEE}>
            {LoadingFallback(data => data.employees.map(x => <Employee employee={x} />))}
        </Query>

    </div>
}

const Employee = ({ employee }) => {
    let [state, setState] = useState(false);
    let detail = state ? LoadEmployeeBalance(employee) : <></>
    return <ExpansionPanel onChange={(e, expended) => setState(expended)}>
        <ExpansionPanelSummary expandIcon={<ExpandMoreIcon />}>
            <Typography>
                {employee.userName}, current leave hours : {employee.currentBalance}
            </Typography>
        </ExpansionPanelSummary>
        <ExpansionPanelDetails>
            {detail}
        </ExpansionPanelDetails>
    </ExpansionPanel>
}

const LoadEmployeeBalance = employee => <Query query={GET_LEAVE_BALANCE}
    variables={{ userName: employee.userName }}
>
    {LoadingFallback(data => BalanceTableView(data.leaveBalances))}
</Query>