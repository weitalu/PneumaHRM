
import gql from 'graphql-tag'

export default gql`mutation DeleteLeaveRequst($requestId:ID!) {
    deleteLeaveRequest(input: $requestId)
  } 
`