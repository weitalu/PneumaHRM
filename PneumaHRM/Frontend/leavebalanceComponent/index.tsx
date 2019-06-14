import React, { useState } from 'react';

import ExpansionPanel from '@material-ui/core/ExpansionPanel';
import ExpansionPanelSummary from '@material-ui/core/ExpansionPanelSummary';
import ExpansionPanelDetails from '@material-ui/core/ExpansionPanelDetails';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';

import { Query, Mutation } from 'react-apollo';

import GET_EMPLOYEE from './getEmployee';
import GET_LEAVE_BALANCE from './getLeaveBalances';
import BalanceTableView from './balancesTableView';

export default () => {
    return <Query query={GET_EMPLOYEE}>
        {LoadingFallback(data => data.employees.map(x => <Employee employee={x} />))}
    </Query>
}

const LoadingFallback = (Comp: (data: any, client: any) => JSX.Element) => ({ data, loading, client }) => loading ? <>Loading</> : Comp(data, client);

const Employee = ({ employee }) => {
    let [state, setState] = useState(false);
    let detail = state ? LoadEmployeeBalance(employee) : <></>
    return <ExpansionPanel onChange={(e, expended) => setState(expended)}>
        <ExpansionPanelSummary
            expandIcon={<ExpandMoreIcon />}>
            {employee.userName}, current leave hours : {employee.currentBalance}
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