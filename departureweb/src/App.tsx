import React, {useState} from 'react';
import { Stack, Text, Link, FontWeights, IStackTokens, IStackStyles, ITextStyles, ThemeProvider } from '@fluentui/react';
import logo from './logo.svg';
import './App.css';
import DepartureList from './DepartureList';
import EnterStop from './EnterStop';


const boldStyle: Partial<ITextStyles> = { root: { fontWeight: FontWeights.semibold } };
const stackTokens: IStackTokens = { childrenGap: 15 };
const stackStyles: Partial<IStackStyles> = {
  root: {
    width: '960px',
    margin: '0 auto',
    textAlign: 'center',
    color: '#605e5c',
  },
};

export const App: React.FunctionComponent = () => {
  const [departures, setDepartures] = useState([{ id: 1, name: "Todo Item 1" }, { id: 2, name: "Todo Item 2" }]);
  return (
    <ThemeProvider >
    <div className="wrapper">
      <Stack horizontalAlign="center">
        <h1>Departures</h1>
        <Stack style={{ width: 300 }} gap={25}>
          <EnterStop />
          <DepartureList departures={departures} />
        </Stack>
      </Stack>
    </div>
    </ThemeProvider>
  );
};
