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
       "&:hover": {
           background: "#0a64c7"
       },
       "&:active": {
           transform: "scale(0.95)"
       }
   } 
});

export function Button(props) {
    const style = useStyles();
    
    return <button className={style.defaultButton} {...props}>
        {props.children} 
    </button>
}