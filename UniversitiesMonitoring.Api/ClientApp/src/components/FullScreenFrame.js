import {createUseStyles} from "react-jss";
import Constants from "../Constants";

const useStyles = createUseStyles({
    fullScreenFrame: {
        position: "absolute",
        background: Constants.brandColor,
        color: "#FFF",
        width: "100vw",
        height: "100vh",
        display: "flex",
        justifyContent: "center",
        alignItems: "center"
    },
})

export function FullscreenFrame(props) {
    const style = useStyles();

    return <div className={style.fullScreenFrame}>
        {props.children}
    </div>
}