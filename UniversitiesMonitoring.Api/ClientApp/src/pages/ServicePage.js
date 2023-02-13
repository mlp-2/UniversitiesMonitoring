import {useLocation} from "react-router-dom";
import {Button} from "../components/Button";
import {createUseStyles} from "react-jss";
import Constants from "../Constants";
import {faStar} from "@fortawesome/free-solid-svg-icons";
import MessagePart from "../assets/images/message-part.svg";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {useEffect, useRef, useState} from "react";
import {SubmitButton} from "../components/SubmitButton";

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
            position: "relative",
            fontSize: 64,
            height: "fit-content"
        },
        "& .service-name-with-status::after": {
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
        justifyContent: "space-around"
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
            gap: 50,
            paddingLeft: 33,
            paddingTop: 33
        },
        "& .comments-title": {
            fontSize: 32,
        },
        padding: 40,
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
        display: "flex",
        justifyContent: "center",
        flexDirection: "column",
        gap: 10,
        "& textarea": {
            width: "15vw",
            borderRadius: "0 0 15px 15px",
            outline: "none",
            padding: 10
        }
    }
});

export function ServicePage() {
    const location = useLocation();
    const {service} = location.state;
    
    return <div>
        <ServiceHeader service={service}/>
        <ServiceBody service={service}/>
    </div>
}

function ServiceHeader({service}) {
    const style = useStyles();
    
    return <div className={style.serviceHeader}>
        <div className="university-name">
            <span>{service.universityName}</span>
        </div>
        <div className="action-section">
            <div className="service-name-with-status" style={{"--service-status": "#3CFB38"}}>
                <span>{service.serviceName}</span>    
            </div>
            <div className="service-action-buttons">
                <Button style={{background: "#00EE75"}}>Сервис офлайн?</Button>
                <Button style={{background: "#FF4D15"}}>Подписаться</Button>
            </div>
        </div>
    </div>
}

function ServiceBody({service}) {
    const style = useStyles();
    
    return <div className={style.serviceBody}>
        <CommentsColumn comments={service.comments}/>
        {service.isOnline && <ReportsColumn/>}
        <SendCommentForm/>
    </div>;
}

function CommentsColumn({comments}) {
    const style = useStyles();
    
    return <div className={style.commentsWrapper}>
        <span className="comments-title">Комментарии</span>
        <div className="comments-container">
            {comments.map(comment => 
                <Comment key={comment.id} from={comment.author.username} content={comment.comment} stars={comment.rate}/>)}
        </div>
    </div>
}

function ReportsColumn() {
    const [reports, setReports] = useState(null);
    const style = useStyles();
    
    useEffect(() => {
        setReports(getServiceReports());
    }, []);
    
    if(reports === null)
    {
        return <span>ЗАГРУЗКА</span>
    }
    
    return <div className={style.commentsWrapper}>
        <span className="comments-title">Сообщения о текущем сбое</span>
        <div className="comments-container">
            {reports.map(report =>
                <Comment key={report.id} from="Пользователя сервиса" content={report.content}/>)}
        </div>
    </div>
}

function SendCommentForm() {
    const [rate, setRate] = useState();
    const style = useStyles();
    
    function handleSubmit(e) {
        e.preventDefault();
        
        const form = e.target;
        const formData = new FormData(form);
        formData.append("rate", rate);

        const formJson = Object.fromEntries(formData.entries());
        console.log(formJson);
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
