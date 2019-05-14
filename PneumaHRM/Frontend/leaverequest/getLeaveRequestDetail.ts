import { gql } from 'apollo-boost'

export default gql`
query GetLeaveRequestDetail($id: String!) {
  leaveRequests(id: $id) {
    deputies
    approves 
    id
    from
    to
    type
    owner
    canDelete
    createdOn
    description
    workHour
    canDeputyBy
  }
}
`