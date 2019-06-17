import React from 'react';
import { QueryResult } from 'react-apollo';


export default (Comp: (data: any, client: any) => JSX.Element) => ({ data, loading, client }: QueryResult) => loading ? <>Loading</> : Comp(data, client);