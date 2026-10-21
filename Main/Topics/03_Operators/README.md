# Topic 03: Operators in C#

## 📖 Theoretical Overview

### 1. Categories of Operators
- **Arithmetic:** `+`, `-`, `*`, `/`, `%`
- **Relational / Comparison:** `==`, `!=`, `<`, `>`, `<=`, `>=`
- **Logical:** `&&` (Short-circuit AND), `||` (Short-circuit OR), `!` (NOT)
- **Bitwise:** `&` (AND), `|` (OR), `^` (XOR), `~` (Complement), `<<` (Left Shift), `>>` (Right Shift)
- **Null Operators:**
  - `??` (Null-Coalescing): Returns the left-hand operand if not null; otherwise evaluates right-hand.
  - `??=` (Null-Coalescing Assignment): Assigns the right-hand value only if the left-hand is null.
- **Type Inspection & Casting Operators:**
  - `is`: Checks if an object is compatible with a given type or matches a pattern.
  - `as`: Safely casts an object to a reference or nullable type; returns `null` instead of throwing `InvalidCastException`.
  - `typeof`: Gets the `System.Type` object for an explicit compile-time type.

---

## 📊 Operator Precedence Hierarchy

```mermaid
flowchart TD
    A["1. Primary: x.y, f(x), a[i], x++, x--, ?. , typeof"] --> B["2. Unary: +x, -x, !x, ~x, ++x, --x, (T)x, await"]
    B --> C["3. Multiplicative: *, /, %"]
    C --> D["4. Additive: +, -"]
    D --> E["5. Shift: <<, >>"]
    E --> F["6. Relational & Type-Testing: <, >, <=, >=, is, as"]
    F --> G["7. Equality: ==, !="]
    G --> H["8. Logical Bitwise: &, ^, |"]
    H --> I["9. Conditional Logical: &&, ||"]
    I --> J["10. Null-Coalescing: ??"]
    J --> K["11. Ternary Conditional: ?:"]
    K --> L["12. Assignment: =, +=, -=, *=, ??="]
```
