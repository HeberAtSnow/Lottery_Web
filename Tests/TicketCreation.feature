Feature: TicketCreation


// 0 - happy path, all balls present and valid
// 1 - missing balls
// 2 - white balls high/low
// 3 - powerball too high/low


A short summary of the feature

@tag1
Scenario: creating a valid ticket, but no username is specified
	Given my six numbers are 1,2,3,4,5,6
	When I create a ticket
	Then the ticket is valid

@BobValidTicket
Scenario: creating a valid ticket
	Given username is Bob
	And my six numbers are 1,2,3,4,5,6
	When I create a ticket
	Then the ticket is valid

Scenario: creating a ticket with Powerball too high
	Given username is Bob
	And my six numbers are 1,2,3,4,5,27
	When I create a ticket
	Then the ticket is not valid
	

Scenario: creating a ticket with Powerball too low
	Given username is Bob
	And my six numbers are 1,2,3,4,5,0
	When I create a ticket
	Then the ticket is not valid
	
Scenario Outline: Creating Tickets
	Given username is Bob
	And my six numbers are <numbers>
	When I create a ticket
	Then the ticket <isOrIsNot> valid
Examples: 
	| numbers      | isOrIsNot | reason                         |
	| 1,2,3,4,5,6  | is        | happy path - valid ticket      |
	| 1,2,3,4,5,27 | is not    | Powerball too large a value    |
	| 1,2,3,4,5,0  | is not    | Powerball too small of a value |
	| 1,2,0,4,5,6  | is not    | Whiteball too small of a value |
	| 1,2,70,4,5,6 | is not    | Whiteball too large of a value |


Scenario: creating a ticket without all numbers present
	Given username is Bob
	And my six numbers are 1,2,3,4,5
	When I create a ticket
	Then the ticket is not valid
