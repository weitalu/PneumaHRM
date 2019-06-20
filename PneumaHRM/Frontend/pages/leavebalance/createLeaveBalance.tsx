import React, { useState } from 'react';
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
import LoadingFallback from '../../util/loadingFallback';
import GET_EMPLOYEE from './getEmployee';

export const CREATE_LEAVE_BALANCE = gql`mutation CreateLeaveBalance($balance:LeaveBalanceInput!) {
    createLeaveBalance(input:$balance){
      id
    }
  }`

export default ({open,onClose}) => {
    let [ownerId,setOwnerId]=useState("");
    let [value,setValue]=useState("0");
    let [description,setDescription]=useState("");
  return (
    <div>
        <Dialog open={open} onClose={onClose} aria-labelledby="form-dialog-title">
          <DialogTitle>Create Balance</DialogTitle>
          <DialogContent>
            <DialogContentText>
              Create Balance
            </DialogContentText>
            <Query query={GET_EMPLOYEE}>
              {LoadingFallback(data =>
                <TextField
                margin="dense"
                select
                label="balance on (employee)"
                fullWidth
                value = {ownerId}
                onChange={(e)=>setOwnerId(e.target.value)}
                children={data.employees.map(employee=><MenuItem key={employee.key} value={employee.key} children={employee.userName} />)}
                />)}
            </Query>
            <TextField
              margin="dense"
              label="balance value"
              value = {value}
              onChange={(e)=>setValue(e.target.value)}
              fullWidth
              number
            />
            <TextField
              margin="dense"
              label="description"
              value={description}
              onChange={(e)=>setDescription(e.target.value)}
              fullWidth
            />
          </DialogContent>
          <DialogActions>
            <Button onClick={onClose} color="primary">
              Cancel
            </Button>
            <Mutation
              mutation={CREATE_LEAVE_BALANCE}
              refetchQueries={["GetLeaveBalance","GetEmployee"]}
              variables={{ balance: { ownerId,value,description } }}
              >
                  {(create)=><Button onClick={()=>{create();onClose();}} color="primary" variant="contained" children={"Create"} />}
            </Mutation>
            
          </DialogActions>
        </Dialog>
    </div>)
  );
}