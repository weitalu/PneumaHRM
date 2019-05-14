import { gql } from 'apollo-boost'

export default gql`mutation Deputy($id:Int!){
    deputyLeaveRequest(leaveRequestId:$id)
  }
`