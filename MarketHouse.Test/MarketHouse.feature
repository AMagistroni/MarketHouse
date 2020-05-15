Feature: Test dell'ordine e del magazzino

Background:

	Given gli utenti registrati
	| UserId   | Nome  | Cognome | Mail 	      | Indirizzo spedizione | Citta  |
	| BMario   | Mario | Rossi   | m.rossi@gl.com | Via Roma 2           | Milano |
	| MBianchi | Marco | Bianchi | m.b@gl.it      | Via Roma 2           | Milano |

@Ordine
Scenario: Viene effettuato un ordine 

	Given Il magazzino
	| Codice | Prodotto | Quantità | Unita di misura | Soglia di alert |
	| P1     | Pomodori | 250      | Cassette        | 25              |
	| V1     | Vino DOC | 350      | Bottiglie       | 40              |

	When Arriva un ordine
	| Utente | Prodotto | Quantità | 
	| BMario | P1       | 2        |
	| BMario | V1       | 1        |

	Then il magazzino contiene questi prodotti
	| Codice | Prodotto | Quantità | 
	| P1     | Pomodori | 248      | 
	| V1     | Vino DOC | 349      | 

@Ordine
Scenario: Viene effettuato un ordine che fa abbassare la quantità dei prodotti sotto la soglia 

	Given Il magazzino
	| Codice | Prodotto | Quantità | Unita di misura | Soglia di alert |
	| P1     | Pomodori | 26       | Cassette        | 25              |
	| V1     | Vino DOC | 350      | Bottiglie       | 40              |

	When Arriva un ordine
	| Utente | Prodotto | Quantità | 
	| BMario | P1       | 2        |
	| BMario | V1       | 1        |

	Then il magazzino contiene questi prodotti
	| Codice | Prodotto | Quantità | 
	| P1     | Pomodori | 24       | 
	| V1     | Vino DOC | 349      | 

	Then viene avvertito l'ufficio Acquisti
	| Prodotti sotto soglia | Quantità | Soglia |
	| P1                    | 24       | 25     |