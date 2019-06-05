import gql from 'graphql-tag'

export default gql`mutation CompleteLeaveRequest($input:ID!) {
  completeLeaveRequest(input: $input){
    comments {
      content
      createdBy
      createdOn
      type
    }
  }
} 
`