# Document Editor 📝

**Developer**: ЛЕБЕДЕВА МИЛАНА (353504)  
**Description**: A console-based document editor developed in C# using key OOP patterns. Supports creation, editing, formatting and multi-format saving of documents. The app provides flexibility, stability, and personalization through a system of roles, settings, and notifications.

---

## Features ✨
- **Document Management** create, open, save (TXT/JSON/XML) and delete.
- **Text formatting** bold, italic, underline.
- **Undo/Redo system** undo and redo actions.
- **User Roles** viewer(read-only), editor, and administrator modes.
- **Multiformat** conversion between formats (Markdown, JSON, XML.
- **Storage** local files, cloud, database.
- **Document editing history and notifications** notifications about changes and the ability to view the history.
- **Settings** themes for the document editor.

---

## Functional Requirements 📋

### 1. Authentication & Roles 🔐
### Log in to the system:
- **Viewer/Editor**: Any username (without password)
- **Admin**:
  - Username: admin
  - Password: `parol` (the only administrator in the system)

### 2. Document Management 📂
### Local Storage:
- **Document creation**:
  - Formats: `.txt`, `.md`
- **Opening documents**:
  - Support: `.txt`, `.md` (only from current storage)
- **Deleting documents**
- **Document Search**:
  - Word/phrase search

### Сохранение документов:
| Формат файла | Доступные места сохранения         |
|--------------|------------------------------------|
| **.txt**     | Локальное хранилище, БД, OneDrive  |
| **.md**      | Локальное хранилище, БД, OneDrive  |

| Формат экспорта | Поддерживаемые форматы              |
|-----------------|-------------------------------------|
| **.txt**        | TXT, JSON, XML                      |
| **.md**         | TXT, MARKDOWN, JSON, XML            |


### 3. Editing & Formatting ✏️
### Common features (for all formats):
- **Basic editing**:
  - Entering/deleting/changing text
  - Text selection: `Shift + arrows`
  - Clipboard:
    - Copy: `Ctrl+C`
    - Insert: `Ctrl+V`
    - Cut: `Ctrl+X`
- **Undo/Redo**:
  - Undo: `Ctrl+Z` (multi-level)
  - Repeat: `Ctrl+Y`

### Specific features for .md:
- **Formatting**:
  - Bold: `Ctrl+B' → `**text**`
  - Italics: `Ctrl+I` → '*text*`
  - Crossed out: `Ctrl+U' → `~~text~~`
- **Preview mode**:
  - Activation: `Ctrl+F12`
  - Preview Mode

### 4. Roles & Permissions 👥
### Viewer (Read-only):
- Viewing documents
- Editor's theme selection

### Editor (Full rights):
- All Viewer functions
- Editing/saving documents
- View your change history

### Admin (Super rights):
- All Editor functions
- **User Management**:
  - Role reversal (Viewer , Editor)
  - Blocking/unblocking documents for users
  - View the list of:
    - Active users
    - Blocked documents

### 5. System Settings ⚙️
### Topics:
- Available to all roles
- Options: Black / DarkBlue / Blue / DarkYellow / Yellow / DarkCyan / Cyan / DarkMagenta / Magenta

### Revision history:
- **Editor**: Sees only its own history
- **Admin**: Full access to the entire history
- Record format:
=== History of changes ===
[21:44:03] username changed the text in the document 'document_name'
---

## Class Diagram 🧩
###Command Pattern

###Strategy Pattern

###Observer Pattern

###Adapter Pattern

###Singleton Pattern

###Factory Pattern

###Decorator Pattern
