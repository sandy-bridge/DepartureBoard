import { useQuery } from '@tanstack/react-query';

export type TStop = {
    stopName: string;
    departures: Array<string>;
};

export const fetchStop = async() => {
    const res = await fetch('https://localhost:7169/stopdepartures/908');
    if (!res.ok){
        throw new Error('Network error');
    }
    const data = await res.json();
    const departure: TStop = {
        stopName: data?.stopName,
        departures: data?.departures
    };
    return (departure);
}
export const useStop = () => {
    return useQuery({
        queryKey: ['stopDepartures'], 
        queryFn: fetchStop
    });
}