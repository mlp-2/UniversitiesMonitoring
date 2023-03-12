import {Link, useLocation} from "react-router-dom";
import {useEffect, useState} from "react";
import axios from "axios";
import {Button, Card, Stack} from "react-bootstrap";
import Swal from "sweetalert2";

export function UniversitiesModerationPanel() {
    const [universities, setUniversities] = useState(null);

    function handleDelete(id) {
        setUniversities(universities.filter(university => university.id !== id));
    }

    async function handleAddService() {
        await Swal.fire({
            title: "Добавление университета",
            input: "text",
            confirmButtonText: "Добавить",
            confirmButtonColor: "#1577ff",
        }).then(async result => {
            if (!result.isConfirmed) return;

            const responseResult = await axios.post(`/api/moderator/universities/add?name=${result.value}`);

            if (responseResult.status === 200) {
                setUniversities([
                    {
                        name: result.value,
                        id: responseResult.data
                    },
                    ...universities
                ])
            }
        });
    }

    useEffect(() => {
        (async () => {
            const result = await axios.get(`/api/services/universities`);

            if (result.status === 200) {
                setUniversities(result.data);
            }
        })();
    }, []);

    if (universities === null) return <span>Загрузка...</span>

    return <div className="p-3">
        <h1>Университеты</h1>
        <Button className="mb-3" onClick={handleAddService}>
            Добавить
        </Button>
        <Stack gap={3}>
            {
                universities.map(university => <UniversityContainer
                    removeUniversity={handleDelete} university={university}
                    updateUniversityName={(universityName) => {
                        university.name = universityName;
                        setUniversities([...universities]);
                    }}/>)
            }
        </Stack>
    </div>
}

function UniversityContainer({university, removeUniversity, updateUniversityName}) {
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

            const resultOfRequest = await axios.delete(`/api/moderator/universities/${university.id}/delete`);

            if (resultOfRequest.status === 200) {
                await Swal.fire({
                    title: "Успешно",
                    icon: "success",
                    showConfirmButton: false,
                    timer: 2000
                });

                removeUniversity(university.id);
            }
        })
    }

    async function handleRename() {
        await Swal.fire({
            title: "Изменение названия ВУЗа",
            input: "text",
            inputPlaceholder: university.name,
            confirmButtonText: "Изменить",
            confirmButtonColor: "#1577ff"
        }).then(async result => {
            if (!result.isConfirmed || result.value === "") return;

            const responseResult = await axios.put(`/api/moderator/universities/${university.id}/rename?newName=${result.value}`)

            if (responseResult.status === 200) {
                await Swal.fire({
                    title: "Успешно",
                    icon: "success",
                    showConfirmButton: false,
                    timer: 2000
                });
                updateUniversityName(result.value);
            }
        });
    }

    return <Card className="p-3">
        <Card.Title>
            <Link to="/moderator/university" state={{
                university: university
            }}>
                {university.name}
            </Link>
        </Card.Title>
        <Stack direction="horizontal" gap={2}>
            <Button onClick={handleDelete} variant="danger">
                Удалить
            </Button>
            <Button onClick={handleRename}>
                Переименовать
            </Button>
        </Stack>
    </Card>
}