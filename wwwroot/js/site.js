const logOut=()=>{
    localStorage.removeItem("token");
    window.location.href = "login.html";
}
const userPayload=(token)=>{
    const payloadBase64 = token.split(".")[1]; // פירוק JWT
    const payloadJSON = atob(payloadBase64); // דיקוד Base64
    const payload = JSON.parse(payloadJSON); // הפיכת JSON לאובייקט
    return payload;
}
let token = localStorage.getItem("token");
token=JSON.parse(token);
let userType=userPayload(token.token).type;
let  userId=userPayload(token.token).UserId

