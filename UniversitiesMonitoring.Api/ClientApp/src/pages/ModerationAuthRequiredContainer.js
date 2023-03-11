import Swal from "sweetalert2";
import {Navigate, Outlet} from "react-router-dom";
import axios from "axios";
import {useEffect, useState} from "react";

export function ModerationAuthRequiredContainer() {
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
                await axios.get(`/api/moderator/test`);
            } catch {
                setRedirect(true);
                await fire();
            }
        })();
    }, []);

    if (redirectToLogin) return <Navigate to="/moderator/login"/>

    return <Outlet/>
}