import { gql } from 'apollo-boost';

export default gql`query GetLeaveBalance{
  leaveBalances{value id owner}
}
`  