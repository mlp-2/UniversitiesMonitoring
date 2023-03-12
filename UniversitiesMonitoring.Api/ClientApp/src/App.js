import React, {Component} from 'react';
import {Route, Routes} from 'react-router-dom';
import './custom.css';
import {Login} from "./pages/Login";
import {Registration} from "./pages/Registration";
import {AccountPage} from "./pages/AccountPage";
import {UniversityPage} from "./pages/UniversityPage";
import {UniversitiesList} from "./pages/UniversitiesList";
import {ServicePage} from "./pages/ServicePage";
import {LoginPanel} from "./pages/LoginPanel";
import axios from 'axios';
import {ModerationLoginPage} from "./pages/ModerationLoginPage";
import {ReportsPage} from "./pages/ReportsPage";
import {UniversitiesModerationPanel} from "./pages/UniversitiesModerationPanel";
import {ServicesModerationPanel} from "./pages/ServicesModerationPanel";
import {Moderation} from "./pages/Moderation";
import {ModulesModeration} from "./pages/ModulesModeration";
import {ModerationAuthRequiredContainer} from "./pages/ModerationAuthRequiredContainer";

axios.interceptors.request.use(
    config => {
        if (config.url.includes("moderator")) {
            console.info("Detected Moderation API Request. Trying using modToken");
            config.headers.authorization = `Bearer ${localStorage.getItem("modToken")}`;
        } else {
            console.info("Detected User API Request. Trying using modToken");
            config.headers.authorization = `Bearer ${localStorage.getItem("token")}`;
        }
        return config;
    },
    error => {
        return Promise.reject(error);
    }
);

export default class App extends Component {
    static displayName = App.name;

    render() {
        return (
            <Routes>
                <Route key={2} path="registration" element={<Registration/>}/>
                <Route path="/" key={4} element={<LoginPanel/>}>
                    <Route key={3} path="account" element={<AccountPage/>}/>
                    <Route key={5} path="university" element={<UniversityPage/>}/>
                    <Route key={6} path="universities-list" element={<UniversitiesList/>}/>
                    <Route key={7} path="service" element={<ServicePage/>}/>
                </Route>
                <Route path="/" key={1111} element={<ModerationAuthRequiredContainer/>}>
                    <Route key={8} path="moderator" element={<Moderation/>}/>
                    <Route key={10} path="moderator/reports" element={<ReportsPage/>}/>
                    <Route key={11} path="moderator/universities" element={<UniversitiesModerationPanel/>}/>
                    <Route key={12} path="moderator/university" element={<ServicesModerationPanel/>}/>
                    <Route key={13} path="moderator/modules" element={<ModulesModeration/>}/>
                </Route>
                <Route key={9} path="moderator/login" element={<ModerationLoginPage/>}/>
                <Route key={1} index element={<Login/>}/>
                <Route key={1} path="*" element={<Login/>}/>
            </Routes>
        );
    }
}
