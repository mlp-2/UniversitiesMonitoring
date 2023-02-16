import {createUseStyles} from "react-jss";

const useStyles = createUseStyles({
    defaultButton: {
        background: "#076FEA",
        borderRadius: 15,
        color: "#FFF",
        border: "none",
        outline: "none",
        padding: "0.5em 1em 0.5em 1em",
        fontSize: 24,
        transition: "background 0.3s, transform 0.3s",
        textShadow: "#050505 0.3px 0 1px",
        "&:hover": {
            background: "#0a64c7"
        },
        "&:active": {
            transform: "scale(0.95)"
        }
    }
});

export function SubmitButton(props) {
    const style = useStyles();
    
    return <input type="submit" className={style.defaultButton} {...props}/>
}