# Automated Component Test Writer
This is an ASP.NET web application I made for my Senior Project at the University of Mississippi. It is intended to assist developers with the task of writing component integration tests for their web applications. The application is open to extension and free to use without any royalties necessary as it is published under the MIT license, and even though I do not require you credit me, I appreciate the credit if you feel the credit is due.

# What is it?
This is a Visual Studio solution that you can use or extend as you desire. You will need ASP.NET for .NET Framework 4.7.2, IIS express, and Rosyln. IIS must be enabled on your machine to run it on a local server.

It is a C# source code generator that generates code for integration tests that test HTTP response codes at endpoints as well as their expected messages, and Data Transfer Objects, which use the Newtonsoft JSON API to serialize the object and send it to the endpoint. The source code is being generated using the .NET CodeDOM API, which as of the time of writing this sentence is only available under .NET Framework and not .NET Core.
