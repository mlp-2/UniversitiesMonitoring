import {WelcomePage} from "./WelcomePage";
import {TextInput} from "../components/TextInput";
import {Button} from "../components/Button";
import {Link, Navigate} from "react-router-dom";
import {createUseStyles} from "react-jss";
import Constants from "../Constants";
import {SubmitButton} from "../components/SubmitButton";
import Swal from "sweetalert2";
import axios from "axios";
import {useState} from "react";

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
    const [registrationDialogEndedWithSuccess, setDialog] = useState(false);
    
    async function handleFormSubmit(e) {
        e.preventDefault();
        
        const form = e.target;
        const formData = new FormData(form);
        const apiEntity = Object.fromEntries(formData.entries());
        
        if (apiEntity.username === "" ||
            apiEntity.password === "" ||
            apiEntity.retryPassword === "") {
            throwError("Ни одно поле не должно быть пустым");
        }
        
        if (apiEntity.username.length < 6) {
            throwError("Имя пользователя должно содержать как минимум 6 символов");   
        }
        
        if (apiEntity.password.length < 8) {
            throwError("Пароль должен быть как минимум из 8 символов");
        }
        
        if (apiEntity.password !== apiEntity.retryPassword) {
            throwError("Пароли должны совпадать");
        }
        
        try {
            const result = await axios.post(`/api/user/register`, apiEntity);
            
            if (result.status !== 200) return;
            
            await Swal.fire({
               title: `Мы очень рады Вас видеть, ${apiEntity.username}!`,
               icon: "success",
                showConfirmButton: false,
                timer: 2000
            });
            
            localStorage.setItem("token", result.data.jwt);
        } catch (error) {
            console.log(error);
            await Swal.fire({
                title: `Такой пользователь уже существует`,
                icon: "error",
                showConfirmButton: false,
                timer: 2000
            });
        }
    }
    
    if (registrationDialogEndedWithSuccess) {
        return <Navigate to="/universities-list"/>;
    }
    
    return <WelcomePage>
        <div className={style.frameStyle}>
            <form className={style.formStyle} method="port" onSubmit={handleFormSubmit}>
                <div>
                    <TextInput type="text" name="username" placeholder="Логин"/>
                    <TextInput type="password" name="password" placeholder="Пароль"/>
                    <TextInput type="password" name="retryPassword" placeholder="Повторите пароль"/>
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

function throwError(text) {
    Swal.fire({
        title: text,
        icon: "error",
        position: "top-end",
        showConfirmButton: false,
        timer: 1000,
        backdrop: false
    });
}