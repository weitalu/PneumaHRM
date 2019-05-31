import gql from 'graphql-tag'

export default gql`mutation Approve($id:Int!, $comment:String!){
  approveLeaveRequest(leaveRequestId:$id, comment:$comment){
    state
    comments {
      content
      createdBy
      createdOn
      type
    }
  }
}
`