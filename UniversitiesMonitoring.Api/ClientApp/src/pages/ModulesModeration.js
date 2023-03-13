import {Button, Card, Container, Stack} from "react-bootstrap";
import {useEffect, useState} from "react";
import axios from "axios";
import Swal from "sweetalert2-neutral";
import DefaultConstants from "../Constants";

export function ModulesModeration() {
    return <Container>
        <h1>Управление модулями</h1>
        <ModulesActions/>
    </Container>
}

function ModulesActions() {
    const [modules, setModules] = useState(null);

    async function handleAdd() {
        try {
            const dialogResult = await Swal.fire({
                title: "Создание модуля",
                input: "text",
                confirmButtonText: "Создать",
                confirmButtonColor: DefaultConstants.brandColor,
                showCancelButton: true,
                cancelButtonText: "Отмена"
            })

            if (!dialogResult.isConfirmed) return;

            const result = await axios.post(`/api/moderator/modules?url=${dialogResult.value}`);

            setModules([result.data, ...modules]);
        } catch {
            await Swal.fire({
                customClass: "text-lg-start",
                title: "Некорректный модуль",
                html: `
                <div class="text-lg-start">
                     <b>Возможные причины:</b>
                    <ul>
                        <li>Некорректная ссылка</li>                       
                        <li>Модуль недоступен</li>
                        <li>Модуль с такой ссылкой уже существует</li>
                        <li>Модуль не соответствует требованиям. Переустановите его, <a href="https://hub.docker.com/repository/docker/denvot/um-module/general">скачав актуальную версию контейнера</a></li>                  
                    </ul>
                </div>             
                `
            })
        }
    }

    useEffect(() => {
        (async () => {
            const result = await axios.get(`/api/moderator/modules`);

            setModules(result.data);
        })();
    }, []);

    if (modules === null) return <span>ЗАГРУЗКА МОДУЛЕЙ...</span>

    return <>
        <Button onClick={handleAdd} className="mb-3">Добавить</Button>
        <Stack gap={3}>
            {modules.map(module => <ModuleContainer module={module}
                                                    deleteModule={() => {
                                                        setModules(modules.filter(mod => mod.id !== module.id))
                                                    }}/>)}
        </Stack>
    </>
}

function ModuleContainer({module, deleteModule}) {
    async function handleDelete() {
        try {
            const dialogResult = await Swal.fire({
                title: "Вы уверены?",
                icon: "question",
                confirmButtonText: "Да",
                confirmButtonColor: DefaultConstants.brandColor,
                showCancelButton: true,
                cancelButtonText: "Нет"
            })

            if (!dialogResult.isConfirmed) return;

            const res = await axios.delete(`/api/moderator/modules/${module.id}`);

            if (res.status === 200) {
                await Swal.fire({
                    title: "Успешно",
                    icon: "success",
                    showConfirmButton: false,
                    timer: 2000
                })
                deleteModule();
            }
        } catch {
            await Swal.fire({
                title: "Что-то пошло не так",
                icon: "error",
                timer: 2000,
                showConfirmButton: false
            })
        }
    }

    return <Card>
        <Card.Body>
            <Card.Title>
                {module.locationName ?? "Локация не определена"} ({module.url})
            </Card.Title>
            <Stack direction="horizontal" gap={2}>
                <Button onClick={handleDelete} variant="danger">Удалить</Button>
            </Stack>
        </Card.Body>
    </Card>;
}