
const redirectToJewleryPage=()=>{
    window.location.href="index.html";
}
const uri = '/User';
let newUri=uri;
let userList = [];
if(userType==='User')
{
    document.getElementById('add-form').hidden=true;
   newUri=`${uri}/${userId}`;
   document.querySelectorAll('th')[5].hidden=true;
   document.getElementById('getBtn').hidden=true;
   document.getElementById('idInput').hidden=true;
    
}
const addUser = (event) => {
    event.preventDefault();
    const nameUser = document.getElementById('add-name');
    const passwordUser = document.getElementById('add-password');
    const typeUser = document.getElementById('users-select');
   const newUser = {
        id: 0,
        name: nameUser.value.trim(),
        password: passwordUser.value,
        type: typeUser.value
    };
    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            "Authorization": `Bearer ${token.token}` 
        },
        body: JSON.stringify(newUser)
    })
        .then(response => {  if(!response.ok)
            {
                return response.text().then(err => { throw new Error(err); });
            }
             response.json()})
        .then(() => {
            getUsers();
            nameUser.value = '';
            passwordUser.value = '';
            typeUser.value = '';
        })
        .catch(error => {console.error('Unable to add user.' );alert(error)
        });
}
const displayEditForm = (id) => {
    const user = userList.find(user => user.id === id);
    document.getElementById('edit-name').value = user.name;
    document.getElementById('edit-id').value = user.id;
    document.getElementById('edit-password').value = user.password;
    document.getElementById('edit-users-select').value =user.type;
    document.getElementById('editForm').style.display = 'block';
}
const updateUser = (event) => {
    event.preventDefault();
  
    const userId = document.getElementById('edit-id').value;
    const updatedUser = {
        id: parseInt(userId, 10),
        name: document.getElementById('edit-name').value.trim(),
        password: document.getElementById('edit-password').value.trim(),
        type: document.getElementById('edit-users-select').value
    };
    fetch(`${uri}/${userId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            "Authorization": `Bearer ${token.token}` 
        },
        body: JSON.stringify(updatedUser)
    })
        .then((response) => { if(!response.ok)
            {
                return response.text().then(err => { throw new Error(err); });
            }
             getUsers()}
    )
        .catch(error =>{ console.error('Unable to update user.', error);alert(error)});
    closeInput();
    return false;
}
const closeInput = () => {
    document.getElementById('editForm').style.display = 'none';
}
const deleteUser = (id) => {
    fetch(`${uri}/${id}`, {
        method: 'DELETE',
        headers:{
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token.token}` // דוגמה לשימוש ב-Token
            }

    })
       .then((response) => { if(!response.ok)
        {
            return response.text().then(err => { throw new Error(err); });
        }
             getUsers()})
        .catch(error => {console.error('Unable to delete user.', error);  alert(error)});
}
const _displayItems = (data) => {
    const tBody = document.getElementById('users-table');
    tBody.innerHTML = '';
    const button = document.createElement('button');
    data.forEach(user => {
        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${user.id})`);
        let dispalayUserInRow = tBody.insertRow();
        let tdId = dispalayUserInRow.insertCell(0);
        tdId.innerHTML = user.id;
        let tdName = dispalayUserInRow.insertCell(1);
        tdName.innerHTML = user.name;
        let tdPassword = dispalayUserInRow.insertCell(2);
        tdPassword.innerHTML = user.password;
        let tdType = dispalayUserInRow.insertCell(3);
        tdType.innerHTML =user.type;
        let tdEdit = dispalayUserInRow.insertCell(4);
        tdEdit.appendChild(editButton);
        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteUser(${user.id})`);
        if(userType=="Admin")
        {
        let tdDelete = dispalayUserInRow.insertCell(5);
        tdDelete.appendChild(deleteButton);
        }
            
    });
    userList = data;
}
const getUsers = () => {
    //לקבלת מערך הפריטים GET גישה לפונקציית 
    fetch(newUri,{
        headers:{
        "Content-Type": "application/json",
        "Authorization": `Bearer ${token.token}` // דוגמה לשימוש ב-Token
        }
    })
        .then(response =>
            { 
                if(!response.ok)
                    return response.text().then(err => { throw new Error(err); });
              return  response.json()
            }
        )
        .then(data =>{ const usersList = Array.isArray(data) ? data : [data];
            _displayItems(usersList);})
        .catch(error => {console.error('Unable to get users.', error);alert(error)});
}

const getUser=()=>{
    const id=document.getElementById('idInput').value;
    fetch(`${uri}/${id}`,{
        headers:{
        "Content-Type": "application/json",
        "Authorization": `Bearer ${token.token}` // דוגמה לשימוש ב-Token
        }
    })
        .then(response =>{
            if(!response.ok)
            {
                return response.text().then(err => { throw new Error(err); });
            }
           return  response.json()})
        .then(data => _displayItems([data]))
        .catch(error => {console.error('Unable to get user.', error);alert(error)});
}
