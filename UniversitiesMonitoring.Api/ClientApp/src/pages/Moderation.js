import {Link} from "react-router-dom";
import {Stack} from "react-bootstrap";

export function Moderation() {
    return <Stack>
        <Link to="/moderator/reports">Жалобы</Link>
        <Link to="/moderator/universities">Управление ВУЗами</Link>
    </Stack>
}