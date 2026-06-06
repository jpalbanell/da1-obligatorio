window.descargarArchivo = (nombreArchivo, base64, tipoMime) => {
    const enlace = document.createElement('a');
    enlace.href = `data:${tipoMime};base64,${base64}`;
    enlace.download = nombreArchivo;
    document.body.appendChild(enlace);
    enlace.click();
    document.body.removeChild(enlace);
};