import gql from 'graphql-tag'

export default gql`
query GetUserName
{
  self {
    userName
  }
}
`