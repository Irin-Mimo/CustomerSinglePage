# 🏢 Customer Single Page Management System

A modern **Single Page Management System** built with **ASP.NET Core MVC**, **Entity Framework Core**, **SQL Server**, and **jQuery AJAX**.

This project manages:

* Customers
* Delivery Addresses
* Dealers
* Marketing Employees
* Shops

All operations are handled dynamically without full page reload (AJAX-based).

---

# 🚀 Core Features

## 👤 Customer Management

* Create, Update, Delete Customers
* Upload Customer Photo
* Credit Limit Management
* Customer Type Classification
* Purchase Date Handling
* One-to-Many Relationship with Delivery Addresses

## 📦 Delivery Address Management

* Multiple delivery addresses per customer
* Contact person & phone tracking
* Due amount tracking per address

## 🧑‍💼 Marketing Employee Management

* Add / Update / Delete Marketing Employees
* Individual Color Assignment 🎨
* Marketing-wise Dealer & Shop Mapping
* Dynamic Table Rendering

## 🏬 Shop Management

* Assign Marketing Employee to Shop
* Pagination (Server-side)
* Live Search (Shop, Owner, Phone, Address, Marketing Name)
* Sorting by Shop Name
* Row Color Highlight Based on Marketing Employee
* Visit Date Formatting

## 🏢 Dealer Management

* Dealer CRUD Operations
* Marketing-wise Dealer Assignment
* Due Amount Tracking
* Date Tracking

---

# 🛠️ Technologies Used

* ASP.NET Core MVC (.NET 6/7)
* Entity Framework Core
* SQL Server
* jQuery AJAX
* Bootstrap 5
* SweetAlert2
* Bootstrap Icons

---

# 🗂️ Database Models

## Customer

* CustomerId
* Name
* Address
* PurchaseDate
* Phone
* CustomerType
* CreditLimit
* Photo
* DeliveryAddresses (One-to-Many)

## DeliveryAddress

* DeliveryAddressId
* CustomerId (FK)
* ContactPerson
* Phone
* Address
* DueAmount

## Marketing

* MarketingId
* Name
* Address
* Phone
* Color 🎨
* Dealers (One-to-Many)

## Dealer

* DealerId
* Name
* Address
* Phone
* DueAmount
* MarketingId (FK)
* Date

## Shop

* Id
* ShopName
* OwnerName
* Phone
* Address
* Date
* MarketingId (FK)

---

# 🔗 Relationships

Customer (1) → (Many) DeliveryAddress
Marketing (1) → (Many) Dealer
Marketing (1) → (Many) Shop

---

# 📊 Pagination & Search System

Implemented in:

* `ShopController`
* Server-side filtering
* Search supported fields:

  * Shop Name
  * Owner Name
  * Phone
  * Address
  * Marketing Employee Name

---

# 🎨 Marketing Color Highlight System

Each Marketing Employee has a unique color stored in:

```csharp
public string Color { get; set; }
```

Used to:

* Highlight Marketing rows
* Highlight Shop rows (Left border + light background)

---

# 🧩 Controllers

* CustomerController
* MarketingController
* DealerController
* ShopController

All controllers use:

* AJAX-based CRUD
* SweetAlert confirmation
* JSON responses

---

# ⚙️ Installation Guide

1️⃣ Clone the repository

2️⃣ Update connection string in `appsettings.json`

3️⃣ Run migrations:

```powershell
Add-Migration InitialCreate
Update-Database
```

4️⃣ Run the project:

```powershell
dotnet run
```

---

# 📌 Future Improvements

* Authentication & Role-based Authorization
* Dashboard with Charts (Sales / Due Report)
* Export to Excel / PDF
* Commission Tracking System
* REST API for React Frontend

---

# 👩‍💻 Developed By

Irin Sarker Mim
ASP.NET Core Developer 🚀

---
