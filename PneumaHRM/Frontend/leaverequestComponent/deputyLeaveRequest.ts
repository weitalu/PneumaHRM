import gql from 'graphql-tag'

export default gql`mutation Deputy($input:LeaveRequestCommentInput!){
    deputyLeaveRequest(input:$input){
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