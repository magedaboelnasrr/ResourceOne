// ===================================
// Timeline Section JavaScript
// Swiper-like functionality
// ===================================

document.addEventListener('DOMContentLoaded', function() {
    // Get DOM elements
    const track = document.querySelector('.timeline-track');
    const items = document.querySelectorAll('.timeline-item');
    const prevBtn = document.querySelector('.timeline-nav-prev');
    const nextBtn = document.querySelector('.timeline-nav-next');
    const wrapper = document.querySelector('.timeline-wrapper');
    
    // Configuration
    let currentIndex = 0;
    let itemsPerView = 4;
    const totalItems = items.length;
    const transitionSpeed = 500; // ms
    
    // Calculate items per view based on screen width
    function calculateItemsPerView() {
        const wrapperWidth = wrapper.offsetWidth;
        
        if (wrapperWidth >= 1200) {
            itemsPerView = 4;
        } else if (wrapperWidth >= 992) {
            itemsPerView = 3;
        } else if (wrapperWidth >= 768) {
            itemsPerView = 2;
        } else {
            itemsPerView = 1;
        }
    }
    
    // Get item width including margins/gaps
    function getItemWidth() {
        if (items.length === 0) return 0;
        const item = items[0];
        const style = window.getComputedStyle(item);
        const width = item.offsetWidth;
        const marginLeft = parseFloat(style.marginLeft) || 0;
        const marginRight = parseFloat(style.marginRight) || 0;
        return width + marginLeft + marginRight;
    }
    
    // Get max index based on visible items
    function getMaxIndex() {
        return Math.max(0, totalItems - itemsPerView);
    }
    
    // Update timeline position
    function updateTimeline() {
        calculateItemsPerView();
        const itemWidth = getItemWidth();
        const maxIndex = getMaxIndex();
        
        // Clamp current index
        currentIndex = Math.max(0, Math.min(currentIndex, maxIndex));
        
        // Calculate translation
        const translateX = -(currentIndex * itemWidth);
        track.style.transform = `translateX(${translateX}px)`;
        
        // Update button states
        updateButtonStates();
    }
    
    // Update button disabled states
    function updateButtonStates() {
        const maxIndex = getMaxIndex();
        prevBtn.disabled = currentIndex === 0;
        nextBtn.disabled = currentIndex >= maxIndex;
    }
    
    // Go to next slide
    function goToNext() {
        const maxIndex = getMaxIndex();
        if (currentIndex < maxIndex) {
            currentIndex++;
            updateTimeline();
        }
    }
    
    // Go to previous slide
    function goToPrevious() {
        if (currentIndex > 0) {
            currentIndex--;
            updateTimeline();
        }
    }
    
    // Event listeners for buttons
    nextBtn.addEventListener('click', goToNext);
    prevBtn.addEventListener('click', goToPrevious);
    
    // Keyboard navigation
    document.addEventListener('keydown', (e) => {
        if (e.key === 'ArrowRight') {
            goToNext();
        } else if (e.key === 'ArrowLeft') {
            goToPrevious();
        }
    });
    
    // Touch/swipe support
    let touchStartX = 0;
    let touchEndX = 0;
    let isSwiping = false;
    
    track.addEventListener('touchstart', (e) => {
        touchStartX = e.changedTouches[0].screenX;
        isSwiping = true;
    }, { passive: true });
    
    track.addEventListener('touchmove', (e) => {
        if (!isSwiping) return;
        touchEndX = e.changedTouches[0].screenX;
    }, { passive: true });
    
    track.addEventListener('touchend', () => {
        if (!isSwiping) return;
        isSwiping = false;
        handleSwipe();
    });
    
    function handleSwipe() {
        const swipeThreshold = 50;
        const diff = touchStartX - touchEndX;
        
        if (Math.abs(diff) > swipeThreshold) {
            if (diff > 0) {
                goToNext();
            } else {
                goToPrevious();
            }
        }
    }
    
    // Mouse drag support
    let isDragging = false;
    let startX = 0;
    let startTranslate = 0;
    
    track.addEventListener('mousedown', (e) => {
        isDragging = true;
        startX = e.pageX;
        track.style.cursor = 'grabbing';
        track.style.transition = 'none';
    });
    
    document.addEventListener('mousemove', (e) => {
        if (!isDragging) return;
        e.preventDefault();
    });
    
    document.addEventListener('mouseup', (e) => {
        if (!isDragging) return;
        
        isDragging = false;
        track.style.cursor = 'grab';
        track.style.transition = `transform ${transitionSpeed}ms cubic-bezier(0.4, 0, 0.2, 1)`;
        
        const endX = e.pageX;
        const diff = startX - endX;
        const dragThreshold = 50;
        
        if (Math.abs(diff) > dragThreshold) {
            if (diff > 0) {
                goToNext();
            } else {
                goToPrevious();
            }
        } else {
            updateTimeline();
        }
    });
    
    // Handle window resize
    let resizeTimeout;
    window.addEventListener('resize', () => {
        clearTimeout(resizeTimeout);
        resizeTimeout = setTimeout(() => {
            updateTimeline();
        }, 150);
    });
    
    // Initialize
    track.style.cursor = 'grab';
    track.style.transition = `transform ${transitionSpeed}ms cubic-bezier(0.4, 0, 0.2, 1)`;
    updateTimeline();
});
