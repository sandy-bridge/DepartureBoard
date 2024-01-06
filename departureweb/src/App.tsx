import React from 'react';
import './App.css';
import { useStop } from './api/departure';

function App() {
  const { isLoading, error, data } = useStop();

  if (isLoading) return (
  <div className="App">
    <h2>Loading</h2>
  </div>
  );
  if (error) console.log('An error occurred while fetching the user data ', error);
  const departures = data?.departures.map((departure: string) => <li>{departure}</li>)
  return (
    <div className="App">
      <h1>{data?.stopName}</h1>
      <ul>{departures}</ul>
    </div>
  );
}

export default App;
