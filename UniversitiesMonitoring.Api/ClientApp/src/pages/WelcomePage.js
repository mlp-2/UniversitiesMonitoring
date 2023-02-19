import useGlobalStyles from "../GlobalStyle";
import PreviewImage from "../assets/images/preview.png";
import {createUseStyles} from "react-jss";
import {Container} from "react-bootstrap";

const useStyles = createUseStyles({
    layout: {
        display: "flex",
        flexDirection: "row",
        width: "100%",
        height: "100%",
        justifyContent: "space-between"
    },
    leftPanel: {
        height: "100%",
        display: "flex",
        flexDirection: "column",
        justifyContent: "space-around",
        alignItems: "center",
        flex: 1,
        gap: "10%"
    },
    headerOfLeftPanel: {
        paddingLeft: 20,
        paddingRight: 20,
        paddingTop: 20,
        flex: 1
    },
    contentOfLeftPanel: {
        flex: 3,
        width: "100%",
        padding: "5%",
        display: "block"
    },
    rightPanel: {
        backgroundColor: "#F6F5F5",
        display: "flex",
        width: "100%",
        background: `url(${PreviewImage})`,
        justifyContent: "center",
        alignItems: "center",
        backgroundSize: "contain",
        backgroundRepeat: "no-repeat",
        backgroundPosition: "center"
    },
    "@media screen and (max-width: 1024px)": {
        rightPanel: {
            display: "none"
        }
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
    
    return <Container className={ctxStyle.leftPanel}>
        <div className={ctxStyle.headerOfLeftPanel}>
            <h1>
                Контролируйте доступность ВУЗов с помощью <span className={globalStyle.brandFontColored}>UniversitiesMonitoring</span>
            </h1>
        </div>
        <div className={ctxStyle.contentOfLeftPanel}>
            {props.children}
        </div>
    </Container>;
}

function RightPanel() {
    const styles = useStyles();
    
    return <div className={styles.rightPanel}/>
}