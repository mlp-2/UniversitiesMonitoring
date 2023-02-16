import {useLocation} from "react-router-dom";
import {useEffect, useState} from "react";
import axios from "axios";

export function ReportsPage() {
    const jwt = useLocation().state.jwt;
    console.log(jwt)
    
    const requestConfig = {
        headers: `Bearer ${jwt}`
    };
    
    const [reports, setReports] = useState(null);
    
    useEffect(() => {
        (async () => {
            const result = await axios.get("/api/moderator/reports", requestConfig);
        })();
    }, []);
    
    if (reports === null) return <span>Загружаем сообщения...</span>
    
    return <div>
        
    </div>
}