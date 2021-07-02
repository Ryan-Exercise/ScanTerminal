# ScanTerminal
## Conventions
In order to simplify the programming, the project is implemented under the following conventions.

1. Product's code is either a single letter or a single number, so that the scope is: **a-z | A-Z | 0-9**
2. Any error caused by **validation** will throw an **exception**.
3. Bulk Count should be no less than **2**.
4. Unit Price and Bulk Price cannot be **negative**.
5. Once a **nonexistent** Code is scanned, terminal will be in illegal state and **unavailable** until **Reset()** being called. Result will be **abandoned**.