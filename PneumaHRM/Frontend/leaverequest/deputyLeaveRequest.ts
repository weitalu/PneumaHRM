import { gql } from 'apollo-boost'

export default gql`mutation Deputy($id:Int!, $comment:String!){
    deputyLeaveRequest(leaveRequestId:$id, comment:$comment)
  }
`