# Healthcare Appointment Booking API

## Overview
This API allows users to register, log in, book, view, cancel and complete healthcare appointments with healthcare professionals. It ensures that no double booking occurs and that appointments can only be canceled with appropriate restrictions. The API uses token-based authentication for secure access and integrates with a MySQL database for data storage.

## Features

- **User Registration & Login**: Allows users to register and log in using token-based authentication (JWT).
- **Healthcare Professional Management**: List all available healthcare professionals.
- **Appointment Booking**: Book an appointment with a healthcare professional, ensuring no double booking.
- **View Appointments**: Users can view all their scheduled appointments.
- **Cancel Appointment**: Users can cancel appointments with constraints (e.g., cannot cancel within 24 hours of the appointment).
- **Complete Appointment**: Users can complete appointments with constraints (e.g., cannot complete prior schedule time of the appointment).

## Technologies Used

- **.NET Core** (.net8.0) for backend API development
- **MySQL** for data storage
- **JWT** for token-based authentication
- **Entity Framework Core** for ORM

## Installation & Setup

### Prerequisites

- .NET Core SDK (latest version)
- MySQL database server running locally or remotely

### Steps to Run Locally

1. **Clone the repository**
2. **Set up MySQL Database**:

   - Ensure that your MySQL server is running and create a new database.
   - You can use the following SQL script to create the necessary tables:

   **Seed.sql:**

        CREATE DATABASE HealthBook;

        USE HealthBook;

        CREATE TABLE Users (
            Id INT AUTO_INCREMENT PRIMARY KEY,
            Name VARCHAR(100),
            Email VARCHAR(100) UNIQUE,
            Password VARCHAR(100)
        );

        CREATE TABLE HealthProfessionals (
            Id INT AUTO_INCREMENT PRIMARY KEY,
            Name VARCHAR(100),
            Specialty VARCHAR(100)
        );

        CREATE TABLE Appointments (
            Id INT AUTO_INCREMENT PRIMARY KEY,
            UserId INT,
            HealthProfessionalId INT,
            AppointmentStartTime DATETIME,
            AppointmentEndTime DATETIME,
            Status VARCHAR(50),
            FOREIGN KEY (UserId) REFERENCES Users(Id),
            FOREIGN KEY (HealthProfessionalId) REFERENCES HealthProfessionals(Id)
        );

        
        INSERT INTO Users (Name, Email, Password) VALUES ('Mayur Gupta', 'mayur.g@gmail.com', 'AMzRt4PqSvQZG/hUdwoplqChKJ4F8W8OhI1+lXc991J9bmLiRLg0x5RbpAdWDmHkxw=='); --Original Password: Password123
        INSERT INTO HealthProfessionals (Name, Specialty) VALUES ('Dr. Meena Shah', 'Cardiology');
        INSERT INTO HealthProfessionals (Name, Specialty) VALUES ('Dr. Jeevan Joshi', 'Orthopedic');

3. **Configure the Database Connection**:

   - Open `appsettings.json` and set your MySQL connection string:
   
     ```json
     "ConnectionStrings": {
         "DefaultConnection": "Server=localhost;Database=healthbook;User=root;Password=qwerty123;"
     }
     ```

4. **Run the Application**

5. **Access the API**:

   - The API will be available at `https://localhost:7020`.

### API Endpoints

#### 1. **User Registration & Login**

- **POST** `/api/user/register`: Registers a new user.
- **POST** `/api/user/login`: Logs in a user and returns a JWT token.
- **GET** `/api/user/professionals`: Lists all available healthcare professionals.

#### 2. **Appointments**

- **POST** `/api/appointment/book`: Books a new appointment.
- **GET** `/api/appointment/appointments/{id}`: Lists all appointments for the selected user.
- **PUT** `/api/appointment/cancel/{id}`: Cancels an appointment (must be at least 24 hours before the appointment time).
- **PUT** `/api/appointments/complete/{id}`: Marks an appointment as completed.

### Future Improvements

- Implement email verification for registration.
- Implement user role management (e.g., health professionals can view their own appointments).
- Implement appointment reschedule functionality.
- Add email and sms notifications for booking confirmations or reschedule or cancellations.
- Provide a user-friendly UI for interacting with the API.
- Integrate payment gateway for online payment.
- Implement logger for debugging purpose.
- Implement user profile, diagnosis and medication prescription history management.