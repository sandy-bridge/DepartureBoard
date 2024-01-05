import logo from './logo.svg';
import './App.css';
import { useStop } from './api/departure'

function App() {
  const { isLoading, error, data } = useStop();
  if (isLoading) return 'Loading...';
  if (error) console.log('An error occurred while fetching the user data ', error);
  return (
      <div>
      <h1>{data?.stopName}</h1>
      <p>{data?.departures}</p>
    </div>
  );
}

export default App;
