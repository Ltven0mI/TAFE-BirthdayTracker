![Logo of the project](https://github.com/Ltven0mI/TAFE-BirthdayTracker/blob/master/icon.png)

# BirthdayTracker
> Tracking friends, clients and family names, likes, dislikes, birth-dates

BirthdayTracker aims to provide a listing per-month of upcoming birthdays
including details as to individual's likes and dislikes -
assisting with the selection of presents.

## Installing / Getting started

### Requirements
- .NET Core SDK (https://dotnet.microsoft.com/download)
- CsvHelper (https://joshclose.github.io/CsvHelper)
  
### 1. Download/Clone The Project
Either download the repo source code to a folder on your local machine,
or clone the repository to a folder on your local machine using:
```shell
> git clone https://github.com/Ltven0mI/TAFE-BirthdayTracker.git
```

### 2. Install The .NET Core SDK
Download and install the .NET Core SDK from [here](https://dotnet.microsoft.com/download)

### 3. Restore The Project
In order to ensure all required packages are installed, run the following
command inside the folder containing the solution (.sln) file:
```shell
> dotnet restore
```

### 4. Running The Application
Run the following command from inside the solution folder:
```shell
> dotnet run
```

## Generating Documentation
This project utilizes [DocFX](https://dotnet.github.io/docfx/) for generating
static site documentation.

If you wish to generate the documentation yourself, first follow the steps in
[Installing / Getting started](#installing--getting-started) then procede with
the steps below.

### Requirements
- DocFX (https://dotnet.github.io/docfx/)

### 1. Install DocFX and add to PATH
Follow the steps on the DocFX site [Use DocFX as a command-line tool](https://dotnet.github.io/docfx/tutorial/docfx_getting_started.html#2-use-docfx-as-a-command-line-tool)
to install and add DocFX to your PATH.

### 2. Build the Website
Run the following command from inside `BirthdayTracker-Docs/docfx_project/`
```shell
> docfx
```

### 3. Preview the Website
Run the following command from inside `BirthdayTracker-Docs/docfx_project/`
```shell
> docfx serve _site
```
The website should now be accessible from your web-browser at `localhost:8080`

## Features

The BirthdayTracker application offers many features, some of which are:
* The tracking of friends/family names, likes, dislikes, and birth-dates.
* Searching for a friend/family member by name.
* Displaying all friends/family members who's birthday falls on a specified month.
* Entering details for a new friend/family member.
* Updating details of a friend/family member.
* Deletion of friends/family members.

## Contributing

If you wish to contribute to this project, please fork the repository and use
a feature branch. Pull requests are warmly welcome.

When contributing please ensure you are following the programming standards
outlined in `CONTRIBUTING.md`.

## Links

- Project homepage: https://github.com/Ltven0mI/TAFE-BirthdayTracker
- Repository: https://github.com/Ltven0mI/TAFE-BirthdayTracker
- Issue tracker: https://github.com/Ltven0mI/TAFE-BirthdayTracker/issues
  - In case of sensitive bugs like security vulnerabilities, please contact
    wade.xr@explodybeans.net directly instead of using issue tracker.
    We value your effort to improve the security and privacy of this project!


## Licensing

The code in this project is licensed under an [MIT](https://github.com/Ltven0mI/TAFE-BirthdayTracker/blob/master/LICENSE.txt) license.