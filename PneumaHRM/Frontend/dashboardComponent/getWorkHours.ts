import { gql } from 'apollo-boost'

export default gql`
query GetWorkHours($from: DateTime!, $to: DateTime!) {
  workHours(from:$from to:$to)
}
`