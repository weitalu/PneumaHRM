
import { gql } from 'apollo-boost'

export default gql`mutation CreateLeaveRequest($leaveRequest:LeaveRequestInput!){
    createLeaveRequest(leaveRequest:$leaveRequest){ 
        from 
        to 
        id 
        owner
        type 
    }
}
`