import {Button} from "../components/Button";
import {WelcomePage} from "./WelcomePage";
import {TextInput} from "../components/TextInput";
import {createUseStyles} from "react-jss";
import {Link} from "react-router-dom";
import Constants from "../Constants";

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

export function Login() {
    const style = useStyles();
    
    return <WelcomePage>
        <div className={style.frameStyle}>
            <div className={style.formStyle}>
                <div>
                    <TextInput type="text" placeholder="Логин"/>
                    <TextInput type="password" placeholder="Пароль"/>
                </div>
                <Button>Войти</Button>
            </div>
            <div>
                <span className={style.registrationLabel}>
                    Вы абитуриент и все еще не с нами? Еще не поздно <Link to="/registration">присоединиться</Link>
                </span>
            </div>
        </div>
    </WelcomePage>
}
