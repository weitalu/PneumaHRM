import React from 'react';

import ExpansionPanel from '@material-ui/core/ExpansionPanel';
import ExpansionPanelSummary from '@material-ui/core/ExpansionPanelSummary';
import ExpansionPanelDetails from '@material-ui/core/ExpansionPanelDetails';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';

import tableView from './tableView'
import detailView from './detailView';

export default class extends React.Component {
    constructor(props) {
        super(props);

        this.state = {};
    }
    render() {
        let { id } = this.state;
        let { search } = this.props.location;
        if (search && !id) id = new URLSearchParams(search).get('id');
        let { pageNum, pageSize } = this.state;
        let tableJSX = tableView(
            pageNum,
            pageSize,
            p => this.setState({ pageNum: p }),
            size => this.setState({ pageNum: 0, pageSize: size }),
            id => this.setState({ id: id });
        );
        let detailJSX = id ? detailView(id) : <></>

        return <>
            <ExpansionPanel defaultExpanded>
                <ExpansionPanelSummary expandIcon={<ExpandMoreIcon />}>
                    Leave Request List
                </ExpansionPanelSummary>
                <ExpansionPanelDetails>
                    {tableJSX}
                </ExpansionPanelDetails>
            </ExpansionPanel>
            {detailJSX}
        </>
    }
};

const a = (p: number) => (state, props) => ({ pageNum: p })