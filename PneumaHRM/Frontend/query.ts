import { gql } from 'apollo-boost'

export default gql`
query GetUserName
{
  self {
    userName
  }
}
`