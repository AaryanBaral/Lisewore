# EmployeeManagementService

EmployeeManagementService is a .NET 9.0 REST API for managing employee records and user authentication, ideal for HR or restaurant management systems.

## Installation

Use [Git](https://git-scm.com/) and [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) to set up the project.

```bash
git clone https://github.com/your-repo/EmployeeManagementService.git
cd EmployeeManagementService
dotnet restore
```

Configure the SQL Server connection in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=EmployeeManagement;User Id=sa;Password=YourStrong@Password123;TrustServerCertificate=True;"
  }
}
```

Apply database migrations:

```bash
dotnet ef database update
```

Run the application:

```bash
dotnet run --project EmployeeManagementService
```

## Usage

Access the API at `https://localhost:<port>/swagger` for interactive documentation.

### User Endpoints

Register a user:

```bash
curl -X POST "https://localhost:<port>/api/User/register" -H "Content-Type: application/json" -d '{"email":"user@example.com","userName":"testuser","password":"password123"}'
```

**Response**:
```json
{
    "data": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZCI6IjI2ZDhiMDZjLWNkYTctNDYyOC04NGZjLWIzMTE4MTAyOTc4ZiIsInN1YiI6ImFyeWFuYmFyYWwxMUBnbWFpbC5jb20iLCJlbWFpbCI6ImFyeWFuYmFyYWwxMUBnbWFpbC5jb20iLCJqdGkiOiJlOWQwMDVmYi1jMWQ3LTRiYzAtYTgzNi0yMmUxYTE1YTNjODYiLCJpYXQiOjE3NTA5MzExMTUsIm5iZiI6MTc1MDkzMTExNSwiZXhwIjoxNzUwOTY3MTE1fQ.tF1FT939g-XJNv4P4_l2tvIu-6nvLsRj6pcKe_i8Gos",
    "success": true
}
```

Login to get a JWT token:

```bash
curl -X POST "https://localhost:<port>/api/User/login" -H "Content-Type: application/json" -d '{"email":"user@example.com","password":"password123"}'
```

**Response**:
```json
{
    "data": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZCI6IjI2ZDhiMDZjLWNkYTctNDYyOC04NGZjLWIzMTE4MTAyOTc4ZiIsInN1YiI6ImFyeWFuYmFyYWwxMUBnbWFpbC5jb20iLCJlbWFpbCI6ImFyeWFuYmFyYWwxMUBnbWFpbC5jb20iLCJqdGkiOiJlOWQwMDVmYi1jMWQ3LTRiYzAtYTgzNi0yMmUxYTE1YTNjODYiLCJpYXQiOjE3NTA5MzExMTUsIm5iZiI6MTc1MDkzMTExNSwiZXhwIjoxNzUwOTY3MTE1fQ.tF1FT939g-XJNv4P4_l2tvIu-6nvLsRj6pcKe_i8Gos",
    "success": true
}
```

Get all users:

```bash
curl -X GET "https://localhost:<port>/api/User"
```

**Response**:
```json
{
  "data": [
    {
      "userId": "1",
      "email": "user@example.com",
      "userName": "testuser"
    }
  ]
}
```

Get authenticated user’s details (requires JWT):

```bash
curl -X GET "https://localhost:<port>/api/User/individual" -H "Authorization: Bearer <token>"
```

**Response**:
```json
{
  "data": {
    "userId": "1",
    "email": "user@example.com",
    "userName": "testuser"
  }
}
```

Update authenticated user (requires JWT):

```bash
curl -X PUT "https://localhost:<port>/api/User" -H "Authorization: Bearer <token>" -H "Content-Type: application/json" -d '{"email":"newuser@example.com","userName":"newuser","password":"newpassword123"}'
```

**Response**:
```json
{
  "data": "User successfully updated"
}
```

Delete authenticated user (requires JWT):

```bash
curl -X DELETE "https://localhost:<port>/api/User/delete" -H "Authorization: Bearer <token>"
```

**Response**:
```json
{
  "data": "User successfully deleted"
}
```

### Employee Endpoints

Get all employees:

```bash
curl -X GET "https://localhost:<port>/api/Employee"
```

**Response**:
```json
{
  "data": [
    {
      "employeeId": "1",
      "firstName": "John",
      "lastName": "Doe",
      "email": "john.doe@example.com",
      "department": "IT",
      "hireDate": "2025-06-26"
    }
  ]
}
```

Get employee by ID:

```bash
curl -X GET "https://localhost:<port>/api/Employee/1"
```

**Response**:
```json
{
  "data": {
    "employeeId": "1",
    "firstName": "John",
    "lastName": "Doe",
    "email": "john.doe@example.com",
    "department": "IT",
    "hireDate": "2025-06-26"
  }
}
```

**Error** (404):
```json
{
  "data": "Employee with given ID not found"
}
```

Create an employee (requires JWT):

```bash
curl -X POST "https://localhost:<port>/api/Employee" -H "Authorization: Bearer <token>" -H "Content-Type: application/json" -d '{"firstName":"John","lastName":"Doe","email":"john.doe@example.com","department":"IT","hireDate":"2025-06-26"}'
```

**Request Body**:
```json
{
  "firstName": "string",
  "lastName": "string",
  "email": "string",
  "department": "string",
  "hireDate": "2025-06-26"
}
```

**Response**:
```json
{
  "data": "1"
}
```

**Error** (400):
```json
{
  "errors": ["Invalid email format"]
}
```

Update an employee (requires JWT):

```bash
curl -X PUT "https://localhost:<port>/api/Employee/1" -H "Authorization: Bearer <token>" -H "Content-Type: application/json" -d '{"firstName":"Jane","lastName":"Doe","email":"jane.doe@example.com","department":"HR","hireDate":"2025-06-26"}'
```

**Request Body**:
```json
{
  "firstName": "string",
  "lastName": "string",
  "email": "string",
  "department": "string",
  "hireDate": "2025-06-26"
}
```

**Response**:
```json
{
  "data": "Employee successfully updated"
}
```

**Errors** (400, 404):
```json
{
  "errors": ["Invalid email format"]
}
```
```json
{
  "data": "Employee with ID 1 not found"
}
```

Delete an employee (requires JWT):

```bash
curl -X DELETE "https://localhost:<port>/api/Employee/1" -H "Authorization: Bearer <token>"
```

**Response**:
```json
{
  "data": "Employee successfully deleted"
}
```

**Error** (404):
```json
{
  "data": "Employee with ID 1 not found"
}
```


