Feature: Order and warehouse testing

Background:

	Given registered users 
	| UserId | Name  | Surname | Mail             | Delivery address | City     |
	| AJ     | John  | Red     | j.red@red.com    | Down street      | London   |
	| MWitch | Marck | Witch   | Mark.Witch@gl.it | High street      | New york |

@Orders
Scenario: An order is submitted 

	Given The warehouse
	| Code | Products | Quantity | Unit of measure | Alert threshold |
	| P1   | Tomato   | 250      | Box             | 25              |
	| V1   | Wine     | 350      | Bottle          | 40              |

	When An order arrives
	| User | Products | Quantity |
	| AJ   | P1       | 2        |
	| AJ   | V1       | 1        |

	Then The warehouse contains these products
	| Code | Products | Quantity |
	| P1   | Tomato   | 248      |
	| V1   | Wine     | 349      |

	Then the Purchasing Office is notified
	| Product under threshold | Quantity | Threshold |

@Orders
Scenario: An order is placed that lowers the quantity of the products under the threshold 

	Given The warehouse
	| Code | Products | Quantity | Unit of measure | Alert threshold |
	| P1   | Tomato   | 26       | Box             | 25              |
	| V1   | Wine     | 350      | Bottle          | 40              |

	When An order arrives
	| User | Products | Quantity |
	| AJ   | P1       | 2        |
	| AJ   | V1       | 1        |

	Then The warehouse contains these products
	| Code | Products | Quantity |
	| P1   | Tomato   | 24       |
	| V1   | Wine     | 349      |

	Then the Purchasing Office is notified
	| Product under threshold | Quantity | Threshold |
	| P1                      | 24       | 25        |