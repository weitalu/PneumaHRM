import React from 'react';
import { render } from "react-dom";
import { BrowserRouter } from 'react-router-dom';

import { ApolloProvider } from 'react-apollo';
import { ApolloClient } from 'apollo-client';
import { InMemoryCache } from 'apollo-cache-inmemory';
import { HttpLink } from 'apollo-link-http';
import gql from 'graphql-tag'

import App from './App';

const cache = new InMemoryCache();
cache.writeData({
    data: {
        currentComment: ""
    }
});
const typeDefs = gql`
  extend type Query {
    currentComment:String
  }
`;
const httpLink = new HttpLink({
    uri: "/api/graphql"
});

const client = new ApolloClient({
    link: httpLink,
    cache,
    typeDefs: typeDefs,
    connectToDevTools: true,
    resolvers: {}
});


render(
    <ApolloProvider client={client}>
        <BrowserRouter>
            <App />
        </BrowserRouter>
    </ApolloProvider>,
    document.querySelector('#root'));