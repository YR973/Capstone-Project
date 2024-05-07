using Microsoft.AspNetCore.Mvc;

namespace Test;

public class SuggestionFetch
{
   
        public int[] OnGet(string query)
        {
            var suggestions = PythonClass.Suggest(query);
            int[] ids = new int[2];
            for (int i = 0; i < 2; i++)
            {
                ids[i] = int.Parse(suggestions[i]);
            }
            return ids;
        }
    
}