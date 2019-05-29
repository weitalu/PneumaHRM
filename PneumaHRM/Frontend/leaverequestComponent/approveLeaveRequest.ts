import { gql } from 'apollo-boost'

export default gql`mutation Approve($id:Int!, $comment:String!){
  approveLeaveRequest(leaveRequestId:$id, comment:$comment)
}
`