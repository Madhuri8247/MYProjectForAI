using Microsoft.Build.Tasks.Deployment.Bootstrapper;
using OnlineNews.Models;
using Newtonsoft.Json; 

namespace OnlineNews.Middleware
{
    // Middleware for initializing an empty shopping cart in the session.
    public class SessionInitializationMiddleware
    {
        private readonly RequestDelegate _next;                                      // Next object in the pipeline.
        public SessionInitializationMiddleware(RequestDelegate next)
        {
            _next = next;                                                            // Assign the next object.
        }
        public async Task InvokeAsync(HttpContext context)
        {
            // Check if the session contains "CartItems"; initialize if missing.
            if (context.Session.GetObject<List<Models.Product>>("CartItems") == null)
            {
                context.Session.SetObject("CartItems", new List<Models.Product>());           // Create empty cart.
            }
            await _next(context);                                                    // Pass control to the next session object.
        }
    }
    public static class SessionExtensions                                            // Extensions for handling complex objects in the session.
    {  
        public static void SetObject<T>(this ISession session, string key, T value)  // Serialize and store an object in the session.
        {
            var json = JsonConvert.SerializeObject(value);                           // Convert object to JSON.
            session.SetString(key, json);                                            // Save JSON string in the session.
        }
        public static T GetObject<T>(this ISession session, string key)              // Retrieve and deserialize an object from the session.
        {
            var json = session.GetString(key); // Get JSON string by key.
            return json == null ? default : JsonConvert.DeserializeObject<T>(json);  // Convert back to object.
        }
    }
}

//  In the SetObject<T> method, T is a generic type parameter. It means that the method can work with any data type, making it flexible and reusable.

//  Explanation of Generics:
//  T as a Type Parameter:

//  The symbol T is a convention in C# (from the word Type), representing any type.
//  When you call the method, the compiler replaces T with a specific data type, such as List<Movie> or int.
//  How does SetObject<T> work?

//  It allows you to store any object (regardless of its type) in the session, for example, a list of movies, numbers, or even entire classes.
//  When the method is called, T becomes the type of the object you want to store.

//  EXAMPLES:
//  var movies = new List<Movie>();
//  session.SetObject("CartItems", movies);        // Here T = List<Movie>

//  int count = 5;
//  session.SetObject("MovieCount", count);        // Here T = int