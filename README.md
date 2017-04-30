This is a very simple demo of using Cookie Authentication is ASP.NET Core

It uses kind of a cheap hack for you to login, I was too lazy to make a login form for this.

To authentication, do:

http://localhost:5000/home/login?password=dumb

Where you replace localhost with your machine name or whatever is appropriate for where you are running.

It shows how to create policies and setup the Cookie Authentication Middleware in the Startup.cs file.  

In the HomeController you can see the code for the authentication and sign in.