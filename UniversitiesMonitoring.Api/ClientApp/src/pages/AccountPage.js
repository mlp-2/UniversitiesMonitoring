import {useEffect, useRef, useState} from "react";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faUser} from "@fortawesome/free-solid-svg-icons";
import {createUseStyles} from "react-jss";
import FireBall from "../assets/images/fireball.svg"
import Swal from "sweetalert2-neutral";
import axios from "axios";
import {Button, Form} from "react-bootstrap";
import {GetUser} from "../ApiMethods";

const useStyles = createUseStyles({
    editEmailForm: {
        display: "flex",
        flexDirection: "column",
        width: "100%",
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
    },
    "@media screen and (max-width: 1000px)": {
        layout: {
            "& #fireball": {
                display: "none"
            }
        }
    }
});

export function AccountPage() {
    const [user, setUser] = useState(null);
    const checkboxRef = useRef();
    const emailRef = useRef();
    const style = useStyles();

    async function updateEmailSettings(data) {
        if (data.email === null || data.email === "") {
            return;
        }

        try {
            const result = await axios.put("/api/user/email/update", data);

            if (result.status === 200) {
                await Swal.fire({
                    title: "Настройки оповещения обновлены",
                    icon: "success",
                    showConfirmButton: false,
                    timer: 2000
                });

                user.email = data.email;
                user.sendToEmail = data.canSend;

                sessionStorage.setItem("user", JSON.stringify(user));
            }
        } catch (e) {
            console.log(e)
            await Swal.fire({
                title: e.response.status === 400 ? "Такой email уже занят" : "Что-то пошло не так",
                icon: "error",
                showConfirmButton: false,
                timer: 2000
            });
        }
    }

    async function handleEditEmailSettingsFormSubmit(e) {
        e.preventDefault();
        const target = e.target;
        const formData = new FormData(target);
        let data = Object.fromEntries(formData.entries());

        data.canSend = checkboxRef.current.checked;

        if (data.email === "") {
            await updateEmailSettings({
                email: null,
                canSend: false
            });
        } else {
            await updateEmailSettings(data);
        }
    }

    useEffect(() => {
        (async () => {
            const fetchedUser = await GetUser();
            setUser(fetchedUser);
        })();
    }, []);

    if (user === null) return <span>Загрузка</span>

    return <div className={style.layout}>
        <div className={style.formsWrapper}>
            <div className={style.container}>
                <FontAwesomeIcon icon={faUser}/>
                <span className="username">@{user.username}</span>
            </div>
            <div className={style.container}>
                <Form method="put" className={style.editEmailForm} onSubmit={handleEditEmailSettingsFormSubmit}>
                    <div>
                        <label htmlFor="email">Email для рассылки:</label>
                        <Form.Control defaultValue={user.email} type="email" ref={emailRef} placeholder="name@domain.ru"
                                      name="email" id="email"/>
                    </div>
                    <div className="checkbox-combo">
                        <label htmlFor="canSend">Присылайте мне уведомления</label>
                        <Form.Check ref={checkboxRef} name="canSend" id="canSend" defaultChecked={user.sendToEmail}/>
                    </div>
                    <Button type="submit">Отправить</Button>
                </Form>
            </div>
        </div>
        <div id="fireball">
            <img src={FireBall} alt="Что-то пошло не так"/>
        </div>
    </div>
}
