import { Outlet } from 'react-router-dom';
import {UserPanel} from "../components/UserPanel";


export function LoginPanel() {
    return <>
        <UserPanel/>
        <Outlet />
    </>
}