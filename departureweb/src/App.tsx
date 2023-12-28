import React, { useState } from 'react';
import { Stack, TextField, PrimaryButton, FontWeights, IStackTokens, IStackStyles, ITextStyles, ThemeProvider, Label } from '@fluentui/react';
import './App.css';
import DepartureList from './DepartureList';

import {
  useQuery,
  useQueryClient,
  QueryClient,
  QueryClientProvider,
} from 'react-query'

interface Departures {
  stopName: string;
  departures: Array<string>
}

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
const queryClient = new QueryClient()

export const App: React.FunctionComponent = () => {
  const queryClient = useQueryClient()
  let stopID: string = "908";
  const [departures, setDepartures] = useState([{ id: 1, name: "Todo Item 1" }, { id: 2, name: "Todo Item 2" }]);
  const query = useQuery(['departures', stopID], async () => {
    const response = await fetch('http://localhost:7169/stopdepartures/' + stopID);
    if (!response.ok) {
      throw new Error('Network error');
    }
    return response.json();
  })
  return (
    <QueryClientProvider client={queryClient}>
      <ThemeProvider >
        <div className="wrapper">
          <Stack horizontalAlign="center">
            <h1>Departures</h1>
            <Stack style={{ width: 300 }} gap={25}>
              <Stack>
                <Stack horizontal >
                  <Stack.Item grow>
                    <TextField placeholder="Stop ID" value={stopID} />
                  </Stack.Item>
                  <PrimaryButton >Enter</PrimaryButton>
                </Stack>
              </Stack>
              <Stack gap={10} >{query.data.departures.map((departure: string) =>
                <Stack><Label>{departure}</Label></Stack>
              )}
              </Stack>
            </Stack>
          </Stack>
        </div>
      </ThemeProvider>
    </QueryClientProvider>
  );
};
