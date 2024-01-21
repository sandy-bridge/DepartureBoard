import React from 'react';
import { render, screen } from '@testing-library/react';
import App from './App';

test('shows stop name', () => {
  render(<App />);
  const linkElement = screen.getByText(/A.H. Tammsaare tee/i);
  expect(linkElement).toBeInTheDocument();
});
