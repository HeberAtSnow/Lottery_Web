Feature: LotteryComputeStats
    Prove 1) ability to 'tally' the winning tickets
	Prove 2) ability to 'tally' the loosing tickets
	Prove 3) ability to extract winning tickets for a given player
	Prove 4) ability to extract loosing tickets for a given player

@mytag
Scenario: Can tally a grand prize winning ticket
	Given a new period
	And a ticket was sold to playerName with the numbers {numbers}
	And a ticket was sold to playerName with the numbers {numbers}
	And a ticket was sold to playerName with the numbers {numbers}
	And the winning ticket is 1,2,3,4,5,6
	When statistics are computed
	Then the count of winning tickets should be WTcount
	And the count of losing tickets should be LTcount

Scenario: Getting player stats by playerName
	Given a new period
	And a ticket was sold to playerName with the numbers {numbers}
	And a ticket was sold to playerName with the numbers {numbers}
	And a ticket was sold to playerName with the numbers {numbers}
	And a ticket was sold to playerName with the numbers {numbers}
	And the winning ticket is 1,2,3,4,5,6
	When statistics are computed
	Then playerName should have N winning tickets
	And playerName should have N losing tickets
	And playerName should have N losing tickets
	And playerName should have N winning tickets
