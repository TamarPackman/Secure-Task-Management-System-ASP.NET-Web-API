const url='/jewerly'
jewelryList=[];
const addJewel=(event)=>{
    event.preventDefault();
    const name=document.getElementById('add-name')
    const price=document.getElementById('add-price')
    const category=document.getElementById('add-category')
    const newJewel={
        id:0,
        name:name.value.trim(),
        price:price.value.trim(),
        category:category.selectedIndex
    }
    fetch(url, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(newJewel)
    })
    .then(response => response.json() )
    .then(() => {
        getJewelry();
        name.value=''
        price.value=''
    })
    .catch(error => alert('Unable to add jewel.', error));
}

const getJewelry=()=>{
fetch(url)
.then(response => response.json())

.then(data => _displayItems(data))
.catch(error => console.error('Unable to get items.', error));
}

const  _displayItems=(data) =>{
   
    const tBody = document.getElementById('jewelry');
    tBody.innerHTML = '';
     const button = document.createElement('button');
    
    data.forEach(item => {
        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${item.id})`);
        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteJewel(${item.id})`);
        let displayJewelInRow = tBody.insertRow();
        let tdId = displayJewelInRow.insertCell(0);
        tdId.innerHTML=item.id;
        let tdName = displayJewelInRow.insertCell(1);
        tdName.innerHTML=item.name;
        let tdPrice = displayJewelInRow.insertCell(2);
        tdPrice.innerHTML=item.price;
        let tdCategory=displayJewelInRow.insertCell(3)
        let tdEdit=displayJewelInRow.insertCell(4)
        tdEdit.appendChild( editButton)
        let tdDelete=displayJewelInRow.insertCell(5)
tdDelete.appendChild(deleteButton)
const firstSelect = document.querySelector("select");
        tdCategory.innerHTML= firstSelect.options[item.category].value;
    });
    jewelryList=data
}
const displayEditForm=(id)=>{
    const item = jewelryList.find(item => item.id === id);
   document.getElementById('edit-name').value=item.name
    document.getElementById('edit-id').value=item.id
   document.getElementById('edit-price').value=item.price
   document.getElementById('edit-category').selectedIndex=item.category
    document.getElementById('editForm').style.display = 'block';
}
const updatejewel=(event)=>{
    event.preventDefault()
    const jewelId = document.getElementById('edit-id').value;
    const jewel = {
        id: parseInt(jewelId, 10),
        price: document.getElementById('edit-price').value.trim(),
        name: document.getElementById('edit-name').value.trim(),
        category:document.getElementById('edit-category').selectedIndex
    };
    fetch(`${url}/${jewelId}`, {
            method: 'PUT',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(jewel)
        })
        .then(() => getJewelry())
        .catch(error => console.error('Unable to update item.', error));

    closeInput();
    //  return false;
}
const closeInput=()=>{
    document.getElementById('editForm').style.display = 'none';
}
const deleteJewel=(id) =>{
    fetch(`${url}/${id}`, {
            method: 'DELETE'
        })
        .then(() =>getJewelry())
        .catch(error => console.error('Unable to delete item.', error));
}
