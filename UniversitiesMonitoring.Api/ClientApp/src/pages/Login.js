import {WelcomePage} from "./WelcomePage";
import {TextInput} from "../components/TextInput";
import {createUseStyles} from "react-jss";
import {Link, Navigate} from "react-router-dom";
import Constants from "../Constants";
import {SubmitButton} from "../components/SubmitButton";
import axios from "axios";
import {useEffect, useState} from "react";
import {Button} from "../components/Button";
import useGlobalStyles from "../GlobalStyle";
import {Container} from "reactstrap";

const useStyles = createUseStyles({
    formStyle: {
        width: "100%",
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        "& div": {
            display: "flex",
            flexDirection: "column",
            width: "100%"
        },
        "& div input": {
            margin: "10px 0 10px 0",
            width: "100%"
        },
        "& button": {
            width: "50%",
            marginTop: "6vh"
        }
    },
    frameStyle: {
        display: "flex",
        flexDirection: "column",
        justifyContent: "flex-start",
        gap: "15vh",
        height: "80%"
    },
    registrationLabel: {
        fontSize: 24,
        "& a": {
            color: Constants.brandColor,
            textDecoration: "none"
        }
    },
    defaultAuthContainer: {
        display: "flex",
        flexDirection: "column",
        justifyContent: "center",
        gap: 20,
        "& span": {
            fontSize: "32px",
            textAlign: "center"
        },
        "& button": {
            width: "50%"
        }
    }
})

export function Login() {
    const style = useStyles();
    const [formError, setFormError] = useState(null)
    const [defaultAuthAccepted, setDefAuthAccepted] = useState(null);
    const [user, setUser] = useState(null);
    const globalStyle = useGlobalStyles();
    
    useEffect(() => {
        async function getLogin() {
            const defaultAuth = await GetUser();

            if (defaultAuth !== null) {
                setUser(defaultAuth);
            }
        }
        getLogin();
    }, []);
    
    
    if (defaultAuthAccepted !== null) {
        if (defaultAuthAccepted) {
            return <Navigate to="/universities-list"/>;
        } 
        
        localStorage.setItem("token", undefined);
        setUser(null);
        setDefAuthAccepted(null);
    }
    
    async function handleSubmit(e) {
        e.preventDefault();
        
        const form = e.target;
        const formData = new FormData(form);
        
        const response = await axios.post("/api/user/auth", Object.fromEntries(formData.entries()));
        
        if(response.status !== 200) {
            setFormError("Неверный логин или пароль");
            return;
        }
        
        localStorage.setItem("token", response.data.jwt)
        setUser(await GetUser());
    }
    
    if (formError !== null) {
        alert("Сам проблемс")
    }
    
    return <WelcomePage>
        <div className={style.frameStyle}>
            {
                user !== null ?  <div className={style.defaultAuthContainer}>
                        <Container className="d-flex flex-column align-items-center justify-content-center gap-2">
                            <span>Это Вы, <b><span className={globalStyle.brandFontColored}>@{user.username}</span></b>?</span>
                            <Button onClick={() => setDefAuthAccepted(true)}>Да</Button>
                            <Button onClick={() => setDefAuthAccepted(false)} style={{background: "#FF4D15"}}>Нет, выйти</Button>
                        </Container>
                    </div> :
                    <form method="post" className={style.formStyle} onSubmit={handleSubmit}>
                        <div>
                            <TextInput type="text" name="username" placeholder="Логин"/>
                            <TextInput type="password" name="password" placeholder="Пароль"/>
                        </div>
                        <SubmitButton value="Войти"/>
                    </form>
            }
            <div>
                <span className={style.registrationLabel}>
                    Вы абитуриент и все еще не с нами? Еще не поздно <Link to="/registration">присоединиться</Link>
                </span>
            </div>
        </div>
    </WelcomePage>
}

async function GetUser() {
    const result = await axios.get("/api/user");
    
    if (result.status === 200) return result.data;
    else return null;
}
