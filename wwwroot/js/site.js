localStorage.removeItem("token");
let token = localStorage.getItem("token");
token=JSON.parse(token);
if(token==null)
    window.location.href = "login.html";
else if (isTokenExpired(token.token)) {
    localStorage.removeItem("token"); // מחיקת הטוקן
    window.location.href = "login.html"; // מעבר לדף התחברות
}
 isTokenExpired=(token) =>{//GPT
        const payloadBase64 = token.split(".")[1]; // פירוק JWT
        const payloadJSON = atob(payloadBase64); // דיקוד Base64
        const payload = JSON.parse(payloadJSON); // הפיכת JSON לאובייקט
        const currentTime = Math.floor(Date.now() / 1000); // זמן נוכחי בשניות
        return payload.exp < currentTime; // true אם פג תוקף
}
const url = '/Jewelry';
let jewelryList = [];
const addJewel = (event) => {
    event.preventDefault();
    const nameJewel = document.getElementById('add-name');
    const pricejewel = document.getElementById('add-price');
    const categoryJewel = document.getElementById('jewelry-select');
    const indexCategory = categoryJewel.selectedIndex;
    const newJewel = {
        id: 0,
        name: nameJewel.value.trim(),
        price: pricejewel.value.trim(),
        category: indexCategory
    };
    fetch(url, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(newJewel)
    })
        .then(response => response.json())
        .then(() => {
            getJewelry();
            nameJewel.value = '';
            pricejewel.value = '';
            categoryJewel.value = '';
        })
        .catch(error => console.error('Unable to add jewel.', error));
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
    fetch(`${url}/${jewelId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(updatedJewel)
    })
        .then(() => getJewelry())
        .catch(error => console.error('Unable to update jewel.', error));
    closeInput();
    return false;
}
const closeInput = () => {
    document.getElementById('editForm').style.display = 'none';
}
const deleteJewel = (id) => {
    fetch(`${url}/${id}`, {
        method: 'DELETE'
    })
        .then(() => getJewelry())
        .catch(error => console.error('Unable to delete jewel.', error));
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
        let tdPrice = dispalayJewelInRow.insertCell(2);
        tdPrice.innerHTML = item.price;
        let tdCategory = dispalayJewelInRow.insertCell(3);
        tdCategory.innerHTML = item.category;
        let tdEdit = dispalayJewelInRow.insertCell(4);
        tdEdit.appendChild(editButton);
        let tdDelete = dispalayJewelInRow.insertCell(5);
        tdDelete.appendChild(deleteButton);
    });
    jewelryList = data;
}
const getJewelry = () => {
    //לקבלת מערך הפריטים GET גישה לפונקציית 
    alert("insert");
    fetch(url)
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error('Unable to get items.', error));
}
