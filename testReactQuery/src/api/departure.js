import { useQuery } from '@tanstack/react-query';

const fetchStop = async() => {
    const res = await fetch('https://localhost:7169/stopdepartures/908');
    if (!res.ok){
        throw new Error('Network error');
    }
    const data = await res.json();
    const departure = {
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