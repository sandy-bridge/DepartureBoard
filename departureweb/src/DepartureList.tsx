import React from 'react';
import { Stack, Label } from "@fluentui/react";
import DepartureItem from './DepartureItem';

function DepartureList(props:any) {

    return (
        <Stack gap={10} >
            { props.departures.length > 0 ? props.departures.map((departure: any) => (
                <DepartureItem departure={departure} key={departure.id}/>
            )): 
            <Label>Departure list is empty...</Label>}
        </Stack>
    );
}

export default DepartureList;