/**
 * DreamPos Web Report - Inactivity Auto-Logout System
 * Elegant client-side activity monitor and warning modal
 */
(function () {
    // Configuration
    var idleTimeSeconds = 0;
    var maxIdleSeconds = 5 * 60; // 5 minutes (300 seconds)
    var warningSeconds = 15; // Warning appears 15 seconds before logout
    var checkInterval;
    var countdownRemaining = warningSeconds;
    var modalInjected = false;

    // Reset idle timer on user activity
    function resetIdleTimer() {
        idleTimeSeconds = 0;
        if (modalInjected) {
            hideWarningModal();
        }
    }

    // Set up global activity listeners
    function initListeners() {
        var activityEvents = ['mousemove', 'keypress', 'mousedown', 'click', 'scroll', 'touchstart'];
        activityEvents.forEach(function (eventName) {
            document.addEventListener(eventName, resetIdleTimer, true);
        });

        // Run tick check every 1 second
        checkInterval = setInterval(checkIdleTime, 1000);
    }

    // Check idle progress every second
    function checkIdleTime() {
        // Only run check if the UserLogin_inv cookie exists
        if (document.cookie.indexOf('UserLogin_inv') === -1) {
            return;
        }

        idleTimeSeconds++;

        var threshold = maxIdleSeconds - warningSeconds;

        if (idleTimeSeconds >= maxIdleSeconds) {
            // Log out immediately
            logoutUser();
        } else if (idleTimeSeconds >= threshold) {
            // Show premium warning modal
            if (!modalInjected) {
                showWarningModal();
            }
            // Update countdown
            countdownRemaining = maxIdleSeconds - idleTimeSeconds;
            updateCountdownDisplay(countdownRemaining);
        }
    }

    // Dynamic injection of premium CSS
    function injectModalStyles() {
        if (document.getElementById('inactivity-modal-styles')) return;

        var css = `
            .inactivity-overlay {
                position: fixed;
                top: 0;
                left: 0;
                width: 100%;
                height: 100%;
                background: rgba(4, 39, 79, 0.45);
                backdrop-filter: blur(12px);
                -webkit-backdrop-filter: blur(12px);
                z-index: 100000;
                display: flex;
                align-items: center;
                justify-content: center;
                opacity: 0;
                transition: opacity 0.4s ease;
            }
            .inactivity-overlay.active {
                opacity: 1;
            }
            .inactivity-card {
                background: rgba(255, 255, 255, 0.85);
                border: 1px solid rgba(255, 255, 255, 0.4);
                box-shadow: 0 20px 50px rgba(0, 0, 0, 0.15);
                border-radius: 16px;
                width: 90%;
                max-width: 420px;
                padding: 30px;
                text-align: center;
                font-family: 'Source Sans Pro', -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
                transform: scale(0.9);
                transition: transform 0.4s cubic-bezier(0.34, 1.56, 0.64, 1);
            }
            .inactivity-overlay.active .inactivity-card {
                transform: scale(1);
            }
            .inactivity-icon {
                font-size: 48px;
                color: #fbc02d;
                margin-bottom: 20px;
                animation: pulse 1.5s infinite alternate;
            }
            .inactivity-title {
                color: #04274f;
                font-size: 22px;
                font-weight: 700;
                margin-bottom: 12px;
            }
            .inactivity-text {
                color: #555;
                font-size: 15px;
                line-height: 1.5;
                margin-bottom: 24px;
            }
            .inactivity-countdown-box {
                display: inline-flex;
                align-items: center;
                justify-content: center;
                width: 80px;
                height: 80px;
                border-radius: 50%;
                background: linear-gradient(135deg, #04274f 0%, #063d7a 100%);
                color: #fff;
                font-size: 32px;
                font-weight: 800;
                margin-bottom: 24px;
                box-shadow: 0 8px 20px rgba(4, 39, 79, 0.3);
                border: 3px solid #fbc02d;
            }
            .inactivity-btn-group {
                display: flex;
                gap: 12px;
                justify-content: center;
            }
            .inactivity-btn {
                padding: 12px 24px;
                font-size: 15px;
                font-weight: 600;
                border-radius: 8px;
                cursor: pointer;
                transition: all 0.3s;
                border: none;
                flex: 1;
            }
            .inactivity-btn-stay {
                background: #04274f;
                color: #fff;
                box-shadow: 0 4px 10px rgba(4, 39, 79, 0.2);
            }
            .inactivity-btn-stay:hover {
                background: #063d7a;
                transform: translateY(-2px);
                box-shadow: 0 6px 15px rgba(4, 39, 79, 0.3);
            }
            .inactivity-btn-logout {
                background: #ea4335;
                color: #fff;
                box-shadow: 0 4px 10px rgba(234, 67, 53, 0.2);
            }
            .inactivity-btn-logout:hover {
                background: #d33828;
                transform: translateY(-2px);
                box-shadow: 0 6px 15px rgba(234, 67, 53, 0.3);
            }
            @keyframes pulse {
                0% { transform: scale(1); }
                100% { transform: scale(1.1); }
            }
        `;

        var style = document.createElement('style');
        style.id = 'inactivity-modal-styles';
        style.appendChild(document.createTextNode(css));
        document.head.appendChild(style);
    }

    // Injects & displays the Warning Modal
    function showWarningModal() {
        injectModalStyles();

        var overlay = document.createElement('div');
        overlay.id = 'inactivity-overlay';
        overlay.className = 'inactivity-overlay';

        overlay.innerHTML = `
            <div class="inactivity-card">
                <div class="inactivity-icon"><i class="fas fa-exclamation-triangle"></i></div>
                <div class="inactivity-title">Sesi Anda Akan Berakhir</div>
                <div class="inactivity-text">Anda terdeteksi tidak aktif. Untuk keamanan data Anda, sistem akan keluar secara otomatis dalam:</div>
                <div class="inactivity-countdown-box" id="inactivity-countdown">${warningSeconds}</div>
                <div class="inactivity-btn-group">
                    <button class="inactivity-btn inactivity-btn-stay" id="inactivity-btn-stay">Lanjutkan Sesi</button>
                    <button class="inactivity-btn inactivity-btn-logout" id="inactivity-btn-logout">Keluar Sekarang</button>
                </div>
            </div>
        `;

        document.body.appendChild(overlay);
        modalInjected = true;

        // Trigger animation transition next frame
        setTimeout(function () {
            overlay.classList.add('active');
        }, 10);

        // Bind button events
        document.getElementById('inactivity-btn-stay').addEventListener('click', function (e) {
            e.stopPropagation();
            resetIdleTimer();
        });
        document.getElementById('inactivity-btn-logout').addEventListener('click', function (e) {
            e.stopPropagation();
            logoutUser();
        });
    }

    // Hides and removes the modal overlay
    function hideWarningModal() {
        var overlay = document.getElementById('inactivity-overlay');
        if (overlay) {
            overlay.classList.remove('active');
            setTimeout(function () {
                if (overlay.parentNode) {
                    overlay.parentNode.removeChild(overlay);
                }
                modalInjected = false;
            }, 400);
        }
    }

    // Updates countdown digit inside modal
    function updateCountdownDisplay(seconds) {
        var displayEl = document.getElementById('inactivity-countdown');
        if (displayEl) {
            displayEl.textContent = seconds;
        }
    }

    // Perform logout
    function logoutUser() {
        clearInterval(checkInterval);
        
        // Expire session and auth cookies client-side
        document.cookie = "UserLogin_inv=; expires=Thu, 01 Jan 1970; path=/; SameSite=Lax";
        document.cookie = ".ASPXFORMSAUTH=; expires=Thu, 01 Jan 1970; path=/; SameSite=Lax";
        
        // Expire all other paths just in case
        document.cookie = "UserLogin_inv=; expires=Thu, 01 Jan 1970; path=/DreamPos-WebReport; SameSite=Lax";
        
        // Also inform VB backend by hitting default page with logout query parameter
        window.location.href = "Default.aspx?logout=timeout";
    }

    // Initialize inactivity tracker when DOM is ready
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', initListeners);
    } else {
        initListeners();
    }
})();
