import {createUseStyles} from "react-jss";
import HeaderBackground from "../assets/images/figures.png";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faComment, faMagnifyingGlass, faStar} from "@fortawesome/free-solid-svg-icons";
import Constants from "../Constants";
import axios from "axios";
import {useEffect, useState} from "react";
import {Loading} from "../components/Loading";
import {Link} from "react-router-dom";
import FormCheckInput from "react-bootstrap/FormCheckInput";
import FormCheckLabel from "react-bootstrap/FormCheckLabel";
import {Stack} from "react-bootstrap";

const useStyles = createUseStyles({
    layout: {
        display: "block"
    },
    subsOptions: {
        background: "rgba(245,245,245,0.68)",
        backdropFilter: "blur(10px)",
        borderRadius: 10,
        padding: 1
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
        "& .searchbar-wrapper": {
            position: "absolute",
            left: "10vw",
            right: "10vw",
            top: -25,
        },
        "& .searchbar": {
            display: "flex",
            alignItems: "center",
            background: "#FFF",
            boxShadow: "0px 3px 8px 3px rgba(0, 0, 0, 0.25)",
            borderRadius: "5em",
            padding: "0.25vh 0.45vw 0.25vh 0.45vw",
            "& input": {
                width: "100%",
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
        userSelect: "none",
        "& .university-name": {
            fontWeight: "bold",
            fontSize: 25,
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
    "@media screen and (max-width: 750px)": {
        mark: {
            display: "none !important"
        },
        universityContainer: {
            borderRight: "15px solid var(--status-color) !important",
            alignItems: "flex-start",
            flexDirection: "column"
        }
    }
});

export function UniversitiesList() {
    const style = useStyles();
    const [universities, setUniversities] = useState(null)

    useEffect(() => {
        (async () => {
            const universities = await getUniversities();

            for (let i in universities) {
                universities[i].nameQueryable = makeStringQueryable(universities[i].name);
            }

            setUniversities(universities);
        })();
    }, []);

    if (universities === null) return <Loading/>

    return <div className={style.layout}>
        <Header/>
        <Listing universities={universities}/>
    </div>
}

function Header() {
    const style = useStyles();

    return <div className={style.header}>
        <h1>Выберите ВУЗ</h1>
    </div>
}

function Listing({universities}) {
    const style = useStyles();
    const [query, setQuery] = useState("");
    const [loading, setLoadingState] = useState(false);
    const [showSub, setSub] = useState(false);

    function updateQuery(newQuery) {
        setQuery(makeStringQueryable(newQuery))
    }

    if (universities === null && !loading) {
        setLoadingState(true);
        return null;
    }

    return <div className={style.listing}>
        <SearchBar updateShowSub={(show) => setSub(show)} updateSearch={updateQuery}/>
        <Universities universities={universities.filter(university =>
            (showSub && university.isSubscribed || !showSub) &&
            (query === "" || university.nameQueryable.startsWith(query)))}/>
    </div>
}

function SearchBar({updateSearch, updateShowSub}) {
    const style = useStyles();
    
    function handleChangingOfText(e) {
        updateSearch(e.target.value);
    }

    return <Stack gap={2} className="searchbar-wrapper">
        <div className="searchbar">
            <FontAwesomeIcon icon={faMagnifyingGlass}/>
            <input onKeyUp={handleChangingOfText} type="text" placeholder="Название ВУЗа"/>
        </div>
        <Stack className={style.subsOptions} direction="horizontal" gap={2}>
            <FormCheckLabel htmlFor="subscribed">Показывать ВУЗы, на которые Вы подписаны</FormCheckLabel>
            <FormCheckInput id="subscribed" onChange={(e) => updateShowSub(e.target.checked)}/>
        </Stack>
    </Stack>
}

function Universities(props) {
    const style = useStyles();

    return <div className={style.universitiesWrapper}>
        {props.universities.map(university => <UniversityContainer key={university.id} university={university}/>)}
    </div>
}

function UniversityContainer(props) {
    const style = useStyles();

    return <div className={style.universityContainer}
                style={{"--status-color": props.university.isOnline ? "#3CFB38" : "#FB4438"}}>
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
                    <FontAwesomeIcon icon={faStar}/>
                    <span>{props.university.rating.toFixed(1)}/5.0</span>
                </div>
                <div className="additional-information">
                    <FontAwesomeIcon icon={faComment}/>
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