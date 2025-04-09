
 isTokenExpired=(token) =>{
        const payloadBase64 = token.split(".")[1]; 
        const payloadJSON = atob(payloadBase64); 
        const payload = JSON.parse(payloadJSON); 
        const currentTime = Math.floor(Date.now() / 1000); 
        return payload.exp < currentTime; 
}
if(token==null)
    window.location.href = "login.html";
else if (isTokenExpired(token.token)) {
    localStorage.removeItem("token"); 
    window.location.href = "login.html"; 
}
 if(userType==='Admin')
 {
 document.getElementById("Display-users").hidden=false;
 document.getElementById("add-userId").hidden=false;
 }
else
{
document.getElementById("Display-user").hidden=false;
document.getElementById("add-userId").value=userId;
}
 const redirectToUsersPage = () => {
    window.location.href = "users.html"; 
};
const uri = '/Task';
let TaskList = [];
const addTask = (event) => {
    event.preventDefault();
    const nameTask = document.getElementById('add-name');
    const DescriptionTask  = document.getElementById('add-Description');   
    const categoryTask = document.getElementById('Task-select');
    const  userIdTask=document.getElementById('add-userId');
    const indexCategory = categoryTask.selectedIndex;
    const newTask = {
        id: 0,
        userId:userIdTask.value!==''?userIdTask.value.trim():0,
        name: nameTask.value.trim(),
        description:DescriptionTask.value.trim(),
        status: indexCategory
    };
    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            "Authorization": `Bearer ${token.token}` 
        },
        body: JSON.stringify(newTask)
    })
        .then(response => {
            if(!response.ok)
                return response.text().then(err => { throw new Error(err); });
             return   response.json()})
        .then(() => {
            getTasks();
            nameTask.value = '';
            DescriptionTask.value = '';
            categoryTask.value = '';
        })
        .catch(error =>{ console.error('Unable to add task.', error);alert(error)});
}
const displayEditForm = (id) => {
    const item = TaskList.find(item => item.id === id);
    document.getElementById('edit-name').value = item.name;
    document.getElementById('edit-id').value = item.id;
    document.getElementById('edit-Description').value = item.description;
    document.getElementById('edit-Task-select').selectedIndex = item.status;
    document.getElementById('editForm').style.display = 'block';
}
const updateTask = (event) => {
    event.preventDefault();
    const TaskId = document.getElementById('edit-id').value;
    const updatedTask = {
        id: parseInt(TaskId, 10),
        name: document.getElementById('edit-name').value.trim(),
        description: document.getElementById('edit-Description').value.trim(),
        status: document.getElementById('edit-Task-select').selectedIndex
    };
    fetch(`${uri}/${TaskId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            "Authorization": `Bearer ${token.token}` 
        },
        body: JSON.stringify(updatedTask)
    })
    .then((response)=>{
        if(!response.ok)
            return response.text().then(err => { throw new Error(err); });
    })
        .then(() => getTasks())
        .catch(error =>{ console.error('Unable to update task.', error);alert(error)});
    closeInput();
    return false;
}
const closeInput = () => {
    document.getElementById('editForm').style.display = 'none';
}

const _displayItems = (data) => {
    const tBody = document.getElementById('Task-table');
    tBody.innerHTML = '';
    const button = document.createElement('button');
    data.forEach(item => {
        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${item.id})`);
        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteTask(${item.id})`);
        let dispalayTaskInRow = tBody.insertRow();
        let tdId = dispalayTaskInRow.insertCell(0);
        tdId.innerHTML = item.id;
        let tdName = dispalayTaskInRow.insertCell(1);
        tdName.innerHTML = item.name;
        let userId = dispalayTaskInRow.insertCell(2);
        userId.innerHTML = item.userId;
        let tdDescription = dispalayTaskInRow.insertCell(3);
        tdDescription.innerHTML = item.description;
        let tdCategory = dispalayTaskInRow.insertCell(4);
        item.status >= 0 ? tdCategory.innerHTML =document.getElementById("edit-Task-select")[item.status].innerHTML:'' ;
        let tdEdit = dispalayTaskInRow.insertCell(5);
        tdEdit.appendChild(editButton);
        let tdDelete = dispalayTaskInRow.insertCell(6);
        tdDelete.appendChild(deleteButton);
        
    });
    TaskList = data;
}
const deleteTask = (id) => {
    fetch(`${uri}/${id}`, {
        method: 'DELETE',
        headers:{
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token.token}` 
            }

    }).then((response)=>{
        if(!response.ok)
            return response.text().then(err => { throw new Error(err); });
    })
        .then(() => getTasks())
        .catch(error => console.error('Unable to delete task.', error));
}
const getTasks = () => {
    fetch(uri,{
        headers:{
        "Content-Type": "application/json",
        "Authorization": `Bearer ${token.token}` 
        }
    })
        .then(response =>{ if(!response.ok)
            {
                return response.text().then(err => { throw new Error(err); });
            } return  response.json()})
        .then(data => _displayItems(data))
        .catch(error => console.error('Unable to get items.', error));
}
const getTask=()=>{
    const id=document.getElementById('idInput').value;
    fetch(`${uri}/${id}`,{
        headers:{
        "Content-Type": "application/json",
        "Authorization": `Bearer ${token.token}` 
        }
    })
        .then(response =>{
            if(!response.ok)
            {
                return response.text().then(err => { throw new Error(err); });
            }
           return  response.json()})
        .then(data => _displayItems([data]))
        .catch(error => {console.error('Unable to get item.', error);alert(error)});
}
