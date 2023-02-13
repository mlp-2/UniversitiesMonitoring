import {useEffect, useState} from "react";
import {Button} from "../components/Button";
import {createUseStyles} from "react-jss";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faComment, faStar} from "@fortawesome/free-solid-svg-icons";
import Constants from "../Constants";
import {Link} from "react-router-dom";

const useStyles = createUseStyles({
    universityHeader: {
       background: Constants.brandColor,
       color: "#FFF",
       display: "flex",
       flexDirection: "row",
       alignItems: "center",
       justifyContent: "space-around",
       "& span": {
           fontSize: 64,
           fontWeight: "bolder"
       },
       height: "25vh"
    },
    servicePanel: {
        display: "flex",
        justifyContent: "space-between",
        alignItems: "center",
        "& .service-actions": {
            display: "flex",
            flexDirection: "row",
            alignItems: "center",
            gap: 20
        },  
        width: "80%",
        background: "#FFF",
        padding: 30,
        boxShadow: "0px 4px 16px 4px rgba(0, 0, 0, 0.25)",
        borderRadius: 15,
        fontSize: 20,
        userSelect: "none",
        "& .service-status": {
            width: 50,
            height: 50,
            borderRadius: "50%",
            background: "var(--service-status)"
        },
        "& a": {
            fontSize: 32,
            fontWeight: "bolder",
            color: "#000"
        },
        "& .additional-info": {
            display: "flex",
            flexDirection: "row",
            alignItems: "center",
            gap: 2,
            color: "#878787"
        }
    },
    servicesWrapper: {
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        gap: 30
    },
    servicesBlock: {
        background: "#F5F5F5",
        display: "flex",
        flexDirection: "column",
        "& .services-title": {
            textAlign: "center",
            fontSize: 48,
            fontWeight: "bold",
            padding: 20
        },
        height: "75vh",
        overflowX: "hidden",
        overflowY: "auto"
    }
});

export function UniversityPage() {
    const universityId = getUniversityId();
    
    return <div>
        <UniversityHeader universityId={universityId}/>
        <ServicesList universityId={universityId}/>
    </div>
}

function UniversityHeader({universityId}) {
    const style = useStyles();
    const [university, setUniversity] = useState(null);

    useEffect(() => {
        setUniversity(getUniversity(universityId));
    }, []);
    
    if(university === null) return <span>ЗАГРУЗКА</span>;
    
    return <div className={style.universityHeader}>
        <span>{university.name}</span>
        <Button style={{background: "#FF4D15"}}>Подписаться</Button>
    </div>    
} 

function ServicesList({universityId}) {
    const style = useStyles();
    const [services, setServices] = useState(null);
    
    useEffect(() => {
        setServices(getUniversityServices());
    }, []) //TODO: Добавить обработку обновления по WS
    
    if(services === null) return <span>ЗАГРУЗКА</span>;
    
    return <div className={style.servicesBlock}>
        <span className="services-title">Сервисы</span>
        <div className={style.servicesWrapper}>
            {services.map(service => <ServiceContainer key={service.serviceId} service={service}/>)}
        </div>
    </div>
}

function ServiceContainer({service}) {
    const style = useStyles();
    
    let rateAvg = 0;
    
    for (let i in service.comments) {
        rateAvg += service.comments[i].rate;
    }
    
    rateAvg /= service.comments.length;
    
    return <div className={style.servicePanel}>
            <Link to="/service" state={{ service: service }}>
                {service.serviceName}
            </Link>
            <div className="service-actions">
                <Button style={{background: "#9D9D9D"}}>Подписаться</Button>
                <div className="additional-info">
                    <FontAwesomeIcon icon={faComment}/>
                    <span>{service.comments.length}</span>
                </div>
                <div className="additional-info">
                    <FontAwesomeIcon icon={faStar}/>
                    <span>{rateAvg}/5</span>
                </div>
                <div className="service-status"
                     style={{"--service-status": service.isOnline ? "#3CFB38" : "#FB4438" }}/>
            </div>
        </div>
}

function getUniversityId() {
    return 0;
}

function getUniversity() {
    return {
        name: "ВШЭ"
    };
}

function getUniversityServices() {
    return [
        {
            serviceId: 1,
            serviceName: "ВШЭ Мобильный",
            universityName: "ВШЭ",
            isOnline: true,
            comments: [
                {
                    id: 1,
                    rate: 5,
                    comment: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas at sapien ut ligula ullamcorper pharetra. Phasellus tempus volutpat tellus a dictum. Cras vel facilisis sapien. Quisque mollis varius enim, eu euismod lacus pellentesque at. Proin vulputate feugiat neque, volutpat ullamcorper elit luctus et. Nullam est metus, pulvinar id euismod vel, laoreet sed tortor. Duis enim arcu, dictum volutpat ipsum vel, bibendum consequat est.",
                    author: {
                        username: "DenVot"
                    }
                },
                {
                    id: 2,
                    rate: 4,
                    comment: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas at sapien ut ligula ullamcorper pharetra. Phasellus tempus volutpat tellus a dictum. Cras vel facilisis sapien. Quisque mollis varius enim, eu euismod lacus pellentesque at. Proin vulputate feugiat neque, volutpat ullamcorper elit luctus et. Nullam est metus, pulvinar id euismod vel, laoreet sed tortor. Duis enim arcu, dictum volutpat ipsum vel, bibendum consequat est.",
                    author: {
                        username: "Prowx"
                    }
                }
            ]
        },
        {
            serviceId: 2,
            serviceName: "ВШЭ сайт",
            universityName: "ВШЭ",
            isOnline: false,
            comments: [
                {
                    id: 3,
                    rate: 5,
                    comment: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas at sapien ut ligula ullamcorper pharetra. Phasellus tempus volutpat tellus a dictum. Cras vel facilisis sapien. Quisque mollis varius enim, eu euismod lacus pellentesque at. Proin vulputate feugiat neque, volutpat ullamcorper elit luctus et. Nullam est metus, pulvinar id euismod vel, laoreet sed tortor. Duis enim arcu, dictum volutpat ipsum vel, bibendum consequat est.",
                    author: {
                        username: "DenVot"
                    }
                },
                {
                    id: 4,
                    rate: 4,
                    comment: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas at sapien ut ligula ullamcorper pharetra. Phasellus tempus volutpat tellus a dictum. Cras vel facilisis sapien. Quisque mollis varius enim, eu euismod lacus pellentesque at. Proin vulputate feugiat neque, volutpat ullamcorper elit luctus et. Nullam est metus, pulvinar id euismod vel, laoreet sed tortor. Duis enim arcu, dictum volutpat ipsum vel, bibendum consequat est.",
                    author: {
                        username: "Prowx"
                    }
                }
            ]
        },
        {
            serviceId: 3,
            serviceName: "ВШЭ сайт",
            universityName: "ВШЭ",
            isOnline: false,
            comments: [
                {
                    id: 5,
                    rate: 5,
                    comment: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas at sapien ut ligula ullamcorper pharetra. Phasellus tempus volutpat tellus a dictum. Cras vel facilisis sapien. Quisque mollis varius enim, eu euismod lacus pellentesque at. Proin vulputate feugiat neque, volutpat ullamcorper elit luctus et. Nullam est metus, pulvinar id euismod vel, laoreet sed tortor. Duis enim arcu, dictum volutpat ipsum vel, bibendum consequat est.",
                    author: {
                        username: "DenVot"
                    }
                },
                {
                    id: 6,
                    rate: 4,
                    comment: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas at sapien ut ligula ullamcorper pharetra. Phasellus tempus volutpat tellus a dictum. Cras vel facilisis sapien. Quisque mollis varius enim, eu euismod lacus pellentesque at. Proin vulputate feugiat neque, volutpat ullamcorper elit luctus et. Nullam est metus, pulvinar id euismod vel, laoreet sed tortor. Duis enim arcu, dictum volutpat ipsum vel, bibendum consequat est.",
                    author: {
                        username: "Prowx"
                    }
                }
            ]
        },
        {
            serviceId: 4,
            serviceName: "ВШЭ сайт",
            universityName: "ВШЭ",
            isOnline: false,
            comments: [
                {
                    id: 7,
                    rate: 5,
                    comment: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas at sapien ut ligula ullamcorper pharetra. Phasellus tempus volutpat tellus a dictum. Cras vel facilisis sapien. Quisque mollis varius enim, eu euismod lacus pellentesque at. Proin vulputate feugiat neque, volutpat ullamcorper elit luctus et. Nullam est metus, pulvinar id euismod vel, laoreet sed tortor. Duis enim arcu, dictum volutpat ipsum vel, bibendum consequat est.",
                    author: {
                        username: "DenVot"
                    }
                },
                {
                    id: 8,
                    rate: 4,
                    comment: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas at sapien ut ligula ullamcorper pharetra. Phasellus tempus volutpat tellus a dictum. Cras vel facilisis sapien. Quisque mollis varius enim, eu euismod lacus pellentesque at. Proin vulputate feugiat neque, volutpat ullamcorper elit luctus et. Nullam est metus, pulvinar id euismod vel, laoreet sed tortor. Duis enim arcu, dictum volutpat ipsum vel, bibendum consequat est.",
                    author: {
                        username: "Prowx"
                    }
                }
            ]
        },
        {
            serviceId: 5,
            serviceName: "ВШЭ сайт",
            universityName: "ВШЭ",
            isOnline: false,
            comments: [
                {
                    id: 9,
                    rate: 5,
                    comment: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas at sapien ut ligula ullamcorper pharetra. Phasellus tempus volutpat tellus a dictum. Cras vel facilisis sapien. Quisque mollis varius enim, eu euismod lacus pellentesque at. Proin vulputate feugiat neque, volutpat ullamcorper elit luctus et. Nullam est metus, pulvinar id euismod vel, laoreet sed tortor. Duis enim arcu, dictum volutpat ipsum vel, bibendum consequat est.",
                    author: {
                        username: "DenVot"
                    }
                },
                {
                    id: 10,
                    rate: 4,
                    comment: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas at sapien ut ligula ullamcorper pharetra. Phasellus tempus volutpat tellus a dictum. Cras vel facilisis sapien. Quisque mollis varius enim, eu euismod lacus pellentesque at. Proin vulputate feugiat neque, volutpat ullamcorper elit luctus et. Nullam est metus, pulvinar id euismod vel, laoreet sed tortor. Duis enim arcu, dictum volutpat ipsum vel, bibendum consequat est.",
                    author: {
                        username: "Prowx"
                    }
                }
            ]
        },
        {
            serviceId: 6,
            serviceName: "ВШЭ сайт",
            universityName: "ВШЭ",
            isOnline: false,
            comments: [
                {
                    id: 11,
                    rate: 5,
                    comment: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas at sapien ut ligula ullamcorper pharetra. Phasellus tempus volutpat tellus a dictum. Cras vel facilisis sapien. Quisque mollis varius enim, eu euismod lacus pellentesque at. Proin vulputate feugiat neque, volutpat ullamcorper elit luctus et. Nullam est metus, pulvinar id euismod vel, laoreet sed tortor. Duis enim arcu, dictum volutpat ipsum vel, bibendum consequat est.",
                    author: {
                        username: "DenVot"
                    }
                },
                {
                    id: 12,
                    rate: 4,
                    comment: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas at sapien ut ligula ullamcorper pharetra. Phasellus tempus volutpat tellus a dictum. Cras vel facilisis sapien. Quisque mollis varius enim, eu euismod lacus pellentesque at. Proin vulputate feugiat neque, volutpat ullamcorper elit luctus et. Nullam est metus, pulvinar id euismod vel, laoreet sed tortor. Duis enim arcu, dictum volutpat ipsum vel, bibendum consequat est.",
                    author: {
                        username: "Prowx"
                    }
                }
            ]
        },
        {
            serviceId: 7,
            serviceName: "ВШЭ сайт",
            universityName: "ВШЭ",
            isOnline: false,
            comments: [
                {
                    id: 13,
                    rate: 5,
                    comment: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas at sapien ut ligula ullamcorper pharetra. Phasellus tempus volutpat tellus a dictum. Cras vel facilisis sapien. Quisque mollis varius enim, eu euismod lacus pellentesque at. Proin vulputate feugiat neque, volutpat ullamcorper elit luctus et. Nullam est metus, pulvinar id euismod vel, laoreet sed tortor. Duis enim arcu, dictum volutpat ipsum vel, bibendum consequat est.",
                    author: {
                        username: "DenVot"
                    }
                },
                {
                    id: 14,
                    rate: 4,
                    comment: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas at sapien ut ligula ullamcorper pharetra. Phasellus tempus volutpat tellus a dictum. Cras vel facilisis sapien. Quisque mollis varius enim, eu euismod lacus pellentesque at. Proin vulputate feugiat neque, volutpat ullamcorper elit luctus et. Nullam est metus, pulvinar id euismod vel, laoreet sed tortor. Duis enim arcu, dictum volutpat ipsum vel, bibendum consequat est.",
                    author: {
                        username: "Prowx"
                    }
                }
            ]
        },
        {
            serviceId: 8,
            serviceName: "ВШЭ сайт",
            universityName: "ВШЭ",
            isOnline: false,
            comments: [
                {
                    id: 15,
                    rate: 5,
                    comment: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas at sapien ut ligula ullamcorper pharetra. Phasellus tempus volutpat tellus a dictum. Cras vel facilisis sapien. Quisque mollis varius enim, eu euismod lacus pellentesque at. Proin vulputate feugiat neque, volutpat ullamcorper elit luctus et. Nullam est metus, pulvinar id euismod vel, laoreet sed tortor. Duis enim arcu, dictum volutpat ipsum vel, bibendum consequat est.",
                    author: {
                        username: "DenVot"
                    }
                },
                {
                    id: 16,
                    rate: 4,
                    comment: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas at sapien ut ligula ullamcorper pharetra. Phasellus tempus volutpat tellus a dictum. Cras vel facilisis sapien. Quisque mollis varius enim, eu euismod lacus pellentesque at. Proin vulputate feugiat neque, volutpat ullamcorper elit luctus et. Nullam est metus, pulvinar id euismod vel, laoreet sed tortor. Duis enim arcu, dictum volutpat ipsum vel, bibendum consequat est.",
                    author: {
                        username: "Prowx"
                    }
                }
            ]
        },
    ];
}
