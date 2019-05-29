
import { gql } from 'apollo-boost'

export default gql`mutation DeleteLeaveRequst($requestId:ID!) {
    deleteLeaveRequest(requestId: $requestId)
  } 
`