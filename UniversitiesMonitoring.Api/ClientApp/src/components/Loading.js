import {FullscreenFrame} from "./FullScreenFrame";
import {createUseStyles} from "react-jss";

const useStyles = createUseStyles({
    circleContainer: {
        display: "flex",
        flexDirection: "row",
        gap: 25
    },
    circle: {
        display: "block",
        background: "#FFF",
        width: 50,
        height: 50,
        borderRadius: "50%",
        animationDelay: "calc(300ms * var(--i))",
        animationName: "$circle-animation",
        animationDuration: "0.5s",
        animationDirection: "alternate",
        animationIterationCount: "infinite"
    },
    "@keyframes circle-animation": {
        from: {
            opacity: "1",
            transform: "scale(1)"
        },
        to: {
            opacity: "0.75",
            transform: "scale(0.8)"
        }
    }
});

export function Loading() {
    const style = useStyles();

    return <FullscreenFrame>
        <div className={style.circleContainer}>
            <span className={style.circle} style={{"--i": 0}}/>
            <span className={style.circle} style={{"--i": 1}}/>
            <span className={style.circle} style={{"--i": 2}}/>
        </div>
    </FullscreenFrame>
}