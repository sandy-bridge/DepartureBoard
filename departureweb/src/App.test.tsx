import React from 'react';
import { render, screen } from '@testing-library/react';
import { App } from './App';

it('renders "Welcome to Your Fluent UI App"', () => {
  render(<App />);
  const linkElement = screen.getByText(/Departures/i);
  expect(linkElement).toBeInTheDocument();
});
