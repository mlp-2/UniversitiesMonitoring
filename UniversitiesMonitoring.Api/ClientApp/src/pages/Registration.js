import {WelcomePage} from "./WelcomePage";
import {Link, Navigate} from "react-router-dom";
import {createUseStyles} from "react-jss";
import Constants from "../Constants";
import Swal from "sweetalert2";
import axios from "axios";
import {useState} from "react";
import {Button, Form} from "react-bootstrap";

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
        "& button": {
            width: "50%"
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
        fontSize: 16,
        "& a": {
            color: Constants.brandColor,
            textDecoration: "none"
        }
    }
})

export function Registration() {
    const style = useStyles();
    const [registrationDialogEndedWithSuccess, setDialog] = useState(false);
    const [loading, setLoading] = useState(false);
    
    async function handleFormSubmit(e) {
        e.preventDefault();
        setLoading(true);
        
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
            setDialog(true);
        } catch (error) {
            console.log(error);
            await Swal.fire({
                title: `Такой пользователь уже существует`,
                icon: "error",
                showConfirmButton: false,
                timer: 2000
            });
        }
        
        setLoading(false);
    }
    
    if (registrationDialogEndedWithSuccess) {
        return <Navigate to="/universities-list"/>;
    }
    
    return <WelcomePage>
        <div className={style.frameStyle}>
            <Form method="post" className={style.formStyle} onSubmit={handleFormSubmit}>
                <Form.Control disabled={loading} 
                              className="mb-3"
                              type="text" 
                              placeholder="Логин"
                              name="username"/>
                <Form.Control disabled={loading} 
                              className="mb-3" 
                              type="password"
                              placeholder="Пароль"
                              name="password"/>
                <Form.Control disabled={loading}
                              className="mb-3"
                              type="password" 
                              placeholder="Повторите пароль"
                              name="retryPassword"/>
                <Button disabled={loading} type="submit">Зарегистрироваться</Button>
            </Form>
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