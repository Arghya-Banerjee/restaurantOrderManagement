# 🍽️ Restaurant Management System

A highly scalable, secure, and modular **Restaurant Management System** built using **C# ASP.NET Core 8 MVC**, **MS SQL Server**, and a touch of **JQuery & Ajax**. This system is designed to streamline restaurant operations such as order management, stock tracking, transaction history, and daily tasks.

---

## 🚀 **Objective**
The main goal of this project is to ease the management of orders, stock, and day-to-day operations in slightly tech-savvy and ever-growing restaurants.

---

## 🛠️ **Technology Stack**
- **Backend Framework:** C# ASP.NET Core 8 MVC
- **Frontend:** HTML, CSS, JQuery, Ajax
- **Database:** Microsoft SQL Server
- **Architecture:** Microservices with MVC framework
- **Security:** Industry-standard secure database querying

---

## 📋 **Key Features**
1. **Order Management**  
   Efficiently place, edit, and track customer orders.
2. **Stock Management** *(Coming Soon)*  
   Monitor and manage restaurant inventory in real-time.
3. **Transaction History**  
   Maintain and review records of transactions with ease.
4. **Daily Management**  
   Handle tasks like table assignments and routine operations.

---

## 📐 **System Design**
### High-Level Architecture
The system is built using a **microservices architecture** for modularity and scalability. Each core feature (e.g., order management, transaction history) is handled by separate controllers for better maintainability. The **MVC framework** ensures a structured separation of concerns:
- **Model:** Handles business logic and database interactions.
- **View:** Presents data to users via dynamic and responsive UI.
- **Controller:** Acts as the bridge between the view and model.

### Database Schema
![image](https://github.com/user-attachments/assets/e884c430-59fa-480f-aeab-84b324c4bf15)

#### UserMaster Table
- **Purpose**: Stores details of all users in the system, such as waiters and administrators.
- **Attributes**:
  - `Id` (Primary Key): Unique identifier for each user.
  - `MobileNumber`: Contact number of the user.
  - `Email`: Email address of the user.
  - `UserId`: Unique user identifier (could be a username).
  - `PassCode`: Password for user authentication.
  - `IsActive`: Status of the user (e.g., 1 for active, 0 for inactive).
  - `UserType`: Identifies the role of the user (e.g., waiter, admin).
- **Relationships**: This table does not directly link to others but is crucial for managing access and permissions.

#### CategoryMaster Table
- **Purpose**: Serves as a reference table for food categories (e.g., appetizers, main course, desserts).
- **Attributes**:
  - `CatId` (Primary Key): Unique identifier for each food category.
  - `CategoryName`: Name of the category (e.g., "Beverages", "Starters").
- **Relationships**:
  - Links to the `Menu` table through the `FoodCategoryId` column in the `Menu` table.

#### Invoice Table
- **Purpose**: Tracks payment transactions for orders.
- **Attributes**:
  - `InvId` (Primary Key): Unique identifier for each invoice.
  - `InvDate`: Date and time of the invoice.
  - `OrderId` (Foreign Key): Links the invoice to a specific order in the `OrderHeader` table.
  - `OrderAmtExclGST`: Amount of the order excluding GST.
  - `GSTAmt`: GST amount applied to the order.
  - `OrderAmtInclGST`: Total amount of the order including GST.
  - `PaymentMode`: Payment method (e.g., cash, card, UPI).
  - `CreatedBy`: The user who created the invoice.
  - `CreatedOn`: Date and time the invoice was created.
- **Relationships**:
  - Links to the `OrderHeader` table through `OrderId`.

#### Menu Table
- **Purpose**: Contains details about the restaurant’s menu items.
- **Attributes**:
  - `MenuId` (Primary Key): Unique identifier for each menu item.
  - `FoodName`: Name of the dish (e.g., "Paneer Tikka").
  - `FoodDescription`: Description of the dish (e.g., ingredients, preparation style).
  - `FoodCategoryId` (Foreign Key): Links to the `CategoryMaster` table to identify the category of the dish.
  - `FoodPrice`: Price of the dish.
  - `FoodAvailable`: Availability status (e.g., 1 for available, 0 for unavailable).
  - `CreatedBy`: User who added the menu item.
  - `CreatedOn`: Timestamp when the menu item was created.
- **Relationships**:
  - Links to the `CategoryMaster` table via `FoodCategoryId`.

#### OrderHeader Table
- **Purpose**: Acts as the main table for tracking customer orders.
- **Attributes**:
  - `OrderId` (Primary Key): Unique identifier for each order.
  - `OrderDate`: Date of the order.
  - `DeliveryMode`: Mode of delivery (e.g., dine-in, takeaway, home delivery).
  - `TableNo`: Table number for dine-in orders.
  - `CreateBy`: The user who created the order.
  - `CreatedOn`: Timestamp when the order was created.
  - `OrderStatus`: Status of the order (e.g., pending, completed, canceled).
- **Relationships**:
  - Links to the `OrderDetail` table via `OrderId`.
  - Links to the `Invoice` table via `OrderId`.

#### OrderDetail Table
- **Purpose**: Tracks individual dishes ordered in an order.
- **Attributes**:
  - `OrderDtlId` (Primary Key): Unique identifier for each order detail record.
  - `OrderId` (Foreign Key): Links to the `OrderHeader` table to group items under a single order.
  - `MenuId` (Foreign Key): Links to the `Menu` table to identify the dish ordered.
  - `NoOfItem`: Number of items ordered for the specific dish.
  - `CreatedBy`: User who recorded the order detail.
  - `CreatedOn`: Timestamp when the order detail was created.
- **Relationships**:
  - Links to the `OrderHeader` table via `OrderId`.
  - Links to the `Menu` table via `MenuId`.

#### Key Relationships
- **Menu to CategoryMaster**: Each menu item belongs to a specific category (`FoodCategoryId` → `CatId`).
- **OrderHeader to OrderDetail**: Each order can have multiple dishes, but each dish belongs to one order (`OrderId` → `OrderId`).
- **OrderHeader to Invoice**: Each order has one corresponding invoice (`OrderId` → `OrderId`).
- **OrderDetail to Menu**: Tracks the specific dishes ordered and their details (`MenuId` → `MenuId`).

#### Why This Design is Effective
1. **Normalization**: The tables are properly normalized to eliminate redundancy:
   - Categories and menu items are stored separately, reducing duplication.
   - Orders and order details are segregated to allow flexibility in managing individual dishes.
2. **Scalability**: New categories, dishes, or payment modes can be easily added without affecting the existing structure.
3. **Clear Relationships**: Foreign key constraints ensure data integrity between related tables.
4. **Flexibility**: The separation of `OrderHeader` and `OrderDetail` tables allows handling complex orders with multiple items seamlessly.

#### Conclusion
This ER Diagram represents a highly structured and scalable database for a Restaurant Management System. It supports essential functionalities like managing users, categorizing food items, tracking orders, and handling invoices effectively. The relationships between tables ensure data integrity and easy navigation within the system.


---

## 🔒 **Non-Functional Requirements**
- **Scalable:** Modular design to support future feature additions and user growth.
- **Secure:** Adheres to industry-standard secure coding practices.
- **Performance:** Optimized for fast response times and efficient database queries.
- **Reliable:** Fault-tolerant to ensure uninterrupted service.

---

## 🚧 **Project Status**
- **Development:** Work in progress (Stock management module under development).
- **Deployment:** Not yet deployed.

---

## 🖥️ **Use Cases**
1. **Waiters:** Take and manage customer orders with ease.
2. **Chefs:** View real-time order details for preparation.
3. **Admins:** Monitor transactions, update stocks, and oversee restaurant operations.

---

## 🌟 **Future Enhancements**
1. **Integration of Stock Management Module.**  
2. **Customer-facing Module** for reservations and feedback.  
3. **Cloud Deployment** for enhanced accessibility and scalability.

---

## 🧑‍💻 **Developer**
This project is being developed by **Arghya Banerjee**, an individual developer building this system for free.

---

## 🤝 **Contributions**
While this project is currently a solo effort, contributions and suggestions are always welcome! Feel free to fork the repo and raise a pull request.
