Feature: ClosePeriod
	Simple calculator for adding two numbers

Background: 
    Given a new period 
	And the winning ticket is 1,2,3,4,5,6

@db-connect-version
 Scenario: database can connect
	When period is closed
	Then DB should answer version

@db-save
Scenario: database saves tickets
	Given a ticket was sold to Bob with the numbers 1,2,3,4,5,6
	And a ticket was sold to Sally with the numbers 1,2,3,4,5,6
	And a ticket was sold to Sue with the numbers 8,9,10,11,12,13
	When period is closed 
	And statistics are computed
	Then the database will have stats of winning tickets count = 2 and losing tickets count = 1

