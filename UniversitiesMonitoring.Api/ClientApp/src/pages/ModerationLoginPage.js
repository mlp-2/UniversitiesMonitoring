import React, {useState} from "react";
import {Button, Form, Container} from "react-bootstrap";
import axios from "axios";
import Swal from "sweetalert2";
import {Navigate} from "react-router-dom";

export function ModerationLoginPage() {
    const [passedAuth, setPassed] = useState(false);

    async function handleSubmitForm(e) {
        e.preventDefault();

        const form = e.target;
        const formData = new FormData(form);
        const apiEntity = Object.fromEntries(formData.entries());

        if (apiEntity.id === "" || apiEntity.password === "") return;

        try {
            const response = await axios.post("/api/moderator/auth", apiEntity);
            localStorage.setItem("modToken", response.data.jwt);
            setPassed(true);
        } catch (error) {
            await Swal.fire({
                title: "Неверный логин или пароль",
                icon: "error",
                showConfirmButton: false,
                timer: 2000
            })
        }
    }

    if (passedAuth) return <Navigate to="/moderator"/>

    return <Container>
        <h1>Панель модератора</h1>
        <Form onSubmit={handleSubmitForm}>
            <Form.Group className="mb-3">
                <Form.Control name="id" type="text" placeholder="Введите Ваш ID"/>
                <Form.Text className="text-muted">
                    Если у Вас нет ID, то обратитесь к администратору
                </Form.Text>
            </Form.Group>
            <Form.Group className="mb-3">
                <Form.Control name="password" type="password" placeholder="Введите Ваш пароль"/>
            </Form.Group>
            <Button type="submit">
                Вход
            </Button>
        </Form>
    </Container>
}