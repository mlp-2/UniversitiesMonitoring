import useGlobalStyles from "../GlobalStyle";
import PreviewImage from "../assets/images/preview.png";
import {createUseStyles} from "react-jss";

const useStyles = createUseStyles({
    layout: {
        display: "flex",
        flexDirection: "row",
        width: "100%",
        height: "100%",
        justifyContent: "space-between"
    },
    leftPanel: {
        display: "flex",
        flexDirection: "column",
        justifyContent: "space-around",
        alignItems: "center",
        flex: 1
    },
    headerOfLeftPanel: {
        padding: "8vh",
        flex: 1
    },
    contentOfLeftPanel: {
        flex: 3,
        width: "100%",
        padding: "8vh"
    },
    rightPanel: {
        flex: 2,
        backgroundSize: "contain",
        backgroundColor: "#F6F5F5",
        height: "100%",
        display: "flex",
        justifyContent: "center",
        alignItems: "center"
    }
});

export function WelcomePage(props) {
    const ctxStyle = useStyles();
    
    return <div className={ctxStyle.layout}>
        <LeftPanel>
            {props.children}
        </LeftPanel>
        <RightPanel/>
    </div>
}

function LeftPanel(props) {
    const globalStyle = useGlobalStyles();
    const ctxStyle = useStyles();
    
    return <div className={ctxStyle.leftPanel}>
        <div className={ctxStyle.headerOfLeftPanel}>
            <h1>
                Контролируйте доступность ВУЗов с помощью <span className={globalStyle.brandFontColored}>UniversitiesMonitoring</span>
            </h1>
        </div>
        <div className={ctxStyle.contentOfLeftPanel}>
            {props.children}
        </div>
    </div>;
}

function RightPanel() {
    const styles = useStyles();
    
    return <div className={styles.rightPanel}>
        <img src={PreviewImage} alt="Что-то пошло не так"  width="80%"/>
    </div>
}