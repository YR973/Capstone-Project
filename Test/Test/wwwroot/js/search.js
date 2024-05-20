// Get the search input field and results container
var searchInput = document.getElementById('searchInput');
var searchResults = document.getElementById('searchResults');

searchInput.addEventListener('input', function() {
    var query = this.value.trim().toLowerCase();
    searchResults.innerHTML = '';

    if (query.length > 0) {
        fetch('/api/SearchApi/suggest?query=' + encodeURIComponent(query))
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
            .then(data => {
                // Process the data
                for (var i = 0; i < Math.min(data.length, 2); i++) {
                    var suggestionDiv = document.createElement('div');
                    suggestionDiv.textContent = data[i];
                    suggestionDiv.classList.add('suggestion');
                    suggestionDiv.addEventListener('click', function() {
                        searchInput.value = this.textContent;
                        searchResults.innerHTML = '';
                    });
                    searchResults.appendChild(suggestionDiv);
                }
                searchResults.style.display = 'block';
            })
            .catch(error => {
                console.error('Error fetching suggestions:', error);
            });
    } else {
        searchResults.style.display = 'none';
    }
});

document.addEventListener('click', function(event) {
    if (!searchInput.contains(event.target)) {
        searchResults.innerHTML = '';
        searchResults.style.display = 'none';
    }
});
