import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faDoorClosed, faHome, faUser} from "@fortawesome/free-solid-svg-icons";
import {faTelegram} from '@fortawesome/free-brands-svg-icons'
import {useEffect, useState} from "react";
import {Link, Navigate} from "react-router-dom";
import {createUseStyles} from "react-jss";
import Constants from "../Constants";
import {GetUser} from "../ApiMethods";

const useStyles = createUseStyles({
    userPanel: {
        position: "fixed",
        right: 10,
        top: 10,
        background: "#FFF",
        padding: 10,
        borderRadius: "3em",
        display: "flex",
        flexDirection: "row",
        gap: 12,
        zIndex: 100000000,
        '& svg': {
            marginLeft: 5,
            marginRight: 5
        },
        "& #home-link": {
            color: "#000"
        },
        "& #quit-link": {
            color: "#FF4D15"
        },
        "& a": {
            color: Constants.brandColor,
            fontWeight: "bolder"
        }
    }
});

export function UserPanel() {
    const [user, setUser] = useState(null);
    const style = useStyles();

    useEffect(() => {
        (async () => {
            setUser(await GetUser());
        })();
    }, [])

    if (user === null) return <></>
    else if (user === "ERROR") return <Navigate to="/login"/>

    return <div className={style.userPanel}>
        <div>
            <FontAwesomeIcon icon={faUser}/>
            <Link to="/account">@{user.username}</Link>
        </div>
        <div>
            <Link to="/universities-list" id="home-link"><FontAwesomeIcon icon={faHome}/></Link>
            <Link to="https://t.me/ummonitoring"><FontAwesomeIcon icon={faTelegram}/></Link>
            <Link to="/login" id="quit-link" onClick={() => {
                sessionStorage.removeItem("user");
                localStorage.removeItem("user");
                localStorage.removeItem("token");
            }}>
                <FontAwesomeIcon icon={faDoorClosed}/>
            </Link>
        </div>
    </div>
}