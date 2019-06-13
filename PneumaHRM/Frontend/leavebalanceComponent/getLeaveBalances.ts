import gql from 'graphql-tag'

export default gql`query GetLeaveBalance($userName:String!){
  leaveBalances(where: [{path: "ownerId", comparison: contains, value: [$userName]}], orderBy: [{path: "createdOn", descending: true}]) {
    createdOn
    createdBy
    description
    value
    snapShotData
  }
}
`  