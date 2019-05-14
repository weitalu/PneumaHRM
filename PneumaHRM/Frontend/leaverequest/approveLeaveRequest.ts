import { gql } from 'apollo-boost'

export default gql`mutation Approve($id:Int!){
    approveLeaveRequest(leaveRequestId:$id)
  }
`