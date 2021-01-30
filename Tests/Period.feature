Feature: Period
	Prove 1) winning ticket can be chosen
	Prove 2) tickets can be graded correctly (isGraded=true, winLevel=0-9, winAmtDollars=0-GrandPrize)

@mytag
Scenario Outline: Winning ticket counts verified
	Given a new period
	And a ticket was sold with the numbers <numbers>
	When the winning ticket is 1,2,3,4,5,6
	Then the sold ticket wins at level <winLevel> and prize amt is <winAmtDollars>

Examples:
		| numbers      | winLevel | winAmtDollars | reason   |
		| 1,2,3,4,5,6  | 1        | 40000000      | W:5 PB:T |
		| 1,2,3,4,5,1  | 2        | 1000000       | W:5 PB:F |
		| 6,2,3,4,5,6  | 3        | 50000         | W:4 PB:T |
		| 6,2,3,4,5,1  | 4        | 100           | W:4 PB:F |
		| 6,7,3,4,5,6  | 5        | 100           | W:3 PB:T |
		| 6,7,3,4,5,1  | 6        | 7             | W:3 PB:F |
		| 6,7,8,4,5,6  | 7        | 7             | W:2 PB:T |
		| 6,7,8,9,5,6  | 8        | 4             | W:1 PB:T |
		| 6,7,8,9,10,6 | 9        | 4             | W:0 PB:T |
		| 6,7,8,9,10,1 | 0        | 0             | W:0 PB:F |
