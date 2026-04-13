
(function() {
    const mobileMenuButton = document.getElementById('mobile-menu-button');
    const mobileMenu = document.getElementById('mobile-menu');

    if (mobileMenuButton && mobileMenu) {
        mobileMenuButton.addEventListener('click', () => {
            const isHidden = mobileMenu.classList.contains('hidden');
            if (isHidden) {
                mobileMenu.classList.remove('hidden');
                mobileMenu.classList.add('block');
            } else {
                mobileMenu.classList.add('hidden');
                mobileMenu.classList.remove('block');
            }
        });

        document.addEventListener('click', (event) => {
            if (!mobileMenu.contains(event.target) && !mobileMenuButton.contains(event.target)) {
                mobileMenu.classList.add('hidden');
                mobileMenu.classList.remove('block');
            }
        });
    }
})();

function handleSubscribe(e) {
    e.preventDefault();
    const email = document.getElementById('subscribeEmail');
    if (email && /^\S+@\S+\.\S+$/.test(email.value)) {
        showSubscribePopup();
        email.value = '';
    }
}

function showSubscribePopup() {
    const popup = document.getElementById('subscribePopup');
    if (popup) {
        popup.classList.remove('hidden');
        popup.setAttribute('aria-hidden', 'false');
    }
}

function closeSubscribePopup() {
    const popup = document.getElementById('subscribePopup');
    if (popup) {
        popup.classList.add('hidden');
        popup.setAttribute('aria-hidden', 'true');
    }
}

document.addEventListener('click', (e) => {
    const popup = document.getElementById('subscribePopup');
    if (popup && !popup.classList.contains('hidden') && !popup.contains(e.target) && !e.target.closest('button[onclick="showSubscribePopup()"]')) {
        closeSubscribePopup();
    }
});