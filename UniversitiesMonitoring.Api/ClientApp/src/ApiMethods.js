import axios from "axios";

export async function TestService(id) {
    const result = await axios.get(`/api/services/${id}/test`);
    
    return ensureSuccessStatus(result, result.data);
}

export async function GetUser() {
    const userFromStorage = JSON.parse(sessionStorage.getItem("user"));
    if (userFromStorage !== null) return userFromStorage;
    
    const result = await axios.get("/api/user");

    sessionStorage.setItem("user", JSON.stringify(result.data));
    
    return ensureSuccessStatus(result, result.data);
}

export async function SubscribeToService(serviceId) {
    const result = await axios.post(`/api/services/${serviceId}/subscribe`);

    return ensureSuccessStatus(result, true);
}

export async function UnsubscribeToService(serviceId) {
    const result = await axios.delete(`/api/services/${serviceId}/unsubscribe`);
    
    return ensureSuccessStatus(result, true);
}

export async function GetService(serviceId) {
    const result = await axios.get(`/api/services/${serviceId}`);

    return ensureSuccessStatus(result, result.data);
}

export async function SendReportToService(serviceId, isOnline, content) {
    const apiEntity = {
        serviceId: serviceId,
        isOnline: isOnline,
        content: content
    };
    
    const result = await axios.post(`/api/services/${serviceId}/report`, apiEntity);
    
    return ensureSuccessStatus(result, true);
}

export async function GetReports(serviceId) {
    const result = await axios.get(`/api/services/${serviceId}/reports-by-offline`);

    return ensureSuccessStatus(result, result.data);
}

export async function SendComment(serviceId, data) {
    const result = await axios.post(`/api/services/${serviceId}/comment`, data);
    
    return ensureSuccessStatus(result, true);
}

function ensureSuccessStatus(response, valueIfSuccess) {
    if (response.status === 200) return valueIfSuccess;
    else return null;
} 
