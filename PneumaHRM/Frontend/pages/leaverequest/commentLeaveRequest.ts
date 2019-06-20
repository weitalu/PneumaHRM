import gql from 'graphql-tag'

export default gql`mutation Comment($input:LeaveRequestCommentInput!){
  commentLeaveRequest(input:$input){
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