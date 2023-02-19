import {createUseStyles} from "react-jss";
import DefaultConstants from "./Constants";

const useGlobalStyles = createUseStyles({
    brandFontColored: {
        color: DefaultConstants.brandColor
    }
})

export default useGlobalStyles;