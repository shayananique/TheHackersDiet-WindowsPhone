# The Hackers Diet application for Windows Phone
This application is (for the time being) a proof-of-concept application that uses concepts from [The Hacker's Diet](http://www.fourmilab.ch/hackdiet/) to explore web services provided by the [Parse platform](https://parse.com/).

There's still a lot to do. My goal is for the application to eventually explore the followin APIs ...

**Objects**

* Create objects (done)
* Retrive objects (in progress)
* Update objects (in progress)
* Query objects (done)
* Delete objects (in progress)

**Users**

* Sign-up users
* Login users

**Roles**

* Retrieving roles (e.g. lookup if user is an admin)

**Files**

* Upload file (e.g. take a picture and upload it)

No guarantees how much I'll do here.

## Using this project
Just a few notes:

1. I have forked <https://github.com/karlseguin/parse-dotnet> and set it up as an external remote for the Windows Phone application.

2. **Newtonsoft.Json.dll** is used for parsing the JSON objects. The assembly is in \resources\windowsphone.

3. Obviously you'll need the latest version of the Windows Phone SDK 7.1.