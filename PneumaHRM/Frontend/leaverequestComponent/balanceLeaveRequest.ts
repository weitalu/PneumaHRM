import { gql } from 'apollo-boost'

export default gql`mutation BalanceLeaveRequest($requestId:ID!) {
    balanceLeaveRequest(requestId: $requestId)
  } 
`