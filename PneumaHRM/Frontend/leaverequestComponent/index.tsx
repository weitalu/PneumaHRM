import React from 'react';

import ExpansionPanel from '@material-ui/core/ExpansionPanel';
import ExpansionPanelSummary from '@material-ui/core/ExpansionPanelSummary';
import ExpansionPanelDetails from '@material-ui/core/ExpansionPanelDetails';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';

import TableView from './tableView'
import detailView from './detailView';

export default class extends React.Component<any, any> {
    constructor(props) {
        super(props);

        this.state = {};
    }
    render() {
        let { id } = this.state;
        let { search } = this.props.location;
        if (search && !id) id = new URLSearchParams(search).get('id');
        let detailJSX = id ? detailView(id) : <></>

        return <>
            <ExpansionPanel defaultExpanded>
                <ExpansionPanelSummary expandIcon={<ExpandMoreIcon />}>
                    Leave Request List
                </ExpansionPanelSummary>
                <ExpansionPanelDetails>
                    <TableView toDetail={id => this.setState({ id: id })} />
                </ExpansionPanelDetails>
            </ExpansionPanel>
            {detailJSX}
        </>
    }
};