function get_val(button) {

    const card = button.closest(".card");

    if (card) {
        card.classList.add("fade-out"); // Lance l'animation

        // Attend la fin de l'animation avant de supprimer le DOM
        setTimeout(() => {
            card.remove();
        }, 200); // 400ms = même durée que dans le CSS
    }
    const id = button.value;
    const action = "Delete";
    const data = { action, id };
    console.log(id);
    window.chrome.webview.postMessage(data);
}


function get_data(button) {

    const card = button.closest(".card")
    const id = button.value;
    const action = "edit";
    const data = { action, id };
    console.log(id);
    window.chrome.webview.postMessage(data);

}