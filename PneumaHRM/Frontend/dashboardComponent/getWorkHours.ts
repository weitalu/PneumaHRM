import gql from 'graphql-tag'

export default gql`
query GetWorkHours($from: DateTime!, $to: DateTime!) {
  workHours(from:$from to:$to)
}
`