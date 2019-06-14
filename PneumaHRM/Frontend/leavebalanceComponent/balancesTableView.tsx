import React, { useState } from 'react';

import Button from '@material-ui/core/Button';
import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableFooter from '@material-ui/core/TableFooter';
import TableHead from '@material-ui/core/TableHead';
import TablePagination from '@material-ui/core/TablePagination';
import TableRow from '@material-ui/core/TableRow';
import Tooltip from '@material-ui/core/Tooltip';
import Paper from '@material-ui/core/Paper';

import moment from 'moment';

export default (balances) => <Table>
    <TableHead>
        <TableRow>
            <TableCell>Create On</TableCell>
            <TableCell>ID</TableCell>
            <TableCell>Created by</TableCell>
            <TableCell>Value</TableCell>
            <TableCell></TableCell>
        </TableRow>
    </TableHead>
    <TableBody>
        {balances.map((row, index) => <TableRow key={index}>
            <Tooltip title={moment(row.createdOn).format('llll')}>
                <TableCell>
                    {moment(row.createdOn).fromNow()}
                </TableCell>
            </Tooltip>
            <TableCell> {row.id}</TableCell>
            <TableCell>
                {row.createdBy}
            </TableCell>
            <TableCell>
                {row.value}
            </TableCell>
            <TableCell>
                {row.description}
            </TableCell>
        </TableRow >)}
    </TableBody>
</Table>