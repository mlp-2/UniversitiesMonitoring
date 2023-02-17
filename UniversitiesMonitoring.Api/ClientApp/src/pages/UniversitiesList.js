import {createUseStyles} from "react-jss";
import HeaderBackground from "../assets/images/figures.png";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import { faStar, faComment, faMagnifyingGlass } from "@fortawesome/free-solid-svg-icons";
import Constants from "../Constants";
import axios from "axios";
import {useEffect, useState} from "react";
import {Loading} from "../components/Loading";
import {Link} from "react-router-dom";

const useStyles = createUseStyles({
    layout: {
        display: "block"
    },
    header: {
        flex: 1,
        background: `url(${HeaderBackground})`,
        backgroundRepeat: "repeat",
        height: "25vh",
        display: "flex",
        alignItems: "center",
        justifyContent: "center",
        "& h1": {
            background: "#FFF",
            fontWeight: "bold",
            color: Constants.brandColor,
            borderRadius: "0.5em",
            padding: 10
        }
    },
    listing: {
        flex: 3,
        position: "relative",
        "& .searchbar": {
            display: "flex",
            alignItems: "center",
            position: "absolute",
            left: "10vw",
            right: "10vw",
            top: -25,
            background: "#FFF",
            boxShadow: "0px 3px 8px 3px rgba(0, 0, 0, 0.25)",
            borderRadius: "5em",
            padding: "0.25vh 0.45vw 0.25vh 0.45vw",
            "& input": {
                width: "78vw",
                height: 50,
                border: "none",
                borderRadius: "5em",
                outline: "none",
                fontSize: 24
            },
            "& .fa-magnifying-glass": {
                width: "32px",
                marginRight: 10,
                marginLeft: 10,
                height: 40,
                color: Constants.brandColor
            }
        }
    },
    universitiesWrapper: {
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        paddingTop: 100,
        background: "#F6F5F5",
        gap: 30,
        overflowX: "hidden",
        overflowY: "auto",
        height: "75vh"
    },
    universityContainer: {
        display: "flex",
        justifyContent: "space-between",
        alignItems: "center",
        background: "#FFF",
        width: "80%",
        padding: "20px 40px 20px 40px",
        cursor: "pointer",
        userSelect: "none",
        transition: "box-shadow 0.3s, transform 0.3s",
        "&:hover": {
            boxShadow: "0px 0px 18px 11px rgba(0, 0, 0, 0.25)"  
        },
        "&:active": {
            transform: "scale(0.99)"
        },
        "& .university-name": {
            fontWeight: "bold",
            fontSize: 28,
            color: "#000"
        },
        "& .additional-information": {
            color: "#878787",
            "& span": {
                margin: 4
            }
        },
        borderRadius: 15,
        boxShadow: "0px 4px 16px 4px rgba(0, 0, 0, 0.25)"
    },
    mark: {
        background: "var(--status-color)",
        width: 50,
        height: 50,
        borderRadius: "50%"
    },
    informationCombo: {
        display: "flex",
        flexDirection: "row",
        flexWrap: "wrap",
        gap: 10,
        "& div": {
            display: "flex",
            flexDirection: "row",
            alignItems: "center",
            gap: 2
        }
    },
});

export function UniversitiesList() {
    const style = useStyles();
    
    return <div className={style.layout}>
        <Header/>
        <Listing/>
    </div>
}

function Header() {
    const style = useStyles();
    
    return <div className={style.header}>
        <h1>Вбейте в поиск названия ВУЗа</h1>
    </div>
}

function Listing() {
    const style = useStyles();
    const [universities, setUniversities] = useState(null);
    const [query, setQuery] = useState("");
    
    function updateQuery(newQuery) {
        setQuery(makeStringQueryable(newQuery))
    }
    
    useEffect(() => {
        (async () => {
            const universities = await getUniversities();
            
            for(let i in universities) {
                universities[i].nameQueryable = makeStringQueryable(universities[i].name);
            }
            
            setUniversities(universities);
        })();
    }, []);
    
    if (universities === null) return <Loading/>
    
    return <div className={style.listing}>
        <SearchBar updateSearch={updateQuery}/>
        <Universities universities={universities.filter(university => query === "" || university.nameQueryable.startsWith(query))}/>
    </div>
}

function SearchBar({updateSearch}) {
    function handleChangingOfText(e) {
        updateSearch(e.target.value);
    }
    
    return <div>
        <div className="searchbar">
            <FontAwesomeIcon icon={faMagnifyingGlass}/>
            <input onKeyUp={handleChangingOfText} type="text" placeholder="Название ВУЗа"/>
        </div>
    </div>
}

function Universities(props) {
    const style = useStyles();
    
    return <div className={style.universitiesWrapper}>
        {props.universities.map(university => <UniversityContainer key={university.id} university={university}/>)}
    </div>
}

function UniversityContainer(props) {
    const style = useStyles();
    
    return <div className={style.universityContainer}>
        <div>
            <Link to="/university" 
                  state={{university: props.university}}
                  className="university-name">
                {props.university.name}
            </Link>    
        </div>
        <div className={style.informationCombo}>
            <div>
                <div className="additional-information">
                    <FontAwesomeIcon icon={faStar} />
                    <span>{props.university.rating.toFixed(1)}/5.0</span>
                </div>
                <div className="additional-information">
                    <FontAwesomeIcon icon={faComment} />
                    <span>{props.university.commentsCount}</span>
                </div>
            </div>
            <div className={style.mark} 
                 style={{"--status-color": props.university.isOnline ? "#3CFB38" : "#FB4438"}}/>
        </div>
    </div>
}

async function getUniversities() {
    return (await axios.get("/api/services/universities")).data;
}

function makeStringQueryable(s) {
    return s.replaceAll(" ", "").toLowerCase();
}