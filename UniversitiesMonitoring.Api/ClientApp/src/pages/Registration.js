import {WelcomePage} from "./WelcomePage";
import {TextInput} from "../components/TextInput";
import {Button} from "../components/Button";
import {Link} from "react-router-dom";
import {createUseStyles} from "react-jss";
import Constants from "../Constants";
import {SubmitButton} from "../components/SubmitButton";

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
    }
})

export function Registration() {
    const style = useStyles();
    
    return <WelcomePage>
        <div className={style.frameStyle}>
            <form className={style.formStyle}>
                <div>
                    <TextInput type="text" placeholder="Логин"/>
                    <TextInput type="password" placeholder="Пароль"/>
                    <TextInput type="password" placeholder="Повторите пароль"/>
                </div>
                <SubmitButton content="Регистрация"/>
            </form>
            <div>
                <span className={style.registrationLabel}>
                    Вы уже с нами? <Link to="/login">Войти</Link>
                </span>
            </div>
        </div>
    </WelcomePage>
}