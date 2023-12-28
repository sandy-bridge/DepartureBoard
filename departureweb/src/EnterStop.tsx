import React, { useState } from 'react';
import { Stack,TextField, PrimaryButton } from "@fluentui/react";

function EnterStop(props:any) {
    const [stopID, setStopID] = useState("");    
    const enterStop = () => {      
        props.enterStop(stopID);
        setStopID("");
    }
    const setStop = (e: any) =>{
        setStopID(e.target.value);
    }

    return (
        <Stack>
            <Stack horizontal >
                <Stack.Item grow>
                    <TextField placeholder="Stop ID" value={stopID} onChange={setStop }/>
                </Stack.Item>
                <PrimaryButton onClick={EnterStop} >Enter</PrimaryButton>
            </Stack>
        </Stack>
    );
}

export default EnterStop;
