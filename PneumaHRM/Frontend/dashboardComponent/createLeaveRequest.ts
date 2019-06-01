
import gql from 'graphql-tag'

export default gql`mutation CreateLeaveRequest($leaveRequest:LeaveRequestInput!){
    createLeaveRequest(input:$leaveRequest){ 
        from 
        to 
        id 
        owner
        type 
    }
}
`