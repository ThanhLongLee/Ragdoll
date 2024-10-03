
document.addEventListener('DOMContentLoaded', function () {
    // Logic to handle click events and animations
    document.querySelectorAll('.circle-outer').forEach(function (card) {
        card.addEventListener('click', function (e) {
            const rect = card.getBoundingClientRect();
            const x = e.clientX - rect.left - rect.width / 2;
            const y = e.clientY - rect.top - rect.height / 2;
            card.style.transform = `perspective(1000px) rotateX(${-y / 10}deg) rotateY(${x / 10}deg)`;
            setTimeout(() => {
                card.style.transform = '';
            }, 100);
            // Update points and handle clicks here
        });
    });
});
