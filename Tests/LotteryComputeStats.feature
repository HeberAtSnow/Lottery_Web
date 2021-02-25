Feature: LotteryComputeStats
    Prove 1) ability to 'tally' the winning tickets
	Prove 2) ability to 'tally' the loosing tickets
	Prove 3) ability to extract winning tickets for a given player
	Prove 4) ability to extract loosing tickets for a given player

  Background: 
    Given a new period 
	And the winning ticket is 1,2,3,4,5,6

  Scenario: Can tally a grand prize winning ticket
	Given a ticket was sold to Bob with the numbers 1,2,3,4,5,6
	And a ticket was sold to Sally with the numbers 1,2,3,4,5,6
	And a ticket was sold to Sue with the numbers 8,9,10,11,12,13
	When statistics are computed
	Then the count of winning tickets should be 2
	And the count of losing tickets should be 1

  Scenario:  Balls purchased out of order are still stored in order
    Given a ticket was sold to Bob with the numbers 5,2,4,1,3,6
	When statistics are computed
	Then the results for Bob should be 
	| b0 | b1 | b2 | b3 | b4 | pb | type   | winLevel | winAmt |
	| 1  | 2  | 3  | 4  | 5  | 6  | custom |1        | 40000000 |
	

  Scenario: Getting player stats by playerName
	Given a ticket was sold to Bob with the numbers 1,2,3,4,5,6
	And a ticket was sold to Bob with the numbers 7,8,9,10,11,12
	And a ticket was sold to Sally with the numbers 1,2,3,4,5,26
	And a ticket was sold to Sue with the numbers 19,20,21,22,23,24
	When statistics are computed
	Then Bob should have 1 winning tickets
	And Sue should have 1 losing tickets
	And Sally should have 1 winning tickets
	And Bob should have 1 losing tickets
	And the results for Bob should be 
	| b0 | b1 | b2 | b3 | b4 | pb | type   | winLevel | winAmt   |
	| 1  | 2  | 3  | 4  | 5  | 6  | custom | 1        | 40000000 |
	| 7  | 8  | 9  | 10 | 11 | 12 | custom |0        | 0        |
	And the results for Sally should be
	| b0 | b1 | b2 | b3 | b4 | pb | type   | winLevel | winAmt |
	| 1  | 2  | 3  | 4  | 5  | 26 | custom |2        | 1000000 |
