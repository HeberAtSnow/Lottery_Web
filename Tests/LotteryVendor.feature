Feature: LotteryVendor
	Prove 1) manual tickets can be sold
	Prove 2) quickPick tickets can be sold

@mytag
Scenario: Vendor sells 10 quickTickets
	Given a new period
	When vendor sells 10 quickPick tickets
	Then the period should have another 10 tickets added to stack

Scenario:  Vendor sells 1 manual ticket
	Given a new period
	When vendor sells 1 custom ticket
	Then the period should have another 1 tickets added to stack