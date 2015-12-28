# decimal3
A ternary decimal type. This is a simple first draft. Not much has been done in the way of documentation.

## This class currently only processes integers.

# Using it

``` csharp
var five = Decimal3.Parse(5);

// Multiply by base3 numbers
var result = five * five - five;
Assert.AreEqual(20, five.Value);

// Multiply by base10 numbers
// base10 numbers are automatically converted to base3 numbers
result = five * 5 - 5;
Assert.AreEqual(20, five.Value);
```
