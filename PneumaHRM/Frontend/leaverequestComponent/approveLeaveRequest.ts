import gql from 'graphql-tag'

export default gql`mutation Approve($input:LeaveRequestCommentInput!){
  approveLeaveRequest(input:$input){
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