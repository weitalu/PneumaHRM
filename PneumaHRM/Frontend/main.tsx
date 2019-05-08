import React from 'react';
import { render } from "react-dom";
import { BrowserRouter } from 'react-router-dom';

import { ApolloProvider } from 'react-apollo';
import { ApolloClient } from 'apollo-client';
import { InMemoryCache } from 'apollo-cache-inmemory';
import { HttpLink } from 'apollo-link-http';

import App from './App';

const cache = new InMemoryCache();

const httpLink = new HttpLink({
    uri: "/api/graphql"
});
const client = new ApolloClient({
    link: httpLink,
    cache,
});

render(
    <ApolloProvider client={client}>
        <BrowserRouter>
            <App />
        </BrowserRouter>
    </ApolloProvider>,
    document.querySelector('#root'));