import {createUseStyles} from "react-jss";

const useStyle = createUseStyles({
    defaultTxtInput: {
        background: "#FFFFFF",
        boxShadow: "0px 0px 10px rgba(0, 0, 0, 0.25)",
        borderRadius: 15,
        border: "none",
        padding: "1vh",
        fontSize: 24,
        transition: "box-shadow 0.3s",
        "&:focus": {
            outline: "none",
            boxShadow: "0px 0px 11px 3px rgba(0, 0, 0, 0.25)"
        }
    }
});

export function TextInput(props) {
    const style = useStyle();

    return <input className={style.defaultTxtInput} {...props}/>
}