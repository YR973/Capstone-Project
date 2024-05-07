// Get the search input field and results container
var searchInput = document.getElementById('searchInput');
var searchResults = document.getElementById('searchResults');

// Add event listener for input event on search input field
searchInput.addEventListener('input', function() {
    var query = this.value.trim().toLowerCase(); // Trim leading and trailing whitespace and convert to lowercase

    // Clear previous search results
    searchResults.innerHTML = '';

    // If query is not empty, filter example suggestions
    if (query.length > 0) {
        var matchingSuggestions = ['amine', 'youssef', 'burak'].filter(function(suggestion) {
            return suggestion.toLowerCase().startsWith(query);
        });

        // Display at most 2 matching suggestions
        for (var i = 0; i < Math.min(matchingSuggestions.length, 2); i++) {
            var suggestionDiv = document.createElement('div');
            suggestionDiv.textContent = matchingSuggestions[i];
            suggestionDiv.classList.add('suggestion');
            suggestionDiv.addEventListener('click', function() {
                // Replace search input value with suggestion
                searchInput.value = this.textContent;
                // Clear search results
                searchResults.innerHTML = '';
            });
            searchResults.appendChild(suggestionDiv);
        }

        // Display the search results container
        searchResults.style.display = 'block';
    } else {
        // Hide search results if query is empty
        searchResults.style.display = 'none';
    }
});

// Hide search results when clicking outside the search bar
document.addEventListener('click', function(event) {
    if (!searchInput.contains(event.target)) {
        searchResults.innerHTML = '';
        searchResults.style.display = 'none';
    }
});