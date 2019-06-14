import React from 'react';
import Button from '@material-ui/core/Button';
import TextField from '@material-ui/core/TextField';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogContentText from '@material-ui/core/DialogContentText';
import DialogTitle from '@material-ui/core/DialogTitle';
import MenuItem from '@material-ui/core/MenuItem';

import gql from 'graphql-tag'
import { Query, Mutation } from 'react-apollo';
import GET_EMPLOYEE from './getEmployee';

export const CREATE_LEAVE_BALANCE = gql`mutation CreateLeaveBalance($balance:LeaveBalanceInput!) {
    createLeaveBalance(input:$balance){
      id
    }
  }`

export default ({open,onClose}) => {
  return (
    <div>
      <Dialog open={open} onClose={onClose} aria-labelledby="form-dialog-title">
        <DialogTitle>Create Balance</DialogTitle>
        <DialogContent>
          <DialogContentText>
            Create Balance
          </DialogContentText>
          <TextField
            autoFocus
            margin="dense"
            select
            label="balance on (employee)"
            fullWidth
          />
          <TextField
            autoFocus
            margin="dense"
            label="balance value"
            fullWidth
            number
          />
          <TextField
            autoFocus
            margin="dense"
            label="description"
            fullWidth
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={onClose} color="primary">
            Cancel
          </Button>
          <Button onClick={onClose} color="primary" variant="contained">
            Create
          </Button>
        </DialogActions>
      </Dialog>
    </div>
  );
}