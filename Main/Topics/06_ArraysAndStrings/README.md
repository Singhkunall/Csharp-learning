# Topic 06: Arrays and Strings

## 📖 Theoretical Overview

### 1. Array Architectures
- **Single-Dimensional Array:** Contiguous block of memory holding elements of the same type (`int[] arr = new int[5];`).
- **Multidimensional (Rectangular) Array:** Fixed 2D/3D grid (`int[,] matrix = new int[3, 3];`).
- **Jagged Array (Array of Arrays):** An array whose elements are themselves independent arrays, allowing rows of differing lengths (`int[][] jagged = new int[3][];`).

### 2. String Mechanics & Immutability
- Strings in .NET are **immutable** reference types. Any modification (e.g. `s += " extra"`) leaves the original string intact on the heap and allocates a brand-new string.
- **`StringBuilder`:** A mutable buffer in `System.Text` that avoids repeated heap reallocations when appending in loops.

---

## 📊 Rectangular vs. Jagged Array Memory Layout

```mermaid
flowchart TD
    subgraph Rectangular_Matrix["Rectangular 2D Array: int[2, 3]"]
        R["Single Contiguous Heap Block: [0,0][0,1][0,2] | [1,0][1,1][1,2]"]
    end
    subgraph Jagged_Array["Jagged Array: int[2][]"]
        J["Root Array (Array of references)"]
        J --> R0["Sub-Array 0 (Length: 2): [10, 20]"]
        J --> R1["Sub-Array 1 (Length: 4): [30, 40, 50, 60]"]
    end
```
