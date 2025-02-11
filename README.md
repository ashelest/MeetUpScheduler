# 📅 Appointment Booking API

Backend API for an appointment booking system with sales managers. Allows customers to find available time slots based on specific criteria.

---

## 🚀 Main Features
✅ Returns available slots for booking  
✅ Filters slots by language, products, and customer rating  
✅ Prevents double booking of managers  
✅ Uses PostgreSQL as the database  
✅ Includes Docker for easy deployment

---

## ⚙️ Installation & Setup

### 📌 1. Prerequisites
Before starting, make sure you have installed:
- [Docker](https://www.docker.com/) - for the database
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) - to run the API
- [Node.js](https://nodejs.org/) - to run tests

---

### 🛠 2. Start the Database
Navigate to the **setup** folder and execute the **start-db.ps1** script.  
Script will execute 
```docker-compose up -d ```

Once started, the database will be available at:
```
postgres://postgres:mypassword123!@localhost:5432/coding-challenge
```

---

### 🔗 3. Database Connection Configuration
No additional configuration is required!
The application will automatically connect to the database upon startup.

---

### 🚀 4. Run the API
Navigate to the **Setup** folder and execute the **start-api.ps1** script.  
Script will execute
```
cd ../src/WebAPI/
dotnet run --launch-profile "WebAPI"
```

Once running, the API will be available at:
```
http://localhost:3000/calendar/query
```

---

## ✅ Running Tests

1. Navigate to the **tests** folder and Install dependencies:
```sh
cd ./tests
npm install
```

2. Run the tests:
```sh
npm run test
```

The tests will send requests to the API at:
```
http://localhost:3000/calendar/query
```

---

## 🔍 API Endpoint

### 📌 `POST /calendar/query`
This endpoint returns available booking slots.

**Request Example:**
```json
{
  "date": "2024-05-03",
  "products": ["SolarPanels", "Heatpumps"],
  "language": "German",
  "rating": "Gold"
}
```

**Response Example:**
```json
[
  {
    "available_count": 1,
    "start_date": "2024-05-03T10:30:00.00Z"
  },
  {
    "available_count": 2,
    "start_date": "2024-05-03T12:00:00.00Z"
  }
]
```

---

## 👨‍💻 Author
**Oleksandr Shelest**

📧 **Email**: alexandr.shelest.f@gmail.com  
🔗 **GitHub**: [MeetUpScheduler](https://github.com/ashelest/MeetUpScheduler)

