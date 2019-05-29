import gql from 'graphql-tag'

export default gql`mutation Deputy($id:Int!, $comment:String!){
    deputyLeaveRequest(leaveRequestId:$id, comment:$comment)
  }
`