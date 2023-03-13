import {WelcomePage} from "./WelcomePage";
import {createUseStyles} from "react-jss";
import {Link, Navigate} from "react-router-dom";
import Constants from "../Constants";
import axios from "axios";
import {useEffect, useState} from "react";
import useGlobalStyles from "../GlobalStyle";
import Swal from "sweetalert2-neutral";
import {GetUser} from "../ApiMethods";
import {Button, Container, Form} from "react-bootstrap";

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
    },
    defaultAuthContainer: {
        display: "flex",
        flexDirection: "column",
        justifyContent: "center",
        gap: 20,
        "& span": {
            fontSize: "16px",
            textAlign: "center"
        },
        "& button": {
            width: "50%"
        }
    }
})

export function Login() {
    const style = useStyles();
    const [user, setUser] = useState(null);

    useEffect(() => {
        async function getLogin() {
            try {
                const defaultAuth = await GetUser();

                if (defaultAuth !== null) {
                    setUser(defaultAuth);
                }

                console.info("Default user found")
            } catch {
                console.info("Default user not found. Using form auth strategy")
            }
        }

        getLogin();
    }, []);

    return <WelcomePage>
        <div className={style.frameStyle}>
            {
                user !== null ? <DefaultAuth user={user}
                                             throwNotThisUser={() => setUser(null)}/> :
                    <FormAuth/>
            }
            <div>
                <span className={style.registrationLabel}>
                    Вы абитуриент и все еще не с нами? Еще не поздно <Link to="/registration">присоединиться</Link>
                </span>
            </div>
        </div>
    </WelcomePage>
}

function DefaultAuth({user, throwNotThisUser}) {
    const [accepted, setAccepted] = useState(false);

    const style = useStyles();
    const globalStyle = useGlobalStyles();

    function handleResult(res) {
        if (res) {
            setAccepted(true);
        } else {
            throwNotThisUser();
        }
    }

    if (accepted) return <Navigate to="/universities-list"/>;

    return <div className={style.defaultAuthContainer}>
        <Container className="d-flex flex-column align-items-center justify-content-center gap-2">
            <span>Это Вы, <b><span className={globalStyle.brandFontColored}>@{user.username}</span></b>?</span>
            <Button onClick={() => handleResult(true)}>Да</Button>
            <Button variant="danger" onClick={() => handleResult(false)}>Нет, выйти</Button>
        </Container>
    </div>;
}

function FormAuth() {
    const [formDialogFinished, setFinished] = useState(false);
    const [loading, setLoading] = useState(false);
    const style = useStyles();

    async function handleSubmit(e) {
        e.preventDefault();

        setLoading(true);
        const form = e.target;
        const formData = new FormData(form);

        try {
            const response = await axios.post("/api/user/auth", Object.fromEntries(formData.entries()));

            localStorage.setItem("token", response.data.jwt)

            setFinished(true);
        } catch (axiosError) {
            const response = axiosError.response;

            await Swal.fire({
                icon: "error",
                title: response.status !== 500 ? "Неверный логин или пароль" :
                    "Что-то пошло не так...",
                showConfirmButton: false,
                timer: 2000
            });
        }

        setLoading(false);
    }

    if (formDialogFinished) return <Navigate to="/universities-list"/>;

    return <Form method="post" className={style.formStyle} onSubmit={handleSubmit}>
        <Form.Control disabled={loading} className="mb-3" type="text" placeholder="Логин" name="username"/>
        <Form.Control disabled={loading} className="mb-3" type="password" placeholder="Пароль" name="password"/>
        <Button disabled={loading} type="submit">Войти</Button>
    </Form>;
}
