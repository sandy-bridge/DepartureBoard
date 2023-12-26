import React from 'react';
import { Stack, Label } from '@fluentui/react';

function DepartureItem(props: any) {
    return (
        <Stack>
            <Stack horizontal verticalAlign="center" horizontalAlign="space-between">
                <Label>{props.departure.name}</Label>
            </Stack>
        </Stack>
    );
}

export default DepartureItem