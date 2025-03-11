# Financial Tracker (Программное средство для учёта финансов) 📄
**Developer**: ЛЕБЕДЕВА МИЛАНА, Группа 353504

## Description ✨
A console application for personal and joint finance accounting with support:
- Multi-user accounts
- Personal and shared accounts
- Categories of expenses/income
- Budgeting with limits
- Report generation

## Class Diagram 🧩
![Class Diagram]() 

## Functional Requirements 📋


### 1. User Management
- **Registration**: The user creates an account by specifying a username, email, and password.
  - **Login**: unique, minimum 6 characters.
  - **Email**: correct format.
  - **Password**: minimum 8 characters, including numbers and letters. 
  - **Password Storage**: hashing algorithm. 
- **Log in**: User authentication by login and password.
  - **Data Verification**: comparing the hash of the entered password with the saved.
  - **Error Handling**: invalid login/password, inactive account.
- **Password Recovery**: password reset.
  - **Generation of a code**: the ability to enter a new password after checking the code.


### 2. Account management
- **Personal accounts**: Creation, editing and deletion of personal accounts (for example, "Cash", "Card").
  - **Account creation**: specifying the account name, initial balance (0 by default).
  - **Edit**: change the name.
  - **Delete**: only if the balance is 0.
- **General accounts**: Joint account management with other users.
  - **Inviting participants**: by email or login, notification of the invited.
  - **Access rights**: all participants can add/view transactions, the account creator can delete the participants.
  - **Change history**: logging operations.


### 3. Transaction Management
- **Adding a Transaction**: Record income or expense.
  - **Amount**: positive for income, negative for expense.
  - **Date**: current by default.
  - **Category**: selection from the list. 
- **Editing a transaction**: Change of transaction data.
  - **Access rights**: only the creator of the transaction can change it.
  - **Change history**: fixing previous values.
- **Deleting a transaction**: Deleting an error transaction.
  - **Adjustment**: the account balance is adjusted when deleted.
  - **Tag**: the transaction is marked as deleted.


### 4. Managing categories and budgets
- **Categories**: Creating categories (for example, "Transport").
  - **System Categories**: preset (Food, Transport, Housing).
  - **Custom categories**: the ability to add your own category.
  - **Category**: selection from the list.
- **Budgets**: Setting spending limits by category.
  - **Limit**: the maximum amount of expenses per month.
  - **Notifications**: a warning when the 80% and 100% limits are reached.


### 5. Reporting
- ** Standard Reports**: Generation of reports for the selected period.
  - **Types of reports**: income vs expenses, expenses by category.
  - **Visualization**: depending on the type of report, for example:
  ===== JANUARY 2024 =====
  Income: 50,000 ₽
  Expenses: 35,000 ₽
  Food category: 15,000 ₽ (75% of the limit)








