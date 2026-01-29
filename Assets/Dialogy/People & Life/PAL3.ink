-> main
VAR correct = "To be in the same boat"
VAR chosen = ""

=== main ===
Savage Camel : Which idiom means to be in the same difficult situation as someone else?
   + To be in the same boat
        ~ chosen = "To be in the same boat" 
        #CORRECT_CHOICE
        -> check_choice
    + A lone wolf
        ~ chosen = "A lone wolf" 
        #WRONG_CHOICE
        -> check_choice
    + To be under the weather
        ~ chosen = "To be under the weather" 
        #WRONG_CHOICE
        -> check_choice
        

=== check_choice ===
Barbas : {chosen == correct: Row, row, row your boat! We are together in this! | No, we are sinking! Think about a vessel.}
-> END