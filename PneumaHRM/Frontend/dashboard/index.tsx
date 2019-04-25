import React from 'react';

import gql from 'graphql-tag';
import { Query } from 'react-apollo';

const GET_REPOSITORIES_OF_ORGANIZATION = gql`
{
    holidays(where: [{path: "value", comparison: greaterThanOrEqual, value: ["2019-01-01"]}]) {
      value
      description
    }
    leaveRequests {
      from
      to
      owner
    }
    leaveRequestsConnection {
      totalCount
      pageInfo {
        endCursor
        hasNextPage
      }
      items {
        id
        name
        state
        type
        from
        to
      }
    }
  }
  
`;


export default () => <Query query={GET_REPOSITORIES_OF_ORGANIZATION}>
    {({ data: { holidays }, loading }) => {
        if (loading || !holidays) {
            return <div>Loading ...</div>;
        }

        return (
            <>{JSON.stringify(holidays)}</>
        );
    }}
</Query>