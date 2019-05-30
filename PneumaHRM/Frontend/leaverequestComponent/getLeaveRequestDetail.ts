import gql from 'graphql-tag'

export default gql`
query GetLeaveRequestDetail($id: String!) {
  currentComment @client
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
    comments {
      content
      createdBy
      createdOn
      type
    }
  }
}
`