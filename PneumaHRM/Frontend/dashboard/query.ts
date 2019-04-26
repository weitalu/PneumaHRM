import { gql } from 'apollo-boost'

export default gql`
{
  holidays(where: [{path: "value", comparison: greaterThanOrEqual, value: ["2019-01-01"]}]) {
    value
    name
    description
  }
  leaveRequests {
    from
    to
    owner
  }
  leaveRequestsConnection {
    totalCount
    pageInfo {
      endCursor
      hasNextPage
    }
    items {
      id
      name
      state
      type
      from
      to
    }
  }
}
`