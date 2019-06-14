import React from 'react';


export default (Comp: (data: any, client: any) => JSX.Element) => ({ data, loading, client }) => loading ? <>Loading</> : Comp(data, client);