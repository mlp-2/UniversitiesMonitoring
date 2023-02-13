import {Login} from "./pages/Login";
import {Registration} from "./pages/Registration";
import {UniversitiesList} from "./pages/UniversitiesList";
import {UniversityPage} from "./pages/UniversityPage";
import {ServicePage} from "./pages/ServicePage";
import {AccountPage} from "./pages/AccountPage";

const AppRoutes = [
    {
        path: "universities-list",
        element: <UniversitiesList/>
    },
    {
        path: 'login',
        element: <Login/>
    },
    {
        path: 'registration',
        element: <Registration/>
    },
    {
        path: "university",
        element: <UniversityPage/>
    },
    {
        path: "service",
        element: <ServicePage/>
    },
    {
        path: "account",
        element: <AccountPage/>
    }
];

export default AppRoutes;