

import React from 'react';
import { render, fireEvent, cleanup, waitForElement } from 'react-testing-library'
import { MockedProvider } from 'react-apollo/test-utils';

import 'jest-dom/extend-expect'
import App from '../App';
import APP_QUERY from '../query'
const mocks = [
  {
    request: {
      query: APP_QUERY
    },
    result: {
      data: {
        self: {
          userName: "tester123"
        },
      },
    },
  },
];
afterEach(cleanup)

it('can render user name', () => {
  render(
    <MockedProvider mocks={mocks}>
      <App />
    </MockedProvider>
  )
});