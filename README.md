# ScanTerminal
## Conventions
In order to simplify the programming, the project is implemented under the following conventions.

1. Product's code is **case sensitive**, either a single letter or a single number, so that the scope is: **a-z | A-Z | 0-9**
2. Any error caused by **validation** will throw an **exception**.
3. Bulk Count should be no less than **2**.
4. Unit Price and Bulk Price cannot be **negative**.
5. Once a **nonexistent** Code is scanned, terminal will be in illegal state and **unavailable** until **Reset()** being called. Result will be **abandoned**.

## Design Trade-off
Bulk Price is abstracted to be **IBulkPriceHolder** interface. There are two ways working with **Product** class.

1. Create a new Class that implements this interface known as **BulkPriceProduct**.
2. Inject the instance of it into Product class as a **property**.

My implementation is 1 for Bulk Price being specific to each product. IBulkPriceHolder also means the class which implements it will do extra computation under the condition it specifies.

## Key Test Cases
Each class has its corresponding unit test class. The following test cases are important to prove the library works.
Describe here briefly for convenience of reviewing the code.

### Produc List

| Product Code  | Unit Price  | Bulk Price |
|:-------------: |:---------------:| :-------------|
| A              | $1.25           |   3 for $3.00 |
| B              | $4.25           |               |
| C              | $1.00           |$5 for a six-pack|
| D              | $0.75           |               |

###Test Case List

| TEST CASE  | Description |
|:-------------: |:---------------|
| ("ABCD", $7.25)| Required        | 
| ("CCCCCCC", $6)| Required        | 
| ("ABCDABA", $13.25)| Required           |
| ("AABCDCCCC", $12.5)| All Products without bulk price|
|("AABCDACCCCC", $13)| All Products with only bulk price|
|("AAACCCCCC", $8)| Bulk price only|
|("AABCCDAABCCDCCC", $20.25)|All Products with unit & bulk price|
|("AB C  D  ", $7.25)|Skip whitespace while scanning|
|("", 0)|Empty means no Product|
|("   ", 0)|Whitespace strings mean no Product|
|("aBCD", 0)|Invalid Product stops scanning and reset total to 0|
|("ABCd", 0)|Invalid Product stops scanning and reset total to 0|


        