import gql from 'graphql-tag'

export default gql`query GetLeaveBalance{
  leaveBalances{value id owner}
}
`  