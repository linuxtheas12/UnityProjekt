-> main
VAR correct = "To get your wires crossed"
VAR chosen = ""

=== main ===
Mythical Wise Tree : Which idiom means to have a misunderstanding or to be confused?
    + To get your wires crossed
        ~ chosen = "To get your wires crossed"
        #CORRECT_CHOICE
        -> check_choice
    + Light years ahead
        ~ chosen = "Light years ahead"
        #WRONG_CHOICE
        -> check_choice
    + Cutting edge
        ~ chosen = "Cutting edge"
        #WRONG_CHOICE
        -> check_choice
        

=== check_choice ===
Barbas : {chosen == correct: Exactly! Our wires are perfectly connected now! | Oh no, total confusion! Try to untangle those wires.}
-> END