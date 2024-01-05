import React from 'react';
import logo from './logo.svg';
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
  return (
    <div className="App">
      <div>
      <h1>{data?.stopName}</h1>
      <p>{data?.departures}</p>
    </div>
    </div>
  );
}

export default App;
