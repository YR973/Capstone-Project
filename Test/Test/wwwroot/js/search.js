// // document.getElementById('search-input').addEventListener('keyup', function() {
// //     var query = this.value;
// //     if (query.length > 0) { // Only send request if query is longer than 0 characters
// //         fetch('/SuggestionFetch?query=' + encodeURIComponent(query))
// //             .then(response => response.json())
// //             .then(data => {
// //                 var suggestions = document.getElementById('suggestions');
// //                 suggestions.innerHTML = '';
// //                 data.forEach(item => {
// //                     var div = document.createElement('div');
// //                     div.textContent = item.toString(); // Convert integer to string for display
// //                     suggestions.appendChild(div);
// //                 });
// //                 suggestions.style.display = 'block';
// //             });
// //     } else {
// //         document.getElementById('suggestions').style.display = 'none'; // Hide suggestions box if input is empty
// //     }
// // });
//
// document.getElementById('search-input').addEventListener('keyup', function() {
//     var query = this.value;
//     if (query.length > 0) { // Only send request if query is longer than 0 characters
//         fetch('/SuggestionFetch?query=' + encodeURIComponent(query))
//             .then(response => {
//                 if (!response.ok) {
//                     throw new Error('Network response was not ok');
//                 }
//                 return response.json();
//             })
//             .then(data => {
//                 console.log(data); // Log the data to the console
//                 var suggestions = document.getElementById('suggestions');
//                 suggestions.innerHTML = '';
//                 data.forEach(item => {
//                     var div = document.createElement('div');
//                     div.textContent = item.toString(); // Convert integer to string for display
//                     suggestions.appendChild(div);
//                 });
//                 suggestions.style.display = 'block';
//             })
//             .catch(error => {
//                 console.error('There has been a problem with your fetch operation:', error);
//             });
//     } else {
//         document.getElementById('suggestions').style.display = 'none'; // Hide suggestions box if input is empty
//     }
// });