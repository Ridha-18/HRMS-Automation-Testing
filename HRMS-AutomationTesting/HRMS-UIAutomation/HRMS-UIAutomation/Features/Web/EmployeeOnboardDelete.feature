Feature: Employee Onboard and delete
  
  @UI @Employee
  Scenario: Ensure user can onboard and delete an employee
    #Login
    Given The user has navigated to the login page
    When The user enters valid email and password
    Then The dashboard should be visible
    #Onboard
    When The user clicks on "Onboard Employee" in the left menu
    Then The "Onboard Employee" page should be visible
    Then The user fills in Step 1 details 
    Then The user fills in Step 2 details 
    And The user reviews the details on Step 3
    Then A popup message containing "onboarded successfully" should appear
    When The user clicks on "Employee Management" in the left menu
    Then The generated employee should be visible on the dashboard
    # Delete
    When The user clicks delete icon for that employee
    Then A popup message having "Are you sure you want to delete this employee?" should appear
    Then The employee should no longer be visible on the dashboard
    
