# 🏢 Apartment Management System

![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Windows Forms](https://img.shields.io/badge/Windows%20Forms-blue?style=for-the-badge)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)

**Apartment Management System** is a comprehensive desktop automation project that enables residential complex and apartment managers to easily manage dues, expenses, cash flow, and residents.

This project was developed using **secure coding principles** (SQL Injection protection), **role-based authorization**, and **modern database architecture**.

---

## 🚀 Features

### 👤 Admin Panel
- **Apartment & Resident Management:** Adding apartments, assigning people, and editing.
- **Financial Transactions:** Processing Income (Dues) and Expense (Invoices, Maintenance) items.
- **Debt Assignment:** Bulk or individual debt/dues assignment.
- **Reporting:** Viewing cash status and past transactions.
- **Log System:** IP and User-based logging of performed operations.

### 🏠 User (Resident) Panel
- Personal debt inquiry.
- Viewing payment history.

### 🔒 Technical Specifications
- **Security:** 100% SQL Injection protection with parameterized queries.
- **Authorization:** Admin, Manager, and User roles.
- **Database:** Relational SQL Server database structure.

---

## 📸 Screenshots

| Login Screen | Main Panel |
| :---: | :---: |
|<img width="1217" height="713" alt="1" src="https://github.com/user-attachments/assets/2b06c6c5-a288-4d7d-a2e5-75a1fa8f64eb" /> | <img width="1915" height="1016" alt="2" src="https://github.com/user-attachments/assets/ca083e3e-24bf-4874-8b57-3d20859d43cb" /> |

| Admin Panel | Apartment Operations |
| :---: | :---: |
|<img width="1640" height="676" alt="3" src="https://github.com/user-attachments/assets/d72ed5f6-7dbc-4245-ac18-4e36af16dc5e" /> | <img width="1251" height="586" alt="4" src="https://github.com/user-attachments/assets/57a50931-061e-4a73-a371-8731539eb9a8" /> |

| Apartment Management | Category Operations | Records |
| :---: | :---: | :---: |
|<img width="1072" height="697" alt="5" src="https://github.com/user-attachments/assets/a769ef6e-16a3-40fb-b976-5bd72d287310" /> |<img width="1157" height="666" alt="6" src="https://github.com/user-attachments/assets/3b00e4eb-6b25-4b29-9bff-e67d3051e10d" /> | <img width="972" height="567" alt="7" src="https://github.com/user-attachments/assets/029cb5f5-6f61-4647-8b03-928381312ad0" /> |



---

## 🛠️ Installation and Execution

Follow the steps below to run the project on your local machine:

### 1. Requirements
- Visual Studio 2019 or 2022
- SQL Server Express (LocalDB)
- .NET Framework 4.8

### 2. Database Installation
1. Open the SQL Server Management Studio (SSMS) application.
2. Create a new database named `Apartman_Yonetim`.
3. Open the **`Veritabani_Kurulum.sql`** file located in the project files and run the codes inside (Execute).
4. This process will create the necessary tables and the Admin user.

### 3. Connection Settings
If your SQL Server name is not `.\SQLEXPRESS`, open the `sqlbaglantisi.cs` file in the project and update it with your own server name:
```csharp
public SqlConnection baglan()
{
    // Edit the address here according to your own server
    SqlConnection baglanti = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=Apartman_Yonetim;Integrated Security=True");
    baglanti.Open();
    return baglanti;
}
```

### 4. Execution
- Open the project in Visual Studio.
- Run it by pressing the **Start** button.

---

## 🔑 Login Credentials
Fully authorized admin account after installation:
| Role | Username (ID) | Password |
| :--- | :--- | :--- |
| **Admin** | `admin` | `1234` |

---
## 👨‍💻 Developer
- **Mustafa Tunç**
- 📧 **Email:** [mstftnc421@gmail.com](mailto:mstftnc421@gmail.com)

© Copyright & License
© 2026 Mustafa Tunç. All Rights Reserved.
This repository is strictly for portfolio and academic demonstration purposes. No part of this code may be reproduced, distributed, modified, or used for any commercial or personal projects without explicit written permission from the author.
