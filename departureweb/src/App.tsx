import React from 'react';
import './App.css';
import { useStop } from './api/departure';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';

function App() {
  const { isLoading, error, data } = useStop();

  if (isLoading) return (
  <div className="App">
    <h2>Loading</h2>
  </div>
  );
  if (error) console.log('An error occurred while fetching the user data ', error);
  const departures = data?.departures.map((departure: string) => <ListItem>{departure}</ListItem>)
  return (
    <div className="App">
      <h1>{data?.stopName}</h1>
      <List>
        {departures}
      </List>
    </div>
  );
}

export default App;
