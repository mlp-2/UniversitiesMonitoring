import {useEffect, useState} from "react";
import {createUseStyles} from "react-jss";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faComment, faStar} from "@fortawesome/free-solid-svg-icons";
import Constants from "../Constants";
import {Link, useLocation} from "react-router-dom";
import axios from "axios";
import {Loading} from "../components/Loading";
import {SubscribeToService, UnsubscribeToService} from "../ApiMethods";
import {Button} from "react-bootstrap";

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
        fontSize: 16,
        userSelect: "none",
        "& .service-status": {
            width: 35,
            height: 35,
            borderRadius: "50%",
            background: "var(--service-status)"
        },
        "& a": {
            fontSize: 25,
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
    },
    "@media screen and (max-width: 1000px)": {
        servicePanel: {
            alignItems: "flex-start",
            flexDirection: "column",
            width: "80%",
            "& a": {
                textAlign: "center"
            },
            "& .service-actions": {
                flexWrap: "wrap"
            },
            "& .service-status": {
                display: "none"
            },
            borderRight: "15px solid var(--service-status)"
        }
    }
});

export function UniversityPage() {
    const location = useLocation();
    const [university, setUniversity] = useState(location.state.university);
    const [services, setServices] = useState(null);
    const [isSubscribed, setSubscribed] = useState(false);
    
    function updateServices(servicesToChange) {
        setServices([...(servicesToChange ?? services)]);
    }
    
    useEffect(() => {
        (async () => {
            setServices(await getUniversityServices(university.id));
        })();
    }, []);
    
    let isSub = true;
    
    for (let i in services) {
        isSub &= services[i].isSubscribed;
    }

    if (isSubscribed !== isSub) setSubscribed(isSub);
    
    return <div>
        <UniversityHeader university={university} services={services} isSubscribed={isSubscribed} updateServices={updateServices}/>
        <ServicesList services={services} updateServices={updateServices}/>
    </div>
}

function UniversityHeader({university, services, updateServices, isSubscribed}) {
    const style = useStyles();
    
    async function handleClickOnSubscribeButton() {
        for (let i in services) {
            if (isSubscribed) { // Отписываемся от всего
                await UnsubscribeToService(services[i].serviceId);
                services[i].isSubscribed = false;
            } else {
                await SubscribeToService(services[i].serviceId);
                services[i].isSubscribed = true;
            }
        }
        
        updateServices(services);
    }
    
    return <div className={style.universityHeader}>
        <span>{university.name}</span>
        <Button onClick={handleClickOnSubscribeButton} variant={isSubscribed ? "secondary" : "danger"}>
            {isSubscribed ? "Отписаться" : "Подписаться"}
        </Button>
    </div>    
} 

function ServicesList({services, updateServices}) {
    const style = useStyles();
    
    if (services === null) return <Loading/>
    
    return <div className={style.servicesBlock}>
        <span className="services-title">Сервисы</span>
        <div className={style.servicesWrapper}>
            {services.map(service => <ServiceContainer updateServices={updateServices} key={service.serviceId} service={service}/>)}
        </div>
    </div>
}

function ServiceContainer({service, updateServices}) {
    const style = useStyles();
    
    async function handleClickSubscribe() {
        service.isSubscribed = !service.isSubscribed;
        
        if (service.isSubscribed) {
            await axios.post(`/api/services/${service.serviceId}/subscribe`);
        } else {
            await axios.delete(`/api/services/${service.serviceId}/unsubscribe`);
        }
        
        updateServices(null);
    }
    
    let rateAvg = 0;
    
    if(service.comments.length === 0) rateAvg = -1;
    else {
        for (let i in service.comments) {
            rateAvg += service.comments[i].rate;
        }

        rateAvg /= service.comments.length;
    }
    
    return <div className={style.servicePanel} 
                style={{"--service-status": service.isOnline ? "#3CFB38" : "#FB4438" }}>
            <Link to="/service" state={{ serviceId: service.serviceId }}>
                {service.serviceName}
            </Link>
            <div className="service-actions">
                <Button onClick={handleClickSubscribe} variant={service.isSubscribed ? "secondary" : "danger"}>
                    {service.isSubscribed ? "Отписаться" : "Подписаться"}
                </Button>
                <div>
                    <div className="additional-info">
                        <FontAwesomeIcon icon={faComment}/>
                        <span>{service.comments.length}</span>
                    </div>
                    {
                        rateAvg > 0 &&
                        <div className="additional-info">
                            <FontAwesomeIcon icon={faStar}/>
                            <span>{rateAvg.toFixed(1)}/5.0</span>
                        </div>
                    }
                </div>
                <div className="service-status"
                     style={{"--service-status": service.isOnline ? "#3CFB38" : "#FB4438" }}/>
            </div>
        </div>;
}

async function getUniversityServices(universityId) {
    const requestResult = await axios.get(`/api/services?loadComments=true&universityId=${universityId}`);
    
    return requestResult.data;
}
