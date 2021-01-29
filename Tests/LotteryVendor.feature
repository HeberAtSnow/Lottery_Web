Feature: LotteryVendor
	Simple calculator for adding two numbers

@mytag
Scenario: Vendor sells 10 quickTickets
	Given a new period with 0 tickets sold
	When vendor sells 10 quickPick tickets
	Then the period should have another 10 tickets added to stack

Scenario:  Vendor sells 1 manual ticket
	Given a new period with 0 tickets sold
	When vendor sells 1 custom ticket
	Then the period should have another 1 tickets added to stack