import {createUseStyles} from "react-jss";
import HeaderBackground from "../assets/images/figures.png";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import { faStar, faComment, faMagnifyingGlass } from "@fortawesome/free-solid-svg-icons";
import Constants from "../Constants";

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
        "& span": {
            background: "#FFF",
            fontWeight: "bold",
            color: Constants.brandColor,
            fontSize: "2em",
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
                width: "2vw",
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
            fontSize: 28
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
        gap: 10,
        "& div": {
            display: "flex",
            flexDirection: "row",
            alignItems: "center",
            gap: 2
        }
    }
});

const testData = [
    {
        id: 1,
        name: "ВШЭ",
        commentsCount: 352,
        rating: 4.2,
        isOnline: true,
    },
    {
        id: 2,
        name: "РТУ МИРЭА",
        commentsCount: 102,
        rating: 4.0,
        isOnline: false
    },
    {
        id: 3,
        name: "РТУ МИРЭА",
        commentsCount: 102,
        rating: 4.0,
        isOnline: false
    },
    {
        id: 4,
        name: "РТУ МИРЭА",
        commentsCount: 102,
        rating: 4.0,
        isOnline: false
    },
    {
        id: 5,
        name: "РТУ МИРЭА",
        commentsCount: 102,
        rating: 4.0,
        isOnline: false
    },
    {
        id: 6,
        name: "РТУ МИРЭА",
        commentsCount: 102,
        rating: 4.0,
        isOnline: false
    },
    {
        id: 7,
        name: "РТУ МИРЭА",
        commentsCount: 102,
        rating: 4.0,
        isOnline: false
    },
    {
        id: 8,
        name: "РТУ МИРЭА",
        commentsCount: 102,
        rating: 4.0,
        isOnline: false
    },
    {
        id: 9,
        name: "РТУ МИРЭА",
        commentsCount: 102,
        rating: 4.0,
        isOnline: false
    },
    {
        id: 10,
        name: "РТУ МИРЭА",
        commentsCount: 102,
        rating: 4.0,
        isOnline: false
    },
    {
        id: 11,
        name: "РТУ МИРЭА",
        commentsCount: 102,
        rating: 4.0,
        isOnline: false
    },
    {
        id: 12,
        name: "РТУ МИРЭА",
        commentsCount: 102,
        rating: 4.0,
        isOnline: false
    }
]

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
        <span>Найдите нужный Вам ВУЗ быстрее, вбив его название в поиск</span>
    </div>
}

function Listing() {
    const style = useStyles();
    
    return <div className={style.listing}>
        <SearchBar/>
        <Universities universities={testData}/>
    </div>
}

function SearchBar() {
    return <div>
        <div className="searchbar">
            <FontAwesomeIcon icon={faMagnifyingGlass}/>
            <input type="text" placeholder="Название ВУЗа"/>
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
            <span className="university-name">{props.university.name}</span>    
        </div>
        <div className={style.informationCombo}>
            <div>
                <div className="additional-information">
                    <FontAwesomeIcon icon={faStar} />
                    <span>{props.university.rating}/5</span>
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