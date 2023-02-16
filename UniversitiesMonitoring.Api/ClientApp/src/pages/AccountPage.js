import {useEffect, useState} from "react";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faUser} from "@fortawesome/free-solid-svg-icons";
import {TextInput} from "../components/TextInput";
import {SubmitButton} from "../components/SubmitButton";
import {createUseStyles} from "react-jss";
import FireBall from "../assets/images/fireball.svg"

const emailRegex = /[\w\d.]+@[a-z]+\.[a-z]{2,3}/
const useStyles = createUseStyles({
   editEmailForm: {
       display: "flex",
       flexDirection: "column",
       width: "30vw",
       "& input::placeholder": {
            fontSize: 18  
       },
       "& div": {
           display: "flex",
           flexDirection: "column",
           gap: 10
       },
       "& .checkbox-combo": {
           flexDirection: "row",
           fontSize: 15
       },
       "& input[type=submit]": {
           width: "30%",
           fontSize: 15
       }
   },
    layout: {
        display: "flex",
        flexDirection: "row",
        justifyContent: "space-around",
        alignItems: "center",
        height: "100vh",
        background: "#0798EA"
    },
    container: {
       background: "#FFF",
        padding: 20,
        borderRadius: 15
    },
    formsWrapper: {
        display: "flex",
        flexDirection: "column",
        gap: 30,
        fontSize: 20,
        "& .fa-user": {
            marginRight: 10,
            color: "#0798EA"
        },
        "& .username": {
            fontWeight: "bold"
        },
        "& .fa-user, .username": {
            fontSize: 24
        }
    }
});

export function AccountPage() {
    const [user, setUser] = useState(null);
    const style = useStyles();
    
    async function handleEditEmailSettingsFormSubmit(e) {
        const target = e.target;
        const formData = new FormData(target);
        const data = Object.fromEntries(formData.entries());
        
        if (emailRegex.exec(data.email) === data.email) {
            
        }
    }
    
    useEffect(() => {
        setUser(getUser());
    }, []);
    
    if (user === null) return <span>Загрузка</span>
    
    return <div className={style.layout}>
        <div className={style.formsWrapper}>
            <div className={style.container}>
                <FontAwesomeIcon icon={faUser}/>
                <span className="username">@{user.username}</span>
            </div>
            <div className={style.container}>
                <form method="put" className={style.editEmailForm}>
                    <div>
                        <label htmlFor="email">Email для рассылки:</label>
                        <TextInput placeholder="name@domain.ru" name="email" id="email"/>    
                    </div>
                    <div className="checkbox-combo">
                        <label htmlFor="canSend">Присылайте мне уведомления</label>
                        <input type="checkbox" name="canSend" id="canSend" defaultChecked={user.sendToEmail}/>    
                    </div>
                    <SubmitButton content="Изменить" disabled={true}/>
                </form>
            </div>
        </div>
        <div>
            <img src={FireBall} alt="Что-то пошло не так"/>
        </div>
    </div>
}

function getUser() {
    return {
        id: 0,
        username: "DenVot",
        email: "denis@denvot.dev",
        sendToEmail: true
    }
}