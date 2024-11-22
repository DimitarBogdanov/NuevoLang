NuevoLang is a hobby project IR-compiled scripting language.
It has a Lua-like (but not quite) syntax and dynamic typing.
Since the runtime is written in C#, Nuevo is inherently garbage collected.

```
module :: App

main = function() {
    Sys.println("Hello, world!")
}
```

## Basic concepts
### 1. Modules
Modules are symbol namespaces. They are defined as the first statement of each Nuevo file.
```
module :: App
```

### 2. Functions
Functions are an integral part of the language. They take in parameters and optionally return values.
Functions themselves are first-class objects and are declares by being assigned to a name.
```
main = function() {}
```

### 3. Control flow
#### 3.1. If statements
```
if x == 0 {
    // ...
} elseif x == 1 {
    // ...
} else {
    // ...
}
```
#### 3.2. For loops
For loops function as a range operator.
```
for x = 1, 5 {
    // 1, 2, 3, 4, 5
}
```
They can optionally be given a `step`:
```
for x = 10, 5, -1 {
    // 10, 9, 8, 7, 6, 5
}
```
Assuming the variable is called `x` and the limit (second value) is called `limit`:
```
forLoopIteratesWhile = function(x, limit, step) {
    if step < 0 {
        return x >= limit
    } else {
        return x <= limit
    }
}
```

#### 3.3. While loops
```
while true {
    // ...
}
```

#### 3.4. Handle blocks
See 4.7. Result

### 4. Values (expressions)
#### 4.1. Number
One unifying type for integer and decimal numbers, represented as float64.
```
x = 1
x = 1.0
```
#### 4.2. String
One type for all character-based operations, represented as UTF-16 characters.
```
x = "Hello, world!"
```
#### 4.3. Boolean
You know what a boolean is. Uses lowercase `true` and `false`.
```
x = true
x = false
```
#### 4.4. Null
Non-value.
```
x = null
```
#### 4.5. Dict
Represents a HashMap. Keys and values can be of any type. Created with curly braces.
When defining items, square brackets [] can be used to use literal types for keys, otherwise strings are inferred.
```
x = {
    ["Foo"] = 10,
    Bar = 20,
    Hi = function() {}
}

x["Foo"] = x["Bar"]
x.Hi()
x.Bar = null
```
#### 4.6. List
Represents an untyped dynamic array. Defined with square brackets. Lists are 0-indexed.
Indexes to undefined elements are not allowed.
```
x = [ 1, 2, 3 ]
x[0] = 3
x[2] = 1
```
#### 4.7. Result
Represents the result of a function. Used for error handling (see 7.)
```
iFailSometimes = function(x) {
    if x < 5 {
        return Result.ok("All good!")
    } else {
        return Result.err("Bad number!")
    }
}

main = function() {
    res = iFailSometimes(1)
    handle res {
        ok :: msg { // msg is a variable
            Sys.println("Ok: " + msg)
        }
        
        err :: msg { // also a variable here
            Sys.println("Error ocurred!")
            Sys.println(msg)
        }
    }
}
```
##### Notes:
* Defining the variable for `ok` and `err` is not required.
* Defining all scenarios is not required in a `handle` block (i.e. you can define only `ok`, only `err`, or both).

### 5. Operators
#### 5.1. Arithmetic
`+`, `-`, `*`, `/` - addition, subtraction, multiplication, division

`^` - power operator

#### 5.2. Comparison
`==`, `!=` - equal, not equal

`>`, `<`, `>=`, `<=` - greater than, less than, GT or equal, LT or equal

#### 5.3. Logical
`&&` - and

`||` - or

`!` - not (unary)

#### 5.4. Assignment
`=` - assignment to symbols, dictionary or list values

`+=`, `-=`, etc. - compound assignment with arithmetic operators

#### 5.5. If expression
Used as a ternary operator.
```
drinkingAge = if country == "USA" 21 else 18

// this is obviously arbitrary
drivingAge = if country == "USA" 16
             elseif countryInEu 17
             else 18
```

### 6. Error handling
See 4.7. Result for standard error handling.

For critical situations, which require the program to be shut down, `Result.panic` can be used:

```
main = function() {
    Result.panic("SIR, WE HAVE BEEN INFILTRATED!")
    
    Sys.println("Hello world!") // won't run, app is shut down
}
```

`panic` can be suppressed via a `handle` block when necessary, however it is not advised. `Resut.panic` should be used only when absolutely necessary.

The app can also be shut down with `Sys.exit()`, which cannot be suppressed.

### 7. Standard library
TODO

