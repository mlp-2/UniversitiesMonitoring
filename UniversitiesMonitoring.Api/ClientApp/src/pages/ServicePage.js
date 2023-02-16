import {useLocation} from "react-router-dom";
import {Button} from "../components/Button";
import {createUseStyles} from "react-jss";
import Constants from "../Constants";
import {faStar} from "@fortawesome/free-solid-svg-icons";
import MessagePart from "../assets/images/message-part.svg";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {useEffect, useState} from "react";
import {SubmitButton} from "../components/SubmitButton";
import {GetReports, SendComment, SendReportToService, SubscribeToService, UnsubscribeToService} from "../ApiMethods";
import Swal from "sweetalert2";
import {Stack} from "react-bootstrap";
import {generateUniqueID} from "web-vitals/dist/modules/lib/generateUniqueID";
import {GenerateUUID} from "../Utils";

const useStyles = createUseStyles({
    serviceHeader: {
        background: Constants.brandColor,
        color: "#FFF",
        display: "flex",
        flexDirection: "column",
        height: "25vh",
        paddingLeft: "5vw",
        paddingRight: "5vw",
        paddingTop: "5vh",
        "& .university-name": {
              fontSize: 32
        },
        "& .action-section": {
            display: "flex",
            flexDirection: "row",
            justifyContent: "space-between"
        },
        "& .service-action-buttons": {
            display: "flex",
            flexDirection: "column",
            gap: 10
        },
        "& .service-name-with-status": {
            display: "flex",
            alignItems: ""
        },
        "& .service-name-with-status span": {
            position: "relative",
            fontSize: 64,
            height: "fit-content",
            verticalAlign: "bottom"
        },
        "& .service-name-with-status span::after": {
            content: '""',
            position: "absolute",
            width: 64,
            height: 64,
            right: "-80px", 
            top: "calc(50% - 32px)",
            borderRadius: "50%",
            background: "var(--service-status)"
        }
    },
    serviceBody: {
        background: "#F5F5F5",
        display: "flex",
        flexDirection: "row",
        justifyContent: "space-around",
        alignItems: "baseline",
        "& .title": {
            position: "sticky",
            fontSize: 32,
            top: 10,
            zIndex: 3,
            backdropFilter: "blur(10px)",
            padding: 10,
            borderRadius: "3em",
            textAlign: "center",
            verticalAlign: "middle",
            width: "fit-content"
        },
        "& .title::after": {
            borderRadius: "3em",
            background: "rgba(245,245,245,0.55)",
            position: "absolute",
            content: '""',
            width: "100%",
            height: "100%",
            zIndex: -1,
            left: 0,
            top: 0
        }
    },
    comment: {
        background: "#FFF",
        width: "100%",
        padding: 15 ,
        position: "relative",
        borderRadius: "0 15px 15px 15px",
        "& .comment-author": {
            color: Constants.brandColor,
            fontWeight: "bolder"
        },
        "& .comment-header": {
            display: "flex",
            flexDirection: "row",
            justifyContent: "flex-start",
            gap: 10
        },
        "& .fa-star": {
            color: "#FDD64E"
        },
        "&::before": {
            background: `url(${MessagePart})`,
            content: '""',
            position: "absolute",
            width: 33,
            height: 35,
            left: -33,
            top: -1.0001
        },
        filter: "drop-shadow(0px 0px 24px rgba(0, 0, 0, 0.25))"
    },
    commentsWrapper: {
        "& .comments-container": {
            display: "flex",
            flexDirection: "column",
            gap: 60,
            paddingLeft: 33,
            paddingTop: 33
        },
        padding: 40,
        display: "flex",
        flexDirection: "column",
        gap: 10,
        width: "30%"
    },
    starsBar: {
        background: "#0798EA",
        borderRadius: "15px 15px 0 0",
        padding: 10,
        width: "15vw",
        "& .fa-star": {
            cursor: "pointer",
            width: "15%",
            height: "100%",
            color: "var(--star-color)"
        }
    },
    commentForm: {
        position: "sticky",
        top: 10,
        display: "flex",
        justifyContent: "center",
        flexDirection: "column",
        gap: 10,
        "& textarea": {
            width: "15vw",
            borderRadius: "0 0 15px 15px",
            outline: "none",
            padding: 10,
        }
    }
});

export function ServicePage() {
    const location = useLocation();
    const [service, setService] = useState(location.state.service)
    
    function updateService(service) {  
        setService(service);
    }   
    
    return <div>
        <ServiceHeader service={service} updateService={updateService}/>
        <ServiceBody service={service} updateService={updateService}/>
    </div>
}

function ServiceHeader({service, updateService}) {
    const style = useStyles();
    
    async function handleClickOnSubscribeButton() {
        if (!service.isSubscribed) {
            await SubscribeToService(service.serviceId);
            
        } else {
            await UnsubscribeToService(service.serviceId);
        }
        
        updateService({
            ...service,
            isSubscribed: !service.isSubscribed 
        });
    }
    
    async function handleClickOnReportButton() {
         await Swal.fire({
            title: `Сообщение о ${service.isOnline ? "не" : ""}доступности сервиса`,
            input: "textarea",
            inputPlaceholder: service.isOnline ? "Опишите. Что случилось? Желательно, подробно" :
                "Если Вам есть, что добавить, то пишите сюда",
            showCancelButton: true,
            confirmButtonText: "Отправить",
            cancelButtonText: "Отмена",
            confirmButtonColor: "#0798EA",
            showLoaderOnConfirm: true,
            allowOutsideClick: () => !Swal.isLoading()
        }).then(result => {
            if (!result.isConfirmed) return;
            
            return SendReportToService(service.serviceId, !service.isOnline, result.value)
        }).then(result => {
            if(result) {
                Swal.fire({
                    title: "Успешно. Ожидайте подтверждения Вашего обращения от администрации",
                    icon: "success"
                });
            } else if(result === false) {
                Swal.fire({
                    title: "Что-то пошло не так...",
                    icon: "error"
                });
            }
        });
    }

    async function handleClickOnReportOfflineButton() {
        await Swal.fire({
            title: `Сообщение о недоступности сервиса`,
            input: "textarea",
            inputPlaceholder: "Опишите. Что случилось? Подробно",
            showCancelButton: true,
            confirmButtonText: "Отправить",
            cancelButtonText: "Отмена",
            confirmButtonColor: "#0798EA",
            showLoaderOnConfirm: true,
            allowOutsideClick: () => !Swal.isLoading()
        }).then(result => {
            if (!result.isConfirmed) return;

            return SendReportToService(service.serviceId, false, result.value)
        }).then(result => {
            if(result) {
                Swal.fire({
                    title: `Спасибо большое за указание причины недоступности ${service.serviceName}!`,
                    icon: "success"
                });
            } else if(result === false) {
                Swal.fire({
                    title: "Что-то пошло не так...",
                    icon: "error"
                });
            }
        });
    }
    
    return <div className={style.serviceHeader}>
        <div className="university-name">
            <span>{service.universityName}</span>
        </div>
        <div className="action-section">
            <div className="service-name-with-status" 
                 style={{"--service-status": service.isOnline ? "#3CFB38" : "#FB4438"}}>
                <span className="to-bottom">{service.serviceName}</span>    
            </div>
            <Stack className="flex-grow-0 gap-2">
                <Button onClick={handleClickOnReportButton} 
                        style={{background: "#1FE03C"}}>
                    Сервис {service.isOnline ? "офлайн" : "онлайн"}?
                </Button>
                {
                    !service.isOnline &&
                        <Button onClick={handleClickOnReportOfflineButton}
                                style={{background: "#EDD715"}}>
                            Вы знаете почему сервис не работает?
                        </Button>
                }
                <Button onClick={handleClickOnSubscribeButton} 
                        style={{background: service.isSubscribed ? "#FF4D15" : "#9D9D9D"}}>
                    {service.isSubscribed ? "Отписаться" : "Подписаться"}
                </Button>
            </Stack>
        </div>
    </div>
}

function ServiceBody({service, updateService}) {
    const style = useStyles();
    
    return <div className={style.serviceBody}>
        <CommentsColumn comments={service.comments}/>
        {!service.isOnline && <ReportsColumn service={service}/>}
        <SendCommentForm service={service} updateService={updateService}/>
    </div>;
}

function CommentsColumn({comments}) {
    const style = useStyles();
    
    return <div className={style.commentsWrapper}>
        <span className="title">Комментарии</span>
        <div className="comments-container">
            {comments.map(comment => 
                <Comment key={comment.id} from={comment.author.username} content={comment.content} stars={comment.rate}/>)}
        </div>
    </div>
}

function ReportsColumn({service}) {
    const [reports, setReports] = useState(null);
    const style = useStyles();
    
    useEffect(() => {
        (async () => {
            setReports(await GetReports(service.serviceId));
        })();
    }, []);
    
    if(reports === null)
    {
        return <span>ЗАГРУЗКА</span>
    }
    
    return <div className={style.commentsWrapper}>
        <span className="title">Сообщения о текущем сбое</span>
        {
            <div className="comments-container">
                {reports.length > 0 ? reports.map(report =>
                    <Comment key={report.id} from="Пользователя сервиса" content={report.content}/>) :  
                    <span>Пока что нет никаких данных об этом сбое. Приходите сюда позже 👀</span>}
            </div>
        }
    </div>
}

function SendCommentForm({service, updateService}) {
    const [rate, setRate] = useState();
    const style = useStyles();
    
    async function handleSubmit(e) {
        e.preventDefault();
        
        const form = e.target;
        const formData = new FormData(form);
        formData.append("rate", rate);

        const apiData = Object.fromEntries(formData.entries());
        
        if (await SendComment(service.serviceId, apiData)) {
            await Swal.fire({
                title: "Комментарий добавлен",
                icon: "success",
                timer: 1000,
                showConfirmButton: false
            }); 
            
            service.comments.push({
                id: GenerateUUID(),
                author: {
                    username: "Вас"  
                },
                ...apiData
            });
            
            updateService({
                ...service,
                comments: service.comments,
            });
        } else {
            await Swal.fire({
                title: "Что-то пошло не так...",
                icon: "error",
                timer: 1000,
                showConfirmButton: false
            });
        }
    }
    
    return <form className={style.commentForm} method="post" onSubmit={handleSubmit}>
            <div>
                <StarsBar onChange={(count) => setRate(count)}/>
                <textarea placeholder="Оставьте Ваше мнение о сервисе здесь" name="content" rows={10} cols={1}/>
            </div>
            <SubmitButton content="Отправить"/> 
        </form>;
}

function StarsBar({onChange}) {
    const style = useStyles();
    const [countOfStars, setCountOfStars] = useState(-1);
    
    useEffect(() => {
        onChange(countOfStars);
    }, [countOfStars])
    
    return <div className={style.starsBar}>
        <RateStar index={1} rate={countOfStars} onClick={() => setCountOfStars(1)}/>
        <RateStar index={2} rate={countOfStars} onClick={() => setCountOfStars(2)}/>
        <RateStar index={3} rate={countOfStars} onClick={() => setCountOfStars(3)}/>
        <RateStar index={4} rate={countOfStars} onClick={() => setCountOfStars(4)}/>
        <RateStar index={5} rate={countOfStars} onClick={() => setCountOfStars(5)}/>
    </div>;
}

function RateStar({index, rate, onClick}) {
    return <FontAwesomeIcon onClick={onClick} icon={faStar} 
                            style={{"--star-color": index <= rate ? "#FDD64E" : "#046298"}}/>
}

function Comment({from, content, stars = -1}) {
    const style = useStyles();
    
    return <div className={style.comment}>  
        <div className="comment-header">
            <span>от <span className="comment-author">{from}</span></span>
            {
                stars > 0 &&
                    <div>{[...Array(stars).keys()].map(_ => <FontAwesomeIcon icon={faStar}/>)}</div> 
            }
        </div>
        <div className="comment-content">
            <p>{content}</p>
        </div>
    </div>
}

function getServiceReports(serviceId) {
    return [
        {
            id: 1,
            content: "Технические работы",
            isOnline: false,
            serviceId: 1
        },
        {
            id: 2,
            content: "Сайт не открывается!!!!!",
            isOnline: false,
            serviceId: 1
        }
    ];
}
