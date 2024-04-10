function getElementContentById(elementId) {
    var element = document.getElementById(elementId);

    if (element) {
        return element.innerHTML;
    }
    else {
        return '';
    }
}

function setElementContentById(elementId, value) {
    document.getElementById(elementId).innerHTML = value;
}

function setFocusById(elementId) {
    var element = document.getElementById(elementId);

    if (element) {
        element.focus();
    }
};

function deleteElementContentById(elementId) {
    var element = document.getElementById(elementId);

    if (element) {
        element.innerHTML = '';
    }
}

function handleDragOver(event) {
    event.preventDefault();

    // Získání typu přetahovaných dat
    var draggedTypes = event.dataTransfer.types;

    if (draggedTypes && draggedTypes.length > 0) {
        var draggedType = draggedTypes[0];

        // Kontrola, zda jsou přetahována souborová data (obrázky)
        if (draggedType.includes("Files") || draggedType.includes("application/x-moz-file")) {

            const target = event.currentTarget;
            target.classList.add("dragover");

            // Posluchač zajistí, aby se třída dragover vymazala v případě, kdy přetahovaný obrázek opustí div, ze kterého je metoda DragOver volána
            target.addEventListener("dragleave", (event) => {
                if (event.currentTarget.elementId === target.elementId) {
                    event.currentTarget.classList.remove("dragover");
                }
            });
        }
    }
}

function handleImage(image, maxHeight, quality) {
    const selection = window.getSelection();
    const range = selection.getRangeAt(0);

    // Kontrola, zda je výběr prázdný (žádný text nebo jiný obsah)
    if (!selection.isCollapsed) {
        // V případě, že výběr není prázdný, není možné vložit obrázek
        return;
    }

    var reader = new FileReader();
    reader.onload = function (e) {
        var img = new Image();
        img.onload = function () {

            // Komprimace obrázku
            var canvas = document.createElement('canvas');
            var ctx = canvas.getContext('2d');
            var width = img.width;
            var height = img.height;
            canvas.width = width;
            canvas.height = height;
            ctx.drawImage(img, 0, 0, width, height);
            var compressedImageDataUrl = canvas.toDataURL('image/webp', quality);

            // Úprava velikosti a stylování obrázku
            var resizedImg = document.createElement('img');
            resizedImg.style.maxHeight = maxHeight + 'px';
            resizedImg.style.marginRight = '6px';
            resizedImg.style.marginBottom = '5px';
            resizedImg.src = compressedImageDataUrl;

            // Vložení obrázku na pozici kurzoru
            range.deleteContents();
            range.insertNode(resizedImg);

            // Nastavení pozice kurzoru za vloženým obrázkem
            const newRange = document.createRange();
            newRange.setStartAfter(resizedImg);
            newRange.collapse(true);
            selection.removeAllRanges();
            selection.addRange(newRange);
        };

        img.src = e.target.result;
    };

    reader.readAsDataURL(image);
}

function handleDrop(event, maxHeight) {
    event.preventDefault();

    const target = event.currentTarget;
    target.focus();
    target.classList.remove('dragover');

    var file = event.dataTransfer.files[0];

    if (file && file.type.startsWith('image/')) {
        handleImage(file, maxHeight, 0.5);
    }
}

function insertImageToDiv(imageId, divId, maxHeight) {
    var div = document.getElementById(divId);
    div.focus(); // Explicitní nastavení fokusu zajistí, aby se obrázek nevložil do jiného elementu, ve kterém byl původně fokus

    var image = document.getElementById(imageId);
    var file = image.files[0];
    image.value = '';

    if (file && file.type.startsWith('image/')) {
        handleImage(file, maxHeight, 0.5);
    }
}

function setListenersDivContenteditable(divId) {
    const div = document.getElementById(divId);

    if (!div) {
        return;
    }

    div.addEventListener('keydown', handleTabKeyPress);

    div.addEventListener('draggable', handleDraggableEvent);

    div.addEventListener('paste', function (event) {
        handleScreenshotPaste(event, divId, 1500);
    });
}

// Funkce pro zachytávání klávesy "Tab" v contenteditable divu
function handleTabKeyPress(event) {
    if (event.key === 'Tab') {
        event.preventDefault();

        const tabCharacter = '\u00a0\u00a0\u00a0\u00a0'; // Čtyři mezery

        // Vytvoření textového uzlu s tabulačními znaky
        const tabNode = document.createTextNode(tabCharacter);

        // Přidání textového uzlu na pozici kurzoru
        const selection = document.getSelection();
        const range = selection.getRangeAt(0);

        range.deleteContents();
        range.insertNode(tabNode);

        // Posunutí kurzoru za vložený textový uzel
        range.setStartAfter(tabNode);
        range.setEndAfter(tabNode);
        selection.removeAllRanges();
        selection.addRange(range);
    }
}

function handleDraggableEvent(event) {
    if (event.target.tagName && event.target.tagName.toLowerCase() === 'img') {
        event.target.draggable = false;
    }
}

function handleScreenshotPaste(event, targetDivId, maxHeight) {
    const targetDiv = document.getElementById(targetDivId);
    targetDiv.focus();

    const clipboardData = event.clipboardData || window.clipboardData;

    if (clipboardData) {
        const clipboardItems = clipboardData.items;

        if (clipboardItems) {
            const clipboardImageItem = clipboardItems[0];

            // Pokud bude pomocí klávesové zkratky CTRL+V vložen text, bude metoda ukončena
            if (clipboardImageItem.type === 'text/plain') {
                return;
            }

            // Metoda event.preventDefault musí být volána, aby se zabránilo zduplikování vloženého obrázku. Nesmí se volat na záčátku metody, aby to nebránilo vkládání textu.
            event.preventDefault();

            if (clipboardImageItem.type.indexOf('image') !== -1) {
                const clipboardBlob = clipboardImageItem.getAsFile();
                const clipboardFile = new File([clipboardBlob], 'screenshot.png', { type: 'image/png' });

                handleImage(clipboardFile, maxHeight, 0.99);
            }
        }
    }
}

function applyFontSettings(family, size, weight, style, color, backroundColor, targetDivId) {
    const targetDiv = document.getElementById(targetDivId);
    targetDiv.focus();

    const selection = window.getSelection();
    const range = selection.getRangeAt(0);

    // Vytvoření nového uzlu pro uložení všech elementů
    const wrapperNode = document.createElement('div');
    wrapperNode.style.fontSize = size;
    wrapperNode.style.backgroundColor = 'white';
    wrapperNode.style.lineHeight = 'initial';

    // Vytvoření nového <span> prvku s aplikovanými styly
    const fontSettingsSpan = document.createElement('span');
    fontSettingsSpan.style.fontFamily = family;
    fontSettingsSpan.style.fontSize = size;
    fontSettingsSpan.style.fontWeight = weight;
    fontSettingsSpan.style.fontStyle = style;
    fontSettingsSpan.style.color = color;
    fontSettingsSpan.style.backgroundColor = backroundColor;
    fontSettingsSpan.style.lineHeight = 'initial';

    // Podmínka pro nastavení počtu pevných mezer v závislosti na barvě pozadí
    if (backroundColor !== "#FFFFFF") {
        fontSettingsSpan.innerHTML = '&nbsp;&nbsp;';
    } else {
        fontSettingsSpan.innerHTML = '&nbsp;';
    }

    // Vložení nového <span> prvku do nového uzlu
    wrapperNode.appendChild(fontSettingsSpan);

    // Vložení nového uzlu na pozici kurzoru
    range.insertNode(wrapperNode);

    // Nastavení pozice kurzoru za vloženým barevným textem
    const newRange = document.createRange();
    newRange.setStartAfter(fontSettingsSpan);
    newRange.collapse(true);

    // Odstranění všech existujících rozsahů a přidání nového rozsahu
    selection.removeAllRanges();
    selection.addRange(newRange);

    // Posunutí kurzoru o jedno místo doleva
    selection.modify('move', 'left', 'character');
}