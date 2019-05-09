import React from 'react';

import tableView from './tableView'
import detailView from './detailView';

export default class extends React.Component {
    constructor(props) {
        super(props);

        this.state = {};
    }
    render() {
        let { id } = this.props.location.state || this.state;
        let { pageNum, pageSize } = this.state;

        if (id) return detailView(id, () => { this.props.history.push("/leaverequest"); this.setState({ id: null }); });
        return tableView(
            pageNum,
            pageSize,
            p => this.setState({ pageNum: p }),
            size => this.setState({ pageNum: 0, pageSize: size }),
            id => this.setState({ id: id });
        );
    }
};

