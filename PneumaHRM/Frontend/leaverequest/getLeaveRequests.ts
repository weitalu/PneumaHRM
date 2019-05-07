import { gql } from 'apollo-boost'

export default gql`
query GetPagedLeaveRequests($search: [WhereExpressionGraph], $skip: Int, $take: Int) {
  page:leaveRequestsConnection(where: $search) {
    totalCount
  }
  leaveRequests(where: $search, skip: $skip, take: $take, orderBy: [{path:"id" descending: true}]) {
    owner
    id
    from
    to
    type
    state
  }
}
`