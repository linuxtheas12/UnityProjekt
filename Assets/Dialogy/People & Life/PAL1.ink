-> main
VAR correct = "A lone wolf"
VAR chosen = ""

=== main ===
Mythical Wise Tree : What do we call a person who prefers to be alone and act independently?
    + A social butterfly
        ~ chosen = "A social butterfly"
        #WRONG_CHOICE
        -> check_choice
    + A lone wolf
        ~ chosen = "A lone wolf"
        #CORRECT_CHOICE
        -> check_choice
    + A dark horse
        ~ chosen = "A dark horse"
        #WRONG_CHOICE
        -> check_choice
        

=== check_choice ===
Barbas : {chosen == correct: Awoo! The lone wolf is correct! | Not quite, think about a solitary animal.}
-> END