import {useEffect, useState} from "react";
import axios from "axios";
import {Link, Navigate} from "react-router-dom";
import Swal from "sweetalert2-neutral";
import {Button, Card, Container, Stack} from "react-bootstrap";
import {createUseStyles} from "react-jss";

const useStyles = createUseStyles({
    noWork: {
        position: "absolute",
        background: "#FFF",
        width: "100%",
        height: "100%",
        top: 0,
        left: 0,
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        fontSize: "1.25em",
        fontWeight: "bold"
    }
});

export function ReportsPage() {
    const [reports, setReports] = useState(null);

    useEffect(() => {
        (async () => {
            try {
                const result = await axios.get("/api/moderator/reports");

                setReports(result.data);
            } catch (error) {
                if (error.status === 401) {
                    await Swal.fire({
                        title: "Вы не вошли в аккаунт",
                        icon: "error",
                        showConfirmButton: false,
                        timer: 2000
                    });

                    return <Navigate to="moderator/login"/>
                } else {
                    await Swal.fire({
                        title: "Что-то пошло не так",
                        icon: "error",
                        showConfirmButton: false,
                        timer: 2000
                    });
                }
            }
        })();
    }, []);

    function removeReport(ctxReport) {
        setReports(reports.filter(report => report.service.serviceId !== ctxReport.service.serviceId))
    }

    if (reports === null) return <span>Загружаем сообщения...</span>

    return <Container>
        <h1>Сообщения</h1>
        {
            reports.length > 0 ? <Stack gap={3} className="mb-3">
                {reports.map(report => <ReportContainer key={report.id} removeReport={removeReport} report={report}/>)}
            </Stack> : <NoWork/>
        }

    </Container>
}

function ReportContainer({report, removeReport}) {
    const date = new Date(report.timestamp + "Z");
    const [loading, setLoading] = useState(false);

    async function handleAccept() {
        return sendChangeStatusOfReport("accept");
    }

    function handleDeny() {
        return sendChangeStatusOfReport("deny");
    }

    async function sendChangeStatusOfReport(statusName) {
        setLoading(true);

        const result = await axios.post(`/api/moderator/reports/${report.id}/${statusName}`)
            .catch(_ => Swal.fire({
                title: "Что-то пошло не так...",
                icon: "error",
                showConfirmButton: false,
                timer: 2000
            }));

        if (result.status === 200) removeReport(report);
    }

    return <Card>
        <Card.Body>
            <Card.Title>
                Сообщение о <b>{!report.isOnline ? "не" : ""}доступности</b> сервиса
                "<Link to={report.service.url}>{report.service.serviceName}</Link>"
                ВУЗа <b>{report.service.universityName}</b>
            </Card.Title>
            <Card.Text>
                {report.content}
            </Card.Text>
            <Stack direction="horizontal" gap={2}>
                <Button disabled={loading} onClick={handleAccept}>Да,
                    сервис {report.isOnline ? "онлайн" : "офлайн"}</Button>
                <Button disabled={loading} onClick={handleDeny}>Нет,
                    сервис {!report.isOnline ? "онлайн" : "офлайн"}</Button>
            </Stack>
        </Card.Body>
        <Card.Footer className="text-muted">
            {formatDate(date)}
        </Card.Footer>
    </Card>
}

function formatDate(date) {
    return `${padTo2Digits(date.getHours())}:${padTo2Digits(date.getMinutes())} 
        ${padTo2Digits(date.getDate())}.${padTo2Digits(date.getMonth() + 1)}.${date.getFullYear()}`
}

function padTo2Digits(num) {
    return num.toString().padStart(2, '0');
}

function NoWork() {
    const style = useStyles();

    return <div className={style.noWork}>
        <span>Никто к нам с жалобами не обращался. Отдыхайте, дорогой модератор ☕</span>
    </div>
}
