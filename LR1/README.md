# Console Paint 🎨

**Developer**: ЛЕБЕДЕВА МИЛАНА (353504)  
**Description**: A simple Paint application developed in C# using object-oriented programming (OOP) principles. This console-based tool allows users to create, edit, and manage geometric shapes with intuitive controls. The application is designed to handle all user inputs gracefully, ensuring stability and a smooth experience.

---

## Features ✨
- **Draw** customizable geometric shapes (circle, rectangle, triangle) with borders and fill.
- **Delete** shapes by name from a list.
- **Move** shapes along the X and Y axes.
- **Save/Load** your canvas to/from a file.
- **Undo/Redo** actions to correct mistakes or revisit previous states.

---

## Functional Requirements 📋

### 1. Drawing Shapes
- **Border & Fill**: Customize shapes using two characters: one for the border and one for the fill (use a space ` ` for no fill).
- **Centered Placement**: All shapes are drawn at the center of the canvas, ready for further actions.
  - **Circle**: Specify a radius (>1) and a name.
  - **Rectangle**: Specify length and height (>1) and a name.
  - **Triangle**: Specify three side lengths (>1) and a name. 

### 2. Deleting Shapes
- View a list of all drawn shapes by name and delete any selected figure.

### 3. Moving Shapes
- Select a shape from the list and move it horizontally (X-axis) or vertically (Y-axis).

### 4. Saving & Loading
- Save the current canvas to a file or load a previously saved canvas. For these actions, a single file is used, the path to which is indicated on the screen.

### 5. Undo/Redo Actions
- Undo the last successful action or redo a previously undone action affecting the canvas or shapes.

---

## Class Diagram 🧩
