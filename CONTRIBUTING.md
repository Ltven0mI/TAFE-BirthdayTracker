# BirthdayTracker Contribution Guide

When writing code for this project the following programming standards and
conventions should be followed at all times.
If required, an examption to these standards may be granted by your supervisor.

## Class / Variable Naming Conventions

### Instance / Member Variables

Use ['camelCase'](https://www.chaseadams.io/posts/most-common-programming-case-types/#camelcase)
which "``must (1) start with a lowercase letter and (2) the first letter of every new subsequent word has its first letter capitalized and is compounded with the previous word.``"
```csharp
int minFriends = 0;
int maxFriends = 2;
```

### Properties

Use ['PascalCase'](https://www.chaseadams.io/posts/most-common-programming-case-types/#pascalcase)
which "``has every word starts with an uppercase letter (unlike camelCase in that the first word starts with a lowercase letter).``"
```csharp
int MinFriends { get; set; } = 0;
int MaxFriends { get; set; } = 2;
```

### Constant / Static Variables

Use ['UPPER_CASE_SNAKE_CASE'](https://www.chaseadams.io/posts/most-common-programming-case-types/#upper_case_snake_case)
which "``is replacing all the spaces with a "_" and converting all the letters to capitals.``"
```csharp
const int MIN_FRIENDS = 0;
static int MAX_FRIENDS = 2;
```

### Methods

Use ['PascalCase'](https://www.chaseadams.io/posts/most-common-programming-case-types/#pascalcase)
which "``has every word starts with an uppercase letter (unlike camelCase in that the first word starts with a lowercase letter).``"
```csharp
int GetMinFriends() {};
int GetMaxFriends() {};
```

### Class Names

Use ['PascalCase'](https://www.chaseadams.io/posts/most-common-programming-case-types/#pascalcase)
which "``has every word starts with an uppercase letter (unlike camelCase in that the first word starts with a lowercase letter).``"
```csharp
class Friend {}
class FriendList : List<Friend> {}
```

## Comments

### File / Class Header Comments

All class files must have a commented file header which contains the name of
the file, purpose, author's name, version control number, date, and
testing notes (if any).
```csharp
/**********************************************************/
// Filename: Customer.cs
// Purpose: To provide template for customer instances
// and store name, address, phone, balance data
// Author: Hans Telford
// Version: 1.0
// Date: 03-Mar-2020
// Tests: Unit test 1 and 2 completed 08-May-2020
/**********************************************************/
```

### Method Header Comments

All methods must have a commented method header which contains the
method signature, purpose, inputs, and outputs.
```csharp
/**********************************************************/
// Method: public bool IsValid (String name)
// Purpose: validates a name suitable for the application.
// - optional extra purpose line.
// Returns: true if name meets requirements
// Returns: false if name does not meet requirements
// Inputs: String name
// Outputs: bool
/**********************************************************/
```

### Source Code Comments

Comments in the source code should be avoided when possible.
Try to maintain readable and un-ambiguous code at all times.

Only include source code comments when the outcome of a block of code is not
instantly comprehensible.

### Tabs / Spacing

Appropriate tab indentations and spacing are to be used throughout to aid
readability and file management.

## Try / Catch Blocks

Appropriate try{} and catch{} blocks are to be used for the coding of database
connections, file i/o operations, error logging, or attempting any numeric
conversions from string values.

## User Input Sanitizing

All user inputs must be sanitized and validated to ensure accuracy and avoid
exceptions / security vulnerabilities.


