
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
const uri = '/Jewelry';
let jewelryList = [];
const addJewel = (event) => {
    event.preventDefault();
    const nameJewel = document.getElementById('add-name');
    const pricejewel  = document.getElementById('add-price');
    const categoryJewel = document.getElementById('jewelry-select');
    const  userIdJewel=document.getElementById('add-userId');
    const indexCategory = categoryJewel.selectedIndex;
    const newJewel = {
        id: 0,
        userId:userIdJewel.value!==''?userIdJewel.value.trim():0,
        name: nameJewel.value.trim(),
        price:pricejewel.value!=='' ?pricejewel.value.trim():0,
        category: indexCategory
    };
    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            "Authorization": `Bearer ${token.token}` 
        },
        body: JSON.stringify(newJewel)
    })
        .then(response => {
            if(!response.ok)
                return response.text().then(err => { throw new Error(err); });
             return   response.json()})
        .then(() => {
            getJewelry();
            nameJewel.value = '';
            pricejewel.value = '';
            categoryJewel.value = '';
        })
        .catch(error =>{ console.error('Unable to add jewel.', error);alert(error)});
}
const displayEditForm = (id) => {
    const item = jewelryList.find(item => item.id === id);
    document.getElementById('edit-name').value = item.name;
    document.getElementById('edit-id').value = item.id;
    document.getElementById('edit-price').value = item.price;
    document.getElementById('edit-jewelry-select').selectedIndex = item.category;
    document.getElementById('editForm').style.display = 'block';
}
const updateJewel = (event) => {
    event.preventDefault();
    const jewelId = document.getElementById('edit-id').value;
    const updatedJewel = {
        id: parseInt(jewelId, 10),
        name: document.getElementById('edit-name').value.trim(),
        price: document.getElementById('edit-price').value.trim(),
        category: document.getElementById('edit-jewelry-select').selectedIndex
    };
    fetch(`${uri}/${jewelId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            "Authorization": `Bearer ${token.token}` 
        },
        body: JSON.stringify(updatedJewel)
    })
    .then((response)=>{
        if(!response.ok)
            return response.text().then(err => { throw new Error(err); });
    })
        .then(() => getJewelry())
        .catch(error =>{ console.error('Unable to update jewel.', error);alert(error)});
    closeInput();
    return false;
}
const closeInput = () => {
    document.getElementById('editForm').style.display = 'none';
}

const _displayItems = (data) => {
    const tBody = document.getElementById('Jewelry-table');
    tBody.innerHTML = '';
    const button = document.createElement('button');
    data.forEach(item => {
        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${item.id})`);
        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteJewel(${item.id})`);
        let dispalayJewelInRow = tBody.insertRow();
        let tdId = dispalayJewelInRow.insertCell(0);
        tdId.innerHTML = item.id;
        let tdName = dispalayJewelInRow.insertCell(1);
        tdName.innerHTML = item.name;
        let userId = dispalayJewelInRow.insertCell(2);
        userId.innerHTML = item.userId;
        let tdPrice = dispalayJewelInRow.insertCell(3);
        tdPrice.innerHTML = item.price;
        let tdCategory = dispalayJewelInRow.insertCell(4);
        item.category > 0 ? tdCategory.innerHTML =document.getElementById("edit-jewelry-select")[item.category].innerHTML:'' ;
        let tdEdit = dispalayJewelInRow.insertCell(5);
        tdEdit.appendChild(editButton);
        let tdDelete = dispalayJewelInRow.insertCell(6);
        tdDelete.appendChild(deleteButton);
        
    });
    jewelryList = data;
}
const deleteJewel = (id) => {
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
        .then(() => getJewelry())
        .catch(error => console.error('Unable to delete jewel.', error));
}
const getJewelry = () => {
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
const getJewel=()=>{
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
