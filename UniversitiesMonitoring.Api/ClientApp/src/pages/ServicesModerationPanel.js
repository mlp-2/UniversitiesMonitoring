import {useLocation} from "react-router-dom";
import {useEffect, useState} from "react";
import axios from "axios";
import {Button, Card, Stack} from "react-bootstrap";
import Swal from "sweetalert2-neutral";

export function ServicesModerationPanel() {
    const university = useLocation().state.university;
    const [services, setServices] = useState(null);

    function handleDelete(id) {
        setServices(services.filter(service => service.serviceId !== id));
    }

    async function handleAddService() {
        await Swal.fire({
            title: "Добавление сервиса",
            html: `
                <input type="text" placeholder="Название сервиса" name="name" id="name"/>
                <input type="text" placeholder="URL" name="url" id="url"/>
            `,
            confirmButtonText: "Добавить",
            confirmButtonColor: "#1577ff",
            preConfirm: () => {
                const name = Swal.getPopup().querySelector('#name').value
                const url = Swal.getPopup().querySelector('#url').value
                if (!name || !url) {
                    Swal.showValidationMessage(`Введите все данные`)
                }
                return {name: name, url: url}
            }
        }).then(async result => {
            if (!result.isConfirmed) return;

            const apiEntity = {
                universityId: university.id,
                name: result.value.name,
                url: result.value.url
            }

            const responseResult = await axios.post(`/api/moderator/services/add`, apiEntity);

            if (responseResult.status === 200) {
                setServices([
                    {
                        serviceName: result.value.name,
                        serviceId: responseResult.data
                    },
                    ...services
                ])
            }
        });
    }

    useEffect(() => {
        (async () => {
            const result = await axios.get(
                `/api/services?universityId=${university.id}&loadComments=false&loadUsers=false`);

            if (result.status === 200) {
                setServices(result.data);
            }
        })();
    }, []);

    if (services === null) return <span>Загрузка...</span>

    return <div className="p-3">
        <h1>Сервисы {university.name}</h1>
        <Button className="mb-3" onClick={handleAddService}>
            Добавить
        </Button>
        <Stack gap={3}>
            {services.map(service => <ServiceContainer
                removeService={handleDelete}
                service={service}
                updateServiceName={(newName) => {
                    service.serviceName = newName;

                    console.log(services);

                    setServices([...services]);
                }}
                updateServiceUrl={(newUrl) => {
                    service.url = newUrl;

                    console.log(services);

                    setServices([...services]);
                }}/>)}
        </Stack>
    </div>
}

function ServiceContainer({service, removeService, updateServiceName, updateServiceUrl}) {
    async function handleDelete() {
        await Swal.fire({
            title: "Вы уверены?",
            icon: "question",
            showDenyButton: true,
            confirmButtonText: "Да",
            denyButtonText: "Нет",
            confirmButtonColor: "#1577ff",
            showLoaderOnConfirm: true
        }).then(async result => {
            if (!result.isConfirmed) {
                return;
            }

            const resultOfRequest = await axios.delete(`/api/moderator/services/${service.serviceId}/delete`);

            if (resultOfRequest.status === 200) {
                await Swal.fire({
                    title: "Успешно",
                    icon: "success",
                    showConfirmButton: false,
                    timer: 2000
                });

                removeService(service.serviceId);
            }
        })
    }

    async function handleRename() {
        await Swal.fire({
            title: "Изменение названия сервиса",
            input: "text",
            inputPlaceholder: service.serviceName,
            confirmButtonText: "Изменить",
            confirmButtonColor: "#1577ff"
        }).then(async result => {
            if (!result.isConfirmed || result.value === "") return;

            const responseResult = await axios.put(`/api/moderator/services/${service.serviceId}/rename?newName=${result.value}`)

            if (responseResult.status === 200) {
                await Swal.fire({
                    title: "Успешно",
                    icon: "success",
                    showConfirmButton: false,
                    timer: 2000
                });
                updateServiceName(result.value);
            }
        });
    }

    async function handleChangeUrl() {
        await Swal.fire({
            title: "Изменение URL сервиса",
            input: "text",
            inputPlaceholder: service.url,
            confirmButtonText: "Изменить",
            confirmButtonColor: "#1577ff"
        }).then(async result => {
            if (!result.isConfirmed || result.value === "") return;

            const responseResult = await axios.put(`/api/moderator/services/${service.serviceId}/change-url?newUrl=${result.value}`)

            if (responseResult.status === 200) {
                await Swal.fire({
                    title: "Успешно",
                    icon: "success",
                    showConfirmButton: false,
                    timer: 2000
                });
                updateServiceUrl(result.value);
            }
        });
    }

    return <Card className="p-3">
        <Card.Title>
            {service.serviceName}
        </Card.Title>
        <Stack direction="horizontal" gap={2}>
            <Button onClick={handleDelete} variant="danger">
                Удалить
            </Button>
            <Button onClick={handleRename}>
                Переименовать
            </Button>
            <Button onClick={handleChangeUrl}>
                Поменять Url
            </Button>
        </Stack>
    </Card>
}