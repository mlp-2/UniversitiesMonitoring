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
        animationDelay: "calc(200ms * var(--i))",
        animationName: "$circle-animation",
        animationDuration: "1s",
        animationDirection: "alternate",
        animationIterationCount: "infinite"
    },
    "@keyframes circle-animation": {
        from: {
            transform: "translateY(0)"
        },
        to: {
            transform: "translateY(-50px)"
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