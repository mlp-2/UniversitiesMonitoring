import {useEffect, useState} from "react";

export function AccountPage() {
    const [user, setUser] = useState();
    
    useEffect(() => {
        setUser(getUser());
    }, []);
    
    if (user === null) return <span>Загрузка</span>
}

function getUser() {
    
}