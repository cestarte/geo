
export function initMap(element) {
    const map = L.map(element).setView([25.9024289, -97.4981698], 13);

    const tiles = L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 19,
        attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
    }).addTo(map);

    return map;
}

export function addMarker(map, lat, lng, message) {
    const marker = L.marker([lat, lng]).addTo(map);
    marker.bindPopup(message).openPopup();
    return marker;
}
