# ASP.NET Core + Angular

## Project Overview

This project consists of two applications:
1. **ASP.NET Core (REST API)** – SmartShopAPI
2. **Angular** – SmartShopUI

Both applications can be run in two ways:
- **Using Docker**
- **Manually (locally) via the console** – requires the appropriate tools and dependencies installed.

## System Requirements

### Docker:
- Docker installed on your machine

### Manual Setup:
- .NET SDK (version 8.0) – download from [here](https://dotnet.microsoft.com/download)
- Node.js (version 20.17.0) – download from [here](https://nodejs.org/)
- Angular CLI (version 18.2.5) – install it globally:
    ```bash
    npm install -g @angular/cli@18.2.5
    ```

---

## Running the Application

### 1. Using Docker

1. Clone the repository:
    ```bash
   git clone https://github.com/robertfisahn/SmartShop
   cd SmartShop
    
2. Run the application using Docker Compose:
    ```bash
   docker-compose up --build
    
3. The application should be accessible at:
   `http://localhost:4288`

### 2. Running Manually

#### Backend (ASP.NET Core)

1. Clone the repository:
    ```bash
    git clone https://github.com/robertfisahn/SmartShop
    cd SmartShop/SmartShopAPI/SmartShopAPI

2. Install dependencies and run the application:
    ```bash
   dotnet restore
   dotnet run

3. The backend should be accessible at: `http://localhost:5108/swagger`

#### Frontend (Angular)

1. Navigate to the Angular folder:  
    ```bash
   cd SmartShop/SmartShopUI/SmartShopUI

2. Install dependencies:
    ```bash
   npm install
    
3. Run the Angular application:
    ```bash
   ng serve
    
4. The frontend will be accessible at: `http://localhost:4200`

---

## User Authentication

You can log into the application using the following demo accounts:

- **Admin Account**:
   - Email: `admin@admin.com`
   - Password: `admin123`
   
- **User Account**:
   - Email: `user@user.com`
   - Password: `user1234`
