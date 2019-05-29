import { gql } from 'apollo-boost'

export default gql`
query GetCalendarData($start:String!) {
  holidays(where: [{path: "value", comparison: greaterThanOrEqual, value: [$start]}]) {
    value
    name
    description
  }
  leaveRequests {
    from
    to
    owner
    type
    id
  }
}
`