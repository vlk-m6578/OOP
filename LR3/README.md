# Student Record Management System 🎓

**Developer**: ЛЕБЕДЕВА МИЛАНА 353504
**Description**: A console-based student management system developed in C# using layered architecture and design patterns. Supports adding, editing, and viewing student records with integrated motivational quotes from an external API. The project demonstrates clean code organization, validation, and API integration.

---

## Features ✨
- **Student Management**: Add, edit, view students.
- **Input Validation**: Ensure valid names (non-empty) and grades (0-100).
- **JSON Storage**: Persist data locally in `students.json`.
- **Motivational Quotes**: Fetch quotes from `quotable.io` API after adding a student.
- **Layered Architecture**: Clear separation of concerns (Presentation, Application, Domain, DataAccess).
- **Design Patterns**: Command, Adapter, Factory Method.

---

## Functional Requirements 📋

### 1. Student Operations 📚
| Operation          | Description                                                                 |
|--------------------|-----------------------------------------------------------------------------|
| **Add Student**    | Input name/grade → Validate → Save to JSON → Display motivational quote.    |
| **Edit Student**   | Select by ID → Update name/grade → Validate → Save changes.                 |
| **View Students**  | Display all students in a formatted list (ID, Name, Grade).                 |

### 2. Validation Rules 🛡️
| Field    | Rules                          |
|----------|--------------------------------|
| **Name** | - Non-empty string             |
| **Grade**| - Integer between 0 and 100    |

### 3. API Integration 🌐
- **Trigger**: After successful student addition.
- **API Used**: `quotable.io/random`.
- **Output Format**:  
  `"Quote text" — Author`

### 4. Storage 🔄
| Storage Type | Format | Location         |
|--------------|--------|------------------|
| **Local**    | JSON   | `students.json`  |

---

## Class Diagram 🧩
### Command Pattern

### Adapter Pattern

### Factory Method Pattern
