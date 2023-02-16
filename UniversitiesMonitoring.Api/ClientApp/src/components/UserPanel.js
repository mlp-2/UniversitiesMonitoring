import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faUser, faHome, faDoorClosed} from "@fortawesome/free-solid-svg-icons";
import {useEffect, useState} from "react";
import {Link, Navigate} from "react-router-dom";
import {createUseStyles} from "react-jss";
import Constants from "../Constants";
import {GetUser} from "../ApiMethods";

const useStyles = createUseStyles({
    userPanel: {
        position: "absolute",
        right: 10,
        top: 10,
        background: "#FFF",
        padding: 10,
        borderRadius: "3em",
        display: "flex",
        flexDirection: "row",
        gap: 12,
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
    
    if(user === null) return <></>
    else if(user === "ERROR") return <Navigate to="/login"/>
    
    return <div className={style.userPanel}>
        <div>
            <FontAwesomeIcon icon={faUser}/>
            <Link to="/account">@{user.username}</Link>    
        </div>
        <div>
            <Link to="/universities-list" id="home-link"><FontAwesomeIcon icon={faHome}/></Link>
            <Link to="/login" id="quit-link"><FontAwesomeIcon icon={faDoorClosed}/></Link>    
        </div>
    </div>
}