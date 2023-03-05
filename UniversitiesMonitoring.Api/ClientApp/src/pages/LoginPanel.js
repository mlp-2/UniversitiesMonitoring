import { Outlet } from 'react-router-dom';
import {UserPanel} from "../components/UserPanel";
import {useEffect, useState} from "react";
import {GetUser} from "../ApiMethods";
import {Navigate} from "react-router-dom";
import Swal from "sweetalert2";

export function LoginPanel() {
    const [redirectToLogin, setRedirect] = useState(false);
    
    useEffect(() => {
        (async () => {
            async function fire() {
                await Swal.fire({
                    title: "Вы не авторизованы",
                    text: "Войдите, чтобы использовать данную страницу",
                    icon: "error",
                    position: 'top-end',
                    showConfirmButton: false,
                    timer: 2000,
                    backdrop: false
                });
            }
            
            try {
                if (await GetUser() === null) {
                    setRedirect(true);
                    await fire();
                }
            } catch {
                setRedirect(true);
                await fire();
            }
        })();
    }, []);
    
    if (redirectToLogin) return <Navigate to="/login"/>
    
    return <>
        <UserPanel/>
        <Outlet />
    </>
}