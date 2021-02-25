Feature: LotteryVendor
	Prove 1) manual tickets can be sold
	Prove 2) quickPick tickets can be sold

@30KquickTickets
Scenario: Vendor sells 30000 quickTickets
	Given a new period
	When vendor sells 30000 quickPick tickets
	Then the period should have another 30000 tickets added to stack

@1manualTicket
Scenario:  Vendor sells 1 manual ticket
	Given a new period
	When vendor sells 1 custom ticket
	Then the period should have another 1 tickets added to stack

@3xThreads
Scenario: Threads sell X tickets each (3x)
	Given a new period
	When Three background threads sell 1000000 tickets each
	Then the period should have another 3000000 tickets added to stack


