import gql from 'graphql-tag'

export default gql`query GetEmployee{
    employees {
        key
        userName
        currentBalance
      }
}
`  