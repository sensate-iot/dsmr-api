# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.6.0] - 18-06-2021
### Added
- Sensor availability per device

### Updated
- Devices DTO
- Users controller

## [1.5.0] - 06-06-2021
### Added
- Energy usage endpoint

### Updated
- Energy usage computation
- Swagger documentation

## [1.4.2] - 30-05-2021
### Updated
- Logout route method name
- Users controller
- Logout route operation ID (swagger)

## [1.4.1] - 30-05-2021
### Updated
- Logout operation ID
- User controller

## [1.4.0] - 02-05-2021
### Added
- Logout HTTP route
- OTP request route

### Updated
- The authentication flow

## [1.3.0] - 27-04-2021
### Added
- Hourly power data aggregate endpoint
- Data access for the [dbo].[DsmrApi_SelectPowerDataByHour] stored procedure

### Updated
- Security policy

## [1.2.0] - 18-04-2021
### Added
- Authorization filter attributes
- User login endpoint

### Updated
- All secure endpoints: added the authorization attribute

### Removed
- Authorization middleware
- Request body logging

## [1.1.0] - 17-04-2021
### Added
- Users controller
- Response abstraction (base controller)

### Updated
- Product token authorization: always allow swagger

## [1.0.2] - 15-04-2021
### Added
- Implement CORS handling

### Updated
- Project dependency's
- Code formatting

## [1.0.1] - 11-04-2021
### Added
- Improve swagger documentation
- Add swagger annotations

### Updated
- Build configuration
- Documentation generation options

## [1.0.0] 11-04-2021
### Added
- Device data access
- User/authentication data access
- Dynamic authorization/authentication

### Updated
- Controllers to include authorization
- Models and DTO definitions
- Project administration
