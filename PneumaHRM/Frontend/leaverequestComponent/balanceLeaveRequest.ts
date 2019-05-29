import gql from 'graphql-tag'

export default gql`mutation BalanceLeaveRequest($requestId:ID!) {
    balanceLeaveRequest(requestId: $requestId)
  } 
`