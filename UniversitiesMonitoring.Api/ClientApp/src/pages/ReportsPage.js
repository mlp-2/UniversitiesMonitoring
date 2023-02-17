import {useEffect, useState} from "react";
import axios from "axios";
import {Navigate} from "react-router-dom";
import Swal from "sweetalert2";

export function ReportsPage() {
    const [reports, setReports] = useState(null);
    
    useEffect(() => {
        (async () => {
            try {
                const result = await axios.get("/api/moderator/reports");    
                
                setReports(result.data);
            } catch (error) {
                if (error.status === 401) {
                    await Swal.fire({
                        title: "Вы не вошли в аккаунт",
                        icon: "error",
                        showConfirmButton: false,
                        timer: 2000
                    });
                    
                    return <Navigate to="moderator/login"/>
                } else {
                    await Swal.fire({
                        title: "Что-то пошло не так",
                        icon: "error",
                        showConfirmButton: false,
                        timer: 2000
                    });
                }
            }
        })();
    }, []);
    
    if (reports === null) return <span>Загружаем сообщения...</span>
    
    return <div>
        <h1>Сообщения</h1>
        <div>
            {reports.map(report => <ReportContainer report={report}/>)}
        </div>
    </div>
}

function ReportContainer({report}) {
    return <div>
        <span>Сообщение о доступности сервиса</span>
        
    </div>
}