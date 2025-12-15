### HRMS Automation Testing
### Overview

This repository contains a UI automation framework built to test a Human Resource Management System (HRMS) web application.
The framework validates end-to-end HR and Employee workflows using BDD principles and a Page Object Model (POM) architecture.

It focuses on real-world scenarios, clean design, and maintainability — suitable for enterprise-level applications.

### Tech Stack

- Language: C#

- Automation Tool: Selenium WebDriver

- BDD Framework: Reqnroll

- Test Framework: MSTest / NUnit

- Test Data: Bogus (Faker)

- Design Pattern: Page Object Model (POM)

### Automated Scenarios

- HR login and dashboard validation

- Employee onboarding (multi-step form)

- Employee visibility verification

- Employee deletion and confirmation

- Role-based UI flows (HR & Employee)

- End-to-end HR workflow validation

### Project Structure
HRMS-Automation-Testing

├── Features/              # Gherkin feature files (BDD scenarios)

├── StepDefinitions/       # Step implementation files

├── PageObjects/           # Page Object Model

│   ├── HR/                # HR-specific pages

│   ├── Employee/          # Employee-specific pages

│   └── Common/            # Shared UI components

├── Hooks/                 # Test setup and teardown logic

├── Models/                # Request/response & UI models

├── TestData/              # Dynamic test data generation (Bogus)

├── WebDriver/             # Browser initialization and management

├── Helpers/Extensions/    # Utility methods and extensions

├── Enums/                 # Application constants and enums

├── Config/                # Environment and configuration settings

└── README.md

### Key Highlights

- Dynamic test data using Bogus (no hardcoded values)

- ScenarioContext for data sharing across steps

- Clean role-based POM structure (HR vs Employee)

- Reusable components and utilities

- End-to-end business flow validation

- Easily scalable for additional modules

### Project Status
- Portfolio automation framework validating real-world HRMS workflows
   
- Actively maintained and extended with additional scenarios and roles
  
### Future Enhancements

- API + UI hybrid testing

- Parallel execution

- CI/CD integration

- Reporting (Extent / Allure)
